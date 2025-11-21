using Domain.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniLibrary.Infrastructure.Members;

internal sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(m => m.Email)
            .IsUnique();

        builder.Property(m => m.Phone)
            .HasMaxLength(20);

        builder.Property(m => m.JoinDate)
            .IsRequired();

        builder.Property(m => m.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Audit fields
        builder.Property(m => m.CreatedOnUtc)
            .IsRequired();

        builder.Property(m => m.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.ModifiedBy)
            .HasMaxLength(100);

        // Soft delete fields
        builder.Property(m => m.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(m => m.DeletedBy)
            .HasMaxLength(100);

        // Ignore domain events
        builder.Ignore(m => m.DomainEvents);
    }
}
