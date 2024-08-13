using Blog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entity.Entities
{
    public class SocialMedia : IEntityBase
    {
        public SocialMedia() { }
        public SocialMedia(int SocialMediaType, string Link)
        {
            this.SocialMediaType = SocialMediaType;
            this.Link = Link;
        }
        public int Id { get; set; }
        public int SocialMediaType { get; set; }
        public string Link { get; set; }

    }
}
