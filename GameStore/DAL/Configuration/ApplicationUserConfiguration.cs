using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .HasMany(c => c.OwnedGames)
                .WithMany(s => s.Users)
                .UsingEntity<GamesLibrary>(
                j => j
                    .HasOne(pt => pt.Game)
                    .WithMany(t => t.GamesLibraries)
                    .HasForeignKey(pt => pt.GameId),  // связь с таблицей Students через StudentId
                j => j
                    .HasOne(pt => pt.AppUser)
                    .WithMany(p => p.GamesLibrary)
                    .HasForeignKey(pt => pt.UserId),   // связь с таблицей Courses через CourseId
                j =>
                {
                    j.HasKey(t => new { t.UserId, t.GameId });
                    j.ToTable("GameLibraries");
                });
        }
    }
}