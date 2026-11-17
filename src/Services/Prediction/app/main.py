"""Serwis Predykcja: gdzie i jakich grzybów można się spodziewać."""

from datetime import date, datetime, timezone

from fastapi import FastAPI, Query

from . import model
from .schemas import ForecastView, HeatCell, HeatmapView, SpeciesChance, WeatherView
from .weather import get_weather

app = FastAPI(title="Atlas grzybów – Predykcja", version="0.1.0")


@app.get("/health")
def health() -> dict:
    return {"service": "prediction", "status": "ok", "time": datetime.now(timezone.utc).isoformat()}


@app.get("/forecast", response_model=ForecastView)
async def forecast(
    lat: float = Query(ge=-90, le=90),
    lon: float = Query(ge=-180, le=180),
) -> ForecastView:
    month = date.today().month
    weather = await get_weather(lat, lon)
    scores = model.species_scores(lat, lon, month, weather)
    return ForecastView(
        latitude=lat,
        longitude=lon,
        score=round(model.overall_score(lat, lon, month, weather), 3),
        weather=WeatherView(**weather.__dict__),
        species=[SpeciesChance(species_id=s.id, name=s.name, probability=round(p, 3)) for s, p in scores],
    )


@app.get("/heatmap", response_model=HeatmapView)
async def heatmap(
    south: float = Query(ge=-90, le=90),
    west: float = Query(ge=-180, le=180),
    north: float = Query(ge=-90, le=90),
    east: float = Query(ge=-180, le=180),
    size: int = Query(default=16, ge=4, le=40),
) -> HeatmapView:
    month = date.today().month
    weather = await get_weather((south + north) / 2, (west + east) / 2)
    d_lat = (north - south) / size
    d_lon = (east - west) / size
    cells = []
    for row in range(size):
        for col in range(size):
            s, w = south + row * d_lat, west + col * d_lon
            score = model.overall_score(s + d_lat / 2, w + d_lon / 2, month, weather)
            cells.append(HeatCell(south=s, west=w, north=s + d_lat, east=w + d_lon, score=round(score, 3)))
    return HeatmapView(weather=WeatherView(**weather.__dict__), cells=cells)
