"""
Heurystyczny model predykcji (etap demo). Wynik 0..1 = sezon × wilgotność × temperatura × siedlisko.
Siedlisko to na razie deterministyczny szum przestrzenny; docelowo dane GBIF i mapa lasów.
"""

from __future__ import annotations

import math

from .species import SPECIES, Species
from .weather import WeatherSummary


def season_factor(species: Species, month: int) -> float:
    if species.season_from <= month <= species.season_to:
        return 1.0
    if month in (species.season_from - 1, species.season_to + 1):
        return 0.3
    return 0.0


def moisture_factor(rain_sum_14d: float, min_rain_mm: float) -> float:
    return max(0.0, min(1.0, rain_sum_14d / (min_rain_mm * 1.6)))


def temperature_factor(mean_temp: float, optimal: float) -> float:
    return math.exp(-((mean_temp - optimal) ** 2) / (2 * 6.0**2))


def habitat_factor(lat: float, lon: float) -> float:
    """Gładki szum 0.15..1 udający zróżnicowanie siedlisk (las / pole / miasto)."""
    value = (
        math.sin(lat * 7.3 + lon * 3.1)
        + math.sin(lat * 2.1 - lon * 5.7 + 1.3)
        + 0.5 * math.sin(lat * 17.0 + lon * 13.0)
    )
    return 0.15 + 0.85 * (value + 2.5) / 5.0


def species_scores(lat: float, lon: float, month: int, weather: WeatherSummary) -> list[tuple[Species, float]]:
    habitat = habitat_factor(lat, lon)
    scores = [
        (
            s,
            season_factor(s, month)
            * moisture_factor(weather.rain_sum_14d, s.min_rain_mm)
            * temperature_factor(weather.mean_temp_7d, s.optimal_temp)
            * habitat,
        )
        for s in SPECIES
    ]
    return sorted(scores, key=lambda item: item[1], reverse=True)


def overall_score(lat: float, lon: float, month: int, weather: WeatherSummary) -> float:
    """Szansa na jakikolwiek grzyb: średnia z 3 najlepszych gatunków."""
    top = [score for _, score in species_scores(lat, lon, month, weather)[:3]]
    return sum(top) / len(top)
