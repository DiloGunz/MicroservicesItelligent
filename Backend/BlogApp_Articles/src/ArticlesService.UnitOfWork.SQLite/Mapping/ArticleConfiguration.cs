using ArticlesService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticlesService.UnitOfWork.SQLite.Mapping;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
        builder.Property(x=>x.Summary).HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.Content).HasMaxLength(200).IsRequired();

        builder.HasMany(x => x.Comments)
            .WithOne(x => x.Article)
            .HasForeignKey(x => x.ArticleId);

        builder.HasIndex(x => x.Title);
    }
}