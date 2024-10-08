﻿using Blog.Entity.ViewModels.Categories;
using Microsoft.AspNetCore.Http;


namespace Blog.Entity.ViewModels.InPresses
{
    public class VMInPressAdd
    {
        public string Title{ get; set; }
        public string Content { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile Photo { get; set; }
        public IList<VMCategory> Categories { get; set; }
    }
}
