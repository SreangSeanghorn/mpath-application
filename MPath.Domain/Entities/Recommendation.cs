using MPath.SharedKernel.Primitive;

namespace MPath.Domain.Entities;

public class Recommendation : Entity<Guid>
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public bool IsCompleted { get; private set; }
    public Guid PatientId { get; private set; }
    
    public Patient Patient { get; private set; }
    
    public void MarkCompleted()
    {
        IsCompleted = true;
    }
    
    private Recommendation()
    {
    }

    private Recommendation(Patient patient, string title, string content, bool isCompleted)
    {
        Patient = patient;
        PatientId = patient.Id;
        Title = title;
        Content = content;
        IsCompleted = isCompleted;
    }

    public static Recommendation Create(Patient patient, string title, string content, bool isCompleted)
    {
        return new Recommendation(patient, title, content, isCompleted);
    }
}