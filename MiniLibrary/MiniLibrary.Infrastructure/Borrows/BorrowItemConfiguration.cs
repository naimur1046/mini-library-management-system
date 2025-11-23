using Domain.Borrows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniLibrary.Infrastructure.Borrows;

internal sealed class BorrowItemConfiguration : IEntityTypeConfiguration<BorrowItem>
{
    public void Configure(EntityTypeBuilder<BorrowItem> builder)
    {
        builder.HasKey(bi => bi.Id);

        builder.Property(bi => bi.BorrowId)
            .IsRequired();

        builder.Property(bi => bi.BookId)
            .IsRequired();

        builder.HasOne(bi => bi.Book)
            .WithMany()
            .HasForeignKey(bi => bi.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(bi => bi.ReturnDate);
        
        builder.Property(bi => bi.CreatedOnUtc)
            .IsRequired();

        builder.Property(bi => bi.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(bi => bi.ModifiedBy)
            .HasMaxLength(100);
        
        builder.Ignore(bi => bi.DomainEvents);
    }
}
