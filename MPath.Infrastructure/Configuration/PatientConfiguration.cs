using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPath.Domain.Entities;

namespace MPath.Infrastructure.Configuration;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd().HasColumnName("id");

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired()
            .HasColumnName("name");

        builder.OwnsOne(p => p.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("email")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(20)
            .HasColumnName("phone_number");

        builder.Property(p => p.Address)
            .HasMaxLength(200)
            .HasColumnName("address");

        builder.Property(p => p.BirthDate)
            .IsRequired()
            .HasColumnName("dob");
        builder.HasMany(p => p.Recommendations)
            .WithOne()
            .HasForeignKey("patient_id");
    }
}