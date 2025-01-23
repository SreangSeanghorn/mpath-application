
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

        public Task<User> GetByEmail(string email)
        {
            return dbContext.Users.FirstOrDefaultAsync(x => x.Email.Value == email);
        }
    }
}