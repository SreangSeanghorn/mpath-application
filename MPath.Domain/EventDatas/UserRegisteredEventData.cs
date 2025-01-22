namespace MPath.Domain.EventDatas

{
    public record UserRegisteredEventData(
        string UserName,
        string Email,
        Guid RoleId
    )
    {   
    }
}
