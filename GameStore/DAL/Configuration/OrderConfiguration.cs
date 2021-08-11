using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .HasQueryFilter(m => !m.IsDeleted);
            builder
                .HasOne(o => o.ApplicationUser)
                .WithMany(o => o.Orders);
            builder.HasIndex(o => new { UserId = o.ApplicationUserId, o.ProductId });
        }
    }
}