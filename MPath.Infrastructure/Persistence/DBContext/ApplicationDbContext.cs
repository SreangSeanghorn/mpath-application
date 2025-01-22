using Microsoft.EntityFrameworkCore;
using MPath.Domain;
using MPath.Domain.Core.Event;
using MPath.Domain.Core.Primitive;
using MPath.Domain.Entities;
using MPath.Infrastructure.Configuration;

namespace MPath.Infrastructure.Persistence.DBContext;
    public class ApplicationDbContext : DbContext,IUnitOfWork
    {
        public DbSet<User> Users { get; set; }
        private readonly IEventPublisher _publisher;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IEventPublisher publisher) : base(options)
        {
            _publisher = publisher;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var domainEvents = ChangeTracker
            .Entries<AggregateRoot<Guid>>()
            .Select(e => e.Entity)
            .Where(e => e.GetDomainEvents().Any())
            .SelectMany(e =>
            {
                var domainEvents = e.GetDomainEvents();

                return domainEvents;
            })
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);

        }

        return result;
    }

    
}
   
