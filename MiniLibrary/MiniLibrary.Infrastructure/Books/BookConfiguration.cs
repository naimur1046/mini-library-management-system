using MiniLibrary.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniLibrary.Infrastructure.Books;

internal sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(b => b.ISBN)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(b => b.ISBN)
            .IsUnique();

        builder.Property(b => b.Category)
            .HasMaxLength(100);

        builder.Property(b => b.PublishedYear)
            .IsRequired();

        builder.Property(b => b.CopiesAvailable)
            .IsRequired();

        builder.Property(b => b.IsAvailable)
            .IsRequired();
        
        builder.Property(b => b.CreatedOnUtc)
            .IsRequired();

        builder.Property(b => b.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.ModifiedBy)
            .HasMaxLength(100);
        
        builder.Property(b => b.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(b => b.DeletedBy)
            .HasMaxLength(100);
        
        builder.Ignore(b => b.DomainEvents);
    }
}
