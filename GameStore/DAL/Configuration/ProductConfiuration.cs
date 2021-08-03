using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Configuration
{
    public class ProductConfiuration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                 .HasQueryFilter(m => !m.isDeleted);
            builder
                 .HasIndex(o => new { o.Name, o.Platform, o.DateCreated, o.TotalRating });
        }
    }
}