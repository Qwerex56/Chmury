namespace Atlas.Map.Application.Abstractions;

/// <summary>Filtr znalezisk: prostokąt mapy (opcjonalny), gatunek i liczba dni wstecz.</summary>
public sealed record FindingFilter(BoundingBox? Area, string? SpeciesId, int? LastDays);
