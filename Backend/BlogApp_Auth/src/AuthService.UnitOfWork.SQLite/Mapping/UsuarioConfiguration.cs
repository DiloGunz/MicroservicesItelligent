using AuthService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.UnitOfWork.SQLite.Mapping;

public class UsuarioConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Username).HasMaxLength(50).IsRequired();
        builder.Property(x => x.PasswordHash).IsRequired();
        builder.Property(x => x.Role).HasMaxLength(50).IsRequired();

        builder.HasIndex(x => x.Username).IsUnique();
    }
}