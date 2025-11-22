using Domain.EmailLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniLibrary.Infrastructure.EmailLogs;

internal sealed class EmailLogConfiguration : IEntityTypeConfiguration<EmailLog>
{
    public void Configure(EntityTypeBuilder<EmailLog> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.RecipientEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Subject)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.Body)
            .IsRequired();

        builder.Property(e => e.SentDate)
            .IsRequired();

        builder.Property(e => e.IsSuccess)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.ErrorMessage)
            .HasMaxLength(1000);

        builder.Property(e => e.EmailType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.RelatedEntityId);

        builder.HasIndex(e => e.RecipientEmail);
        builder.HasIndex(e => e.SentDate);
        builder.HasIndex(e => e.EmailType);

        // Audit fields
        builder.Property(e => e.CreatedOnUtc)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        // Ignore domain events
        builder.Ignore(e => e.DomainEvents);
    }
}
