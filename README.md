# Atlas grzybów

Aplikacja dla grzybiarzy: mapa znalezisk, przewidywanie gdzie i jakich grzybów można się spodziewać
oraz forum do dyskusji i dzielenia się znaleziskami.

Projekt na przedmiot *Projektowanie usług dziedzinowych w Infrastrukturze Chmurowej*.
**Etap 1: demo technologiczne**: wszystkie serwisy i building blocki działają end-to-end,
logika biznesowa jest celowo prosta.

## Architektura

```
                    Cloudflare Tunnel (atlas.home-apps.dev)
                                   │
                         ┌─────────▼─────────┐
                         │  Bramka (YARP)    │  :8080
                         └─┬───┬───┬───┬───┬─┘
       /api/users  ────────┘   │   │   │   └──────── /  → frontend (Vue 3 + Leaflet, nginx)
       /api/forum  ────────────┘   │   └──────────── /api/prediction → Predykcja (Python/FastAPI)
       /api/map    ────────────────┘
   ┌──────────────┐   ┌──────────────┐   ┌──────────────┐
   │ Użytkownicy  │   │    Forum     │   │     Mapa     │   .NET 10, Clean Architecture
   │ JWT, konta   │   │ wątki, posty │   │ PostGIS      │
   └──────┬───────┘   └──────▲───────┘   └──────┬───────┘
          │ UserRegistered   │ FindingReported  │
          └──────────────► RabbitMQ (MassTransit 8) ◄────┘
                PostgreSQL 17 + PostGIS: osobna baza na serwis
```

| Serwis | Technologia | Odpowiada za | Endpointy (przez bramkę) |
|---|---|---|---|
| Użytkownicy | .NET 10, EF Core, JWT | rejestracja, logowanie, wydawanie tokenów | `POST /api/users/auth/register`, `POST /api/users/auth/login`, `GET /api/users/me` |
| Forum | .NET 10, EF Core, MassTransit | wątki, posty, wątki z nowych znalezisk | `GET/POST /api/forum/threads`, `GET /api/forum/threads/{id}`, `POST /api/forum/threads/{id}/posts` |
| Mapa | .NET 10, EF Core + NetTopologySuite, PostGIS | znaleziska, słownik gatunków | `GET/POST /api/map/findings`, `GET /api/map/species` |
| Predykcja | Python 3.13, FastAPI | prognoza dla punktu, warstwa szans na mapie | `GET /api/prediction/forecast?lat&lon`, `GET /api/prediction/heatmap?south&west&north&east` |
| Bramka | YARP | routing `/api/*` i frontendu | `GET /health` |

Każdy serwis ma `GET /health`. Tokeny JWT wydaje serwis Użytkownicy, a każdy serwis .NET weryfikuje je sam
(wspólny klucz `Jwt:SigningKey`).

**Przepływ zdarzeń (demo RabbitMQ):** dodanie znaleziska na mapie → Mapa publikuje `FindingReported`
→ Forum konsumuje i zakłada wątek w kategorii *Znaleziska*.

## Struktura repozytorium

```
src/
  BuildingBlocks/         wspólne klocki dla serwisów .NET
    Atlas.Kit.Domain          AppError, ErrorKind (FluentResults)
    Atlas.Kit.Application     MediatR + walidacja FluentValidation, port IIntegrationEventPublisher
    Atlas.Kit.Infrastructure  MassTransit/RabbitMQ, migracje i seedery, JwtOptions
    Atlas.Kit.Api             JWT, IEndpoint (Minimal API), ProblemDetails, OpenAPI, /health
    Atlas.Contracts           zdarzenia integracyjne (jedna wiadomość na plik)
  Services/
    Users/  Forum/  Map/  każdy: Domain → Application → Infrastructure → Api + Dockerfile
    Prediction/           FastAPI (app/), testy (tests/), Dockerfile
  Gateway/Atlas.Gateway   YARP
frontend/                 Vue 3 + Vite + TypeScript + Leaflet
tests/                    testy domeny serwisów .NET
docker/postgres/init.sql  tworzy bazy atlas_users, atlas_forum, atlas_map
```

Konwencje .NET: jeden typ na plik, warstwy Clean Architecture, jeden folder na komendę/zapytanie
(`Threads/CreateThread/CreateThreadCommand.cs`, `...Validator.cs`, `...Handler.cs`).

## Uruchomienie (wszystko w Dockerze)

```bash
cp .env.example .env        # opcjonalnie: zmień hasła i klucz JWT
docker compose up -d --build
```

- aplikacja: http://localhost:8080
- panel RabbitMQ: http://localhost:15672 (atlas / atlas)
- PostgreSQL: localhost:5432 (atlas / atlas)

Konto demo: **demo@atlas.pl / Grzybek123!** Bazy wypełniają się przykładowymi danymi przy pierwszym starcie
(11 gatunków, ~60 znalezisk w polskich lasach, kilka wątków na forum).

## Praca lokalna (Rider / VS Code)

1. Infrastruktura w Dockerze: `docker compose up -d postgres rabbitmq`
2. Serwisy .NET z Ridera (profile w `launchSettings.json`) albo z terminala:

   | Projekt | Port |
   |---|---|
   | `Atlas.Users.Api` | 5101 |
   | `Atlas.Forum.Api` | 5102 |
   | `Atlas.Map.Api` | 5103 |
   | `Atlas.Gateway` | 8080 |

   W trybie Development dokumentacja API jest pod `/scalar` każdego serwisu (np. http://localhost:5103/scalar).
3. Predykcja:
   ```bash
   cd src/Services/Prediction
   python -m venv .venv && source .venv/bin/activate
   pip install -r requirements-dev.txt
   uvicorn app.main:app --reload --port 5104
   ```
4. Frontend: `cd frontend && npm install && npm run dev` → http://localhost:5173 (proxy `/api` → bramka :8080)

Testy: `dotnet test AtlasGrzybow.slnx`, `python -m pytest` w `src/Services/Prediction`, `npm run build` we `frontend`.

### Migracje EF Core

Migracje stosują się same przy starcie serwisu. Nowa migracja (przykład dla Mapy):

```bash
dotnet tool restore
dotnet ef migrations add NazwaMigracji \
  --project src/Services/Map/Atlas.Map.Infrastructure \
  --startup-project src/Services/Map/Atlas.Map.Api \
  --output-dir Persistence/Migrations
```

## Publikacja przez Cloudflare Tunnel

1. W Cloudflare Zero Trust → *Networks → Tunnels* utwórz tunel i skopiuj token.
2. Dodaj public hostname `atlas.home-apps.dev` → `http://gateway:8080`.
3. Wpisz token do `.env` (`CLOUDFLARE_TUNNEL_TOKEN=...`), zmień `JWT_SIGNING_KEY` i hasła.
4. `docker compose --profile tunnel up -d`

## Podział pracy (4 osoby)

Każdy serwis jest niezależny (własna baza, własny Dockerfile, własne testy), więc każda osoba może rozwijać
swój serwis bez konfliktów: **Użytkownicy**, **Forum**, **Mapa**, **Predykcja**. Wspólne rzeczy
(`BuildingBlocks`, `Contracts`, `docker-compose.yml`, frontend) zmieniamy przez PR z review.

## Co dalej (kolejne etapy)

- Predykcja: dane historyczne z GBIF, mapa lasów, prawdziwy model zamiast heurystyki.
- Mapa: zdjęcia znalezisk, klastrowanie markerów, prywatne znaleziska.
- Forum: edycja i moderacja, powiadomienia, powiązanie wątku z punktem na mapie.
- Użytkownicy: refresh tokeny, profil, role moderatorów.
- Observability: logi i metryki (OpenTelemetry), health checki baz w `/health/ready`.
