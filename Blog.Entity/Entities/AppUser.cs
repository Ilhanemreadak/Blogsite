using Blog.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Blog.Entity.Entities
{
    public class AppUser : IdentityUser<Guid>, IEntityBase
    {
        public AppUser() {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ImageId { get; set; } 
        public Image Image { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<Educational> Educationals { get; set; }
        public ICollection<InPress> InPresses { get; set; }

    }
}
