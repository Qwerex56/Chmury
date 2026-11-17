"""Słownik gatunków z preferencjami pogodowymi. Identyfikatory zgodne z serwisem Mapa."""

from dataclasses import dataclass


@dataclass(frozen=True)
class Species:
    id: str
    name: str
    season_from: int
    season_to: int
    optimal_temp: float  # °C, średnia dobowa
    min_rain_mm: float  # suma opadów z 14 dni, od której gatunek "rusza"


SPECIES: tuple[Species, ...] = (
    Species("borowik-szlachetny", "Borowik szlachetny", 6, 10, 16.0, 25.0),
    Species("podgrzybek-brunatny", "Podgrzybek brunatny", 7, 11, 13.0, 20.0),
    Species("kania", "Czubajka kania", 7, 10, 17.0, 15.0),
    Species("kurka", "Pieprznik jadalny (kurka)", 6, 10, 18.0, 20.0),
    Species("maslak-zwyczajny", "Maślak zwyczajny", 6, 11, 14.0, 15.0),
    Species("kozlarz-babka", "Koźlarz babka", 6, 10, 16.0, 20.0),
    Species("rydz", "Mleczaj rydz", 8, 10, 13.0, 25.0),
    Species("opienka-miodowa", "Opieńka miodowa", 9, 11, 10.0, 20.0),
    Species("goryczak-zolciowy", "Goryczak żółciowy", 6, 10, 16.0, 20.0),
    Species("muchomor-czerwony", "Muchomor czerwony", 7, 11, 13.0, 15.0),
    Species("muchomor-sromotnikowy", "Muchomor sromotnikowy", 7, 10, 15.0, 20.0),
)
