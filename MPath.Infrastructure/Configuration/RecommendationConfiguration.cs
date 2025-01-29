using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPath.Domain.Entities;

namespace MPath.Infrastructure.Configuration;

public class RecommendationConfiguration : IEntityTypeConfiguration<Recommendation>
{
    public void Configure(EntityTypeBuilder<Recommendation> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd().HasColumnName("id");
        builder.Property(r => r.Title).HasMaxLength(100).IsRequired().HasColumnName("title");
        builder.Property(r => r.Content).HasMaxLength(500).IsRequired().HasColumnName("content");
        builder.Property(r => r.IsCompleted).IsRequired().HasColumnName("is_completed"); 
        builder.Property(r => r.CreatedByUserId).IsRequired().HasColumnName("created_by_user_id");
    }
}