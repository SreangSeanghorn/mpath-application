using MPath.SharedKernel.Primitive;

namespace MPath.Domain.Entities;

public class Recommendation : Entity<Guid>
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public bool IsCompleted { get; private set; }
    public Guid PatientId { get; private set; }
    
    public void MarkCompleted()
    {
        IsCompleted = true;
    }
    

}