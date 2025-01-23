using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPath.Domain.Entities;

namespace MPath.Infrastructure.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnName("id");
        builder.Property(e => e.Token).IsRequired().HasColumnName("token");
        builder.Property(e => e.ExpiryDate).IsRequired().HasColumnName("expiry_date");
        builder.Property(e => e.IsRevoked).IsRequired().HasColumnName("is_revoked");
    }
}