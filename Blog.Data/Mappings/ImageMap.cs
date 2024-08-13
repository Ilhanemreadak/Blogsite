using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class ImageMap : IEntityTypeConfiguration<Image>
    {
        void IEntityTypeConfiguration<Image>.Configure(EntityTypeBuilder<Image> builder)
        {
            
        }
    }
}
