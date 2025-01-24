using Microsoft.EntityFrameworkCore;
using MPath.Domain;
using MPath.Domain.Entities;
using MPath.Infrastructure.Configuration;
using MPath.SharedKernel.Event;
using MPath.SharedKernel.Primitive;

namespace MPath.Infrastructure.Persistence.DBContext;
    public class ApplicationDbContext : DbContext,IUnitOfWork
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
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
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new PatientConfiguration());
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
   
