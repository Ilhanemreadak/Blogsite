using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasData(new Article
            {
                Id = Guid.Parse("3E418302-6617-44B1-AAFB-C080A20CEDAC"),
                Title = "Mubitek Deneme 1",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec placerat elit eu ullamcorper condimentum. Proin non suscipit massa. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aliquam vel massa velit. Fusce nec elementum odio. Quisque mattis ipsum ornare arcu commodo, ac tristique enim cursus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Sed quis aliquet velit. Etiam mollis massa sed sagittis dignissim. Vivamus ultrices malesuada scelerisque. Nam ac vulputate neque. Nullam pharetra, ante id aliquet efficitur, arcu elit ullamcorper velit, vitae viverra ante nisi id enim. Nullam ante justo, dictum vitae.",
                ViewCount = 15,
                CategoryId = Guid.Parse("63AFD642-3FEE-4C23-BB9D-8D37EE7E7310"),
                ImageId = Guid.Parse("4675F9E0-085B-4A27-8D4A-C97ACE6A7971"),
                CreatedBy = "admin",
                CreatedDate = new DateTime(2024, 7, 29),
                IsDeleted = false,
                UserId = Guid.Parse("C118D34E-5077-4F02-8C68-6BFBC61A4DF0")
            },
            new Article
            {
                Id = Guid.Parse("FA729AF6-A354-44B5-B7A2-DF6458B6AF92"),
                Title = "Mubitek-Evok Deneme 1",
                Content = "VisualStudio ile lorem aaaLorem ipsum dolor sit amet, consectetur adipiscing elit. Donec placerat elit eu ullamcorper condimentum. Proin non suscipit massa. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aliquam vel massa velit. Fusce nec elementum odio. Quisque mattis ipsum ornare arcu commodo, ac tristique enim cursus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Sed quis aliquet velit. Etiam mollis massa sed sagittis dignissim. Vivamus ultrices malesuada scelerisque. Nam ac vulputate neque. Nullam pharetra, ante id aliquet efficitur, arcu elit ullamcorper velit, vitae viverra ante nisi id enim. Nullam ante justo, dictum vitae.",
                ViewCount = 26,
                CategoryId = Guid.Parse("7FA8D647-9BBD-4D2F-BC80-2B5E6C16A052"),
                ImageId = Guid.Parse("D20EEB5B-2979-4068-A91F-42EBF3B9B03E"),
                CreatedBy = "admin",
                CreatedDate = new DateTime(2024, 7, 29),
                IsDeleted = false,
                UserId = Guid.Parse("20978C52-0503-4800-A4B5-68927E3D1EE0")
            }
            
            ); 
        }
    }
}
