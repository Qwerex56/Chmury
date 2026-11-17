"""Pogoda z Open-Meteo (bez klucza API). Gdy API jest niedostępne, używamy klimatologii dla Polski."""

from __future__ import annotations

import logging
import time
from dataclasses import dataclass
from datetime import date

import httpx

log = logging.getLogger(__name__)

OPEN_METEO_URL = "https://api.open-meteo.com/v1/forecast"

# Średnia temperatura i suma opadów z 14 dni dla Polski, wg miesiąca (przybliżenie).
CLIMATE = {
    1: (-1.5, 15.0), 2: (-0.5, 14.0), 3: (3.0, 16.0), 4: (8.5, 18.0),
    5: (13.5, 28.0), 6: (17.0, 33.0), 7: (19.0, 38.0), 8: (18.5, 32.0),
    9: (14.0, 25.0), 10: (9.0, 20.0), 11: (4.0, 20.0), 12: (0.5, 18.0),
}


@dataclass(frozen=True)
class WeatherSummary:
    mean_temp_7d: float
    rain_sum_14d: float
    source: str  # "open-meteo" albo "klimatologia"


_cache: dict[tuple[float, float], tuple[float, WeatherSummary]] = {}
_CACHE_SECONDS = 3600


def climatology(month: int) -> WeatherSummary:
    temp, rain = CLIMATE[month]
    return WeatherSummary(temp, rain, "klimatologia")


async def get_weather(lat: float, lon: float, today: date | None = None) -> WeatherSummary:
    today = today or date.today()
    key = (round(lat, 1), round(lon, 1))
    cached = _cache.get(key)
    if cached and time.monotonic() - cached[0] < _CACHE_SECONDS:
        return cached[1]

    params = {
        "latitude": key[0],
        "longitude": key[1],
        "daily": "temperature_2m_mean,precipitation_sum",
        "past_days": 14,
        "forecast_days": 1,
        "timezone": "Europe/Warsaw",
    }
    try:
        async with httpx.AsyncClient(timeout=4.0) as client:
            response = await client.get(OPEN_METEO_URL, params=params)
            response.raise_for_status()
            daily = response.json()["daily"]
        temps = [t for t in daily["temperature_2m_mean"] if t is not None][-7:]
        rains = [r for r in daily["precipitation_sum"] if r is not None][-14:]
        summary = WeatherSummary(sum(temps) / len(temps), sum(rains), "open-meteo")
    except Exception as exc:  # noqa: BLE001 – każda awaria API = fallback
        log.warning("Open-Meteo niedostępne (%s), używam klimatologii", exc)
        summary = climatology(today.month)

    _cache[key] = (time.monotonic(), summary)
    return summary
