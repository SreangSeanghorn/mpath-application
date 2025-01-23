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
            builder.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnName("id");
            builder.Property(e => e.Name).IsRequired().HasColumnName("name");
            builder.Property(e => e.Description).IsRequired().HasColumnName("description");
            
            builder.HasData(
                new { Id = Guid.Parse("d3b07020-4b99-4b8b-a520-6634a482ef55"), Name = "Admin", Description = "Admin can do anything" },
                new { Id = Guid.Parse("ac7d77e3-72e8-4e89-b034-14d8bc5d0dd3"), Name = "User", Description = "User can do tasks based on their specific permission" },
                new { Id = Guid.Parse("7a75d6b2-fbb3-4af6-9c7d-48131c6d5c2e"), Name = "Default", Description = "Default Role" }

            );
        }
    }
}