from app import model
from app.species import SPECIES
from app.weather import WeatherSummary, climatology

WET_AUTUMN = WeatherSummary(mean_temp_7d=13.0, rain_sum_14d=45.0, source="test")


def test_out_of_season_species_score_zero():
    borowik = next(s for s in SPECIES if s.id == "borowik-szlachetny")
    assert model.season_factor(borowik, 2) == 0.0


def test_wet_september_beats_dry_january():
    wet = model.overall_score(52.3, 20.5, 9, WET_AUTUMN)
    dry = model.overall_score(52.3, 20.5, 1, climatology(1))
    assert wet > dry


def test_scores_are_between_zero_and_one():
    for _, score in model.species_scores(50.0, 20.0, 8, WET_AUTUMN):
        assert 0.0 <= score <= 1.0
