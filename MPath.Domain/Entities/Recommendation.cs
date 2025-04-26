using MPath.SharedKernel.Primitive;

namespace MPath.Domain.Entities;

public class Recommendation : Entity<Guid>
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public bool IsCompleted { get; private set; }
    
    public Guid CreatedByUserId { get; private set; }
    private Recommendation()
    {
    }

    private Recommendation(string title, string content, bool isCompleted, Guid createdByUserId)
    {
        Title = title;
        Content = content;
        IsCompleted = isCompleted;
        CreatedByUserId = createdByUserId;
    }

    public static Recommendation Create(string title, string content, bool isCompleted, Guid createdByUserId)
    {
        return new Recommendation(title, content, isCompleted, createdByUserId);
    }
    public void MarkCompleted()
    {
        IsCompleted = true;
    }
}