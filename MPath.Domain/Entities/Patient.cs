using MPath.Domain.ValueObjects;
using MPath.SharedKernel.Primitive;

namespace MPath.Domain.Entities;

public class Patient : Entity<Guid>
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Email Email { get; set; }
    public string PhoneNumber { get; private set; }
    public string Address { get; private set; }
    public DateTime BirthDate { get; private set; }

    public ICollection<Recommendation> Recommendations { get; private set; } = new List<Recommendation>();
    
    private Patient()
    {
    }
    private Patient(string name, Email email, string phoneNumber, string address, DateTime birthDate)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        BirthDate = birthDate;
    }
    public static Patient Create(string name, Email email, string phoneNumber, string address, DateTime birthDate)
    {
        return new Patient(name, email, phoneNumber, address, birthDate);
    }
    
    public void AddRecommendation(string title, string content, bool isCompleted)
    {
        Recommendations.Add(Recommendation.Create(this, title, content, isCompleted));
    }
    
}