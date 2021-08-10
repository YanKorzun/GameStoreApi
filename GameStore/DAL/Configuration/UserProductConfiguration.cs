using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Configuration
{
    public class UserProductConfiguration : IEntityTypeConfiguration<ProductLibraries>
    {
        public void Configure(EntityTypeBuilder<ProductLibraries> builder)
        {
            builder
                .HasOne(x => x.Game)
                .WithMany(x => x.ProductLibraries)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.GameId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired();
            builder
                .HasOne(x => x.AppUser)
                .WithMany(x => x.ProductLibraries)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(x => x.UserId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired();

            builder.HasKey(k => new { k.UserId, k.GameId });
            builder.HasQueryFilter(o => !(o.Game.IsDeleted && o.AppUser.IsDeleted));
        }
    }
}