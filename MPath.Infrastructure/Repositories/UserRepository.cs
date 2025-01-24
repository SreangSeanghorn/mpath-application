
using Microsoft.EntityFrameworkCore;
using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Infrastructure.Persistence.DBContext;

namespace MPath.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetByEmail(string email)
        {
            return await dbContext.Users.Include(u=>u.Roles).FirstOrDefaultAsync(x => x.Email.Value == email);
        }
    }
}