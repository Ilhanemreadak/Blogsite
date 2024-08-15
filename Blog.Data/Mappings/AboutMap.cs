using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class AboutMap : IEntityTypeConfiguration<About>
    {
        public void Configure(EntityTypeBuilder<About> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(c => c.Title);
            builder.Property(c => c.Description);

            builder.HasData(new About
            {
                Id = 1,
                Title = "Ben Gül çiçek zengin",
                Description = "Yenilikçi bir Endüstriyel Tasarımcı olarak, tasarım düşüncesini kullanarak yaratıcı çözümler üretiyorum. Teknoloji tutkumu tasarımlarıma yansıtırken, boş zamanlarımda amatör ressamlık yaparak sanatsal yönümü geliştiriyorum."
            });

        }
    }
}
