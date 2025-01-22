using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPath.Domain.Entities;

namespace MPath.Infrastructure.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).IsRequired();
            
            builder.HasData(
                new { Id = Guid.NewGuid(), Name = "Admin", Description = "Admin can do anything" },
                new { Id = Guid.NewGuid(), Name = "User", Description = "User can do tasks based on their specific permission" },
                new { Id = Guid.NewGuid(), Name = "Default", Description = "Default Role" }

            );
        }
    }
}