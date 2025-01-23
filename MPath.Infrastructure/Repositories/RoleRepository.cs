using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Infrastructure.Persistence.DBContext;

namespace MPath.Infrastructure.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}