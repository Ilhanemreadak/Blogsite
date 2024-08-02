using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Abstractions
{
    public interface IArticleService
    {
        Task<List<VMArticle>> GetAllArticlesWithCategoryNonDeletedAsync();
        Task CreateArticleAsync(VMArticleAdd vmArticleAdd);
        Task<VMArticle> GetArticlesWithCategoryNonDeletedAsync(Guid articleId);
        Task UpdateArticleAsync(VMArticleUpdate vmArticleUpdate);
        Task SafeDeleteArticleAsync(Guid articleId);
    }
}
