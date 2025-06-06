using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Configuration
{
    public class AppUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder
                .HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired();
            builder
                .HasOne(x => x.AppUser)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId)
                .HasPrincipalKey(x => x.Id)
                .IsRequired();
        }
    }
}