using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class SocialMediaMap : IEntityTypeConfiguration<SocialMedia>
    {
        public void Configure(EntityTypeBuilder<SocialMedia> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(c => c.SocialMediaType);
            builder.Property(c => c.Link);

            builder.HasData(new SocialMedia
            {
                Id = 1,
                SocialMediaType = 1,
                Link = "asdfasdf"
            },
            new SocialMedia
            {
                Id = 2,
                SocialMediaType = 2,
                Link = "asadsfasdf"
            },
            new SocialMedia
            {
                Id = 3,
                SocialMediaType = 3,
                Link = "asadsfasdfawdgasd"
            },
            new SocialMedia
            {
                Id = 4,
                SocialMediaType = 4,
                Link = "asadsfaasdfgasdf"
            });

        }
    }
}
