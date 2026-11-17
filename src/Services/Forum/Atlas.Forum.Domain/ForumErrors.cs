using Atlas.Kit.Domain;

namespace Atlas.Forum.Domain;

public static class ForumErrors
{
    public static readonly AppError ThreadNotFound = AppError.NotFound("forum.thread_not_found", "Nie znaleziono wątku.");

    public static readonly AppError EmptyPost = AppError.Validation("forum.empty_post", "Treść posta nie może być pusta.");
}
