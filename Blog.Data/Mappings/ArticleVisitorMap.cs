using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class ArticleVisitorMap : IEntityTypeConfiguration<ArticleVisitor>
    {
        public void Configure(EntityTypeBuilder<ArticleVisitor> builder)
        {
            builder.HasKey(av => new { av.ArticleId, av.VisitorId });

            builder.HasOne(av => av.Article)
                .WithMany(a => a.ArticleVisitors)
                .HasForeignKey(av => av.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(av => av.Visitor)
                .WithMany(v => v.ArticleVisitors)
                .HasForeignKey(av => av.VisitorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
