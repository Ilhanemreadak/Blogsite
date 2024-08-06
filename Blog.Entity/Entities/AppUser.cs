using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entity.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ImageId { get; set; } = Guid.Parse("30b0c9f9-face-4dc5-b03b-d4c8a2ccea5d");
        public Image Image { get; set; }

        public ICollection<Article> Articles { get; set; }

    }
}
