using MPath.SharedKernel.Primitive;

namespace MPath.Domain.Entities;

public class Recommendation : Entity<Guid>
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public bool IsCompleted { get; private set; }
    private Recommendation()
    {
    }

    private Recommendation(string title, string content, bool isCompleted)
    {
        Title = title;
        Content = content;
        IsCompleted = isCompleted;
    }

    public static Recommendation Create(string title, string content, bool isCompleted)
    {
        return new Recommendation(title, content, isCompleted);
    }
    public void MarkCompleted()
    {
        IsCompleted = true;
    }
}