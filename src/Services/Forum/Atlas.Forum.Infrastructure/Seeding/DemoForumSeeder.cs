using Atlas.Forum.Domain;
using Atlas.Forum.Infrastructure.Persistence;
using Atlas.Kit.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Forum.Infrastructure.Seeding;

/// <summary>Kilka wątków, żeby forum nie było puste na prezentacji.</summary>
internal sealed class DemoForumSeeder(ForumDbContext db, TimeProvider clock) : IDataSeeder
{
    private static readonly Guid SystemAuthor = Guid.Empty;

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (await db.Threads.AnyAsync(cancellationToken))
        {
            return;
        }

        var now = clock.GetUtcNow();

        var welcome = ForumThread.Start(
            "Witamy w Atlasie grzybów!",
            ForumCategory.Dyskusje,
            "To forum dla grzybiarzy. Dzielcie się znaleziskami, pytajcie o oznaczanie gatunków i planujcie wspólne wyjścia do lasu.",
            SystemAuthor,
            "Atlas",
            now.AddDays(-6));
        welcome.AddPost("Super pomysł, w końcu jedno miejsce na mapę i rozmowy!", SystemAuthor, "Kania", now.AddDays(-5));

        var question = ForumThread.Start(
            "Jak odróżnić kanię od muchomora sromotnikowego?",
            ForumCategory.Pytania,
            "Znalazłem coś podobnego do kani, ale nie jestem pewien. Na co zwracać uwagę?",
            SystemAuthor,
            "Podgrzybek",
            now.AddDays(-3));
        question.AddPost(
            "Kania ma ruchomy pierścień i wężykowaty wzór na trzonie, a sromotnik pochwę u nasady trzonu. W razie wątpliwości: nie zbieraj!",
            SystemAuthor,
            "Borowik",
            now.AddDays(-2));

        var recipe = ForumThread.Start(
            "Sos kurkowy, sprawdzony przepis",
            ForumCategory.Przepisy,
            "Kurki, masło, cebula, śmietana 30%, natka. Kurki krótko na mocnym ogniu, potem reszta.",
            SystemAuthor,
            "Kurka",
            now.AddDays(-1));

        db.Threads.AddRange(welcome, question, recipe);
        await db.SaveChangesAsync(cancellationToken);
    }
}
