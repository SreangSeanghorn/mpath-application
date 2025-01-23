using MPath.Domain.ValueObjects;
using MPath.SharedKernel.Primitive;

namespace MPath.Domain.Entities;

public class Patient : Entity<Guid>
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Address { get; private set; }
    public DateTime BirthDate { get; private set; }
    
}