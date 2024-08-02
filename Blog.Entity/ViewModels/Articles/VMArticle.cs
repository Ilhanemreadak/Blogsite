﻿using Blog.Entity.ViewModels.Categories;

namespace Blog.Entity.ViewModels.Articles
{
    public class VMArticle
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public VMCategory Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
