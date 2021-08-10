using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Configuration
{
    public class ProductRatingConfiguration : IEntityTypeConfiguration<ProductRating>
    {
        public void Configure(EntityTypeBuilder<ProductRating> builder)
        {
            builder
                .HasOne(o => o.Product)
                .WithMany(o => o.Ratings);
            builder.HasIndex(o => new { o.UserId, o.ProductId });
        }
    }
}