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
        Task<List<VMArticle>> GetAllArticlesWithCategoryDeletedAsync();
        Task CreateArticleAsync(VMArticleAdd vmArticleAdd);
        Task<VMArticle> GetArticlesWithCategoryNonDeletedAsync(Guid articleId);
        Task<string> UpdateArticleAsync(VMArticleUpdate vmArticleUpdate);
        Task<string> SafeDeleteArticleAsync(Guid articleId);
        Task<string> UndoDeleteArticleAsync(Guid articleId);
    }
}
