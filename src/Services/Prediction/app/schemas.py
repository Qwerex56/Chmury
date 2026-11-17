from pydantic import BaseModel


class WeatherView(BaseModel):
    mean_temp_7d: float
    rain_sum_14d: float
    source: str


class SpeciesChance(BaseModel):
    species_id: str
    name: str
    probability: float


class ForecastView(BaseModel):
    latitude: float
    longitude: float
    score: float
    weather: WeatherView
    species: list[SpeciesChance]


class HeatCell(BaseModel):
    south: float
    west: float
    north: float
    east: float
    score: float


class HeatmapView(BaseModel):
    weather: WeatherView
    cells: list[HeatCell]
