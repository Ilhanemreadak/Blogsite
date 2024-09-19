using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Categories;

namespace Blog.Entity.ViewModels.Educationals
{
    public class VMEducational
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public VMCategory Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public Image Image { get; set; }
        public AppUser User { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int ViewCount { get; set; }
    }
}
