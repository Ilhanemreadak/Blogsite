using Blog.Core.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace Blog.Entity.Entities
{
    public class Educational : EntityBase
    {
        public Educational()
        {
            
        }

        public Educational(string title, string content, Guid userId, string createdBy, Guid categoryId, Guid imageId)
        {
            Title = title;
            Content = content;
            UserId = userId;
            CategoryId = categoryId;
            ImageId = imageId;
            CreatedBy = createdBy;
        }

        public Educational(string title, string content, Guid userId, AppUser user, string createdBy, Guid categoryId, Guid imageId)
        {
            Title = title;
            Content = content;
            UserId = userId;
            CategoryId = categoryId;
            ImageId = imageId;
            CreatedBy = createdBy;
            User = user;
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; } = 0;

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid? ImageId { get; set; }
        public Image Image { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<EducationalVisitor> EducationalVisitors { get; set; }
    }
}
