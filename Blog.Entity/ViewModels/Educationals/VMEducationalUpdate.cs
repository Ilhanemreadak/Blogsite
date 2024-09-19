using Blog.Entity.ViewModels.Categories;
using Microsoft.AspNetCore.Http;
using Blog.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entity.ViewModels.Educationals
{
    public class VMEducationalUpdate
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid CategoryId { get; set; }
        public Image Image { get; set; }
        public IFormFile? Photo { get; set; }
        public IList<VMCategory> Categories { get; set; }
    }
}
