using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Configuration
{
    public class UserGameConfiguration : IEntityTypeConfiguration<GamesLibrary>
    {
        public void Configure(EntityTypeBuilder<GamesLibrary> builder)
        {
            builder
                 .HasOne(x => x.Game)
                 .WithMany(x => x.GamesLibraries)
                 .HasForeignKey(x => x.GameId)
                 .HasPrincipalKey(x => x.Id)
                 .IsRequired();
            builder
                .HasOne(x => x.AppUser)
                .WithMany(x => x.GamesLibrary)
                .HasForeignKey(x => x.UserId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired();

            builder.HasKey(k => new { k.UserId, k.GameId });
        }
    }
}