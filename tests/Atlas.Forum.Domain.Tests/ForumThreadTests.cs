using Atlas.Forum.Domain;

namespace Atlas.Forum.Domain.Tests;

public sealed class ForumThreadTests
{
    private static readonly DateTimeOffset Start = new(2026, 9, 1, 10, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Start_creates_first_post_with_thread_content()
    {
        var thread = ForumThread.Start("Kurki", ForumCategory.Dyskusje, "Są już kurki?", Guid.NewGuid(), "Kurka", Start);

        var post = Assert.Single(thread.Posts);
        Assert.Equal("Są już kurki?", post.Content);
        Assert.Equal(thread.Id, post.ThreadId);
    }

    [Fact]
    public void AddPost_moves_last_activity()
    {
        var thread = ForumThread.Start("Kurki", ForumCategory.Dyskusje, "Są już kurki?", Guid.NewGuid(), "Kurka", Start);

        var result = thread.AddPost("Tak, w Kampinosie!", Guid.NewGuid(), "Borowik", Start.AddHours(2));

        Assert.True(result.IsSuccess);
        Assert.Equal(2, thread.Posts.Count);
        Assert.Equal(Start.AddHours(2), thread.LastActivityAt);
    }

    [Fact]
    public void AddPost_rejects_empty_content()
    {
        var thread = ForumThread.Start("Kurki", ForumCategory.Dyskusje, "Są już kurki?", Guid.NewGuid(), "Kurka", Start);

        var result = thread.AddPost("   ", Guid.NewGuid(), "Borowik", Start);

        Assert.True(result.IsFailed);
        Assert.Single(thread.Posts);
    }
}
