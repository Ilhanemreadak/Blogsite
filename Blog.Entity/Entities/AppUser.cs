using Blog.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Blog.Entity.Entities
{
    public class AppUser : IdentityUser<Guid>, IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ImageId { get; set; } = Guid.Parse("405653d0-c54b-4b42-89a4-f26ae06578d3");
        public Image Image { get; set; }
        public ICollection<Article> Articles { get; set; }

    }
}
