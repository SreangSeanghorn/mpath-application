using Microsoft.EntityFrameworkCore;
using MPath.Domain.Core.Repositories;
using MPath.Infrastructure.Persistence.DBContext;

namespace MPath.Infrastructure.Repositories;

 public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext dbContext;

        public GenericRepository(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            dbContext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
