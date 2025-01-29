using MPath.Domain.Core.Repositories;
using MPath.Domain.Entities;

namespace MPath.Domain.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    public Task<User?> GetByEmail(string email);
    public Task<User?> GetUserWithPatient(Guid userId);
}