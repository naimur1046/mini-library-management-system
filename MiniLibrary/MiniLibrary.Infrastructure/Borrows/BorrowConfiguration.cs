using Domain.Borrows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniLibrary.Infrastructure.Borrows;

internal sealed class BorrowConfiguration : IEntityTypeConfiguration<Borrow>
{
    public void Configure(EntityTypeBuilder<Borrow> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.MemberId)
            .IsRequired();

        builder.HasOne(b => b.Member)
            .WithMany()
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(b => b.BorrowDate)
            .IsRequired();

        builder.Property(b => b.DueDate)
            .IsRequired();

        builder.HasMany(b => b.BorrowItems)
            .WithOne(bi => bi.Borrow)
            .HasForeignKey(bi => bi.BorrowId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(b => b.CreatedOnUtc)
            .IsRequired();

        builder.Property(b => b.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.ModifiedBy)
            .HasMaxLength(100);
        
        builder.Ignore(b => b.DomainEvents);
    }
}
