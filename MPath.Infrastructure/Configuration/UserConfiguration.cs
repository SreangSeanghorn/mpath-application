using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPath.Domain.Entities;

namespace MPath.Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnName("id");
            builder.Property(e => e.UserName).HasColumnName("username").IsRequired();
            builder.Property(e => e.Password).IsRequired().HasColumnName("password");
            builder.OwnsOne(e => e.Email, email => { email.Property(e => e.Value).IsRequired().HasColumnName("email"); });
            builder.HasMany(e => e.Roles)
                      .WithMany(e => e.Users)
                        .UsingEntity<Dictionary<string, object>>(
                          "UserRole",
                          j => j.HasOne<Role>()
                                .WithMany().HasForeignKey("roleId"),
                          j => j.HasOne<User>()
                                .WithMany().HasForeignKey("userId"), j => {
                              j.HasKey("userId", "roleId"); });
        }
    }
}