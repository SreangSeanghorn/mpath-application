namespace MPath.Domain.EventDatas;

public record CreatedPatientEventData
(string Name, string Email, string PhoneNumber, string Address, DateTime BirthDate);