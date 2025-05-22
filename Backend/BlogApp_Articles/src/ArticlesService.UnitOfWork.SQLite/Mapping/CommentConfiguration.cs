using ArticlesService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticlesService.UnitOfWork.SQLite.Mapping;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(x=>x.Id);
        builder.Property(x => x.Content).HasMaxLength(200).IsRequired();

    }
}