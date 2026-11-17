namespace Atlas.Kit.Domain;

/// <summary>Rodzaj błędu domenowego, mapowany na kod HTTP w warstwie Api.</summary>
public enum ErrorKind
{
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
}
