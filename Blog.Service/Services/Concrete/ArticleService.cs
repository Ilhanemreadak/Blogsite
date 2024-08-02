using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Articles;
using Blog.Service.Services.Abstractions;
using AutoMapper;

namespace Blog.Service.Services.Concrete
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper) {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task CreateArticleAsync(VMArticleAdd vmArticleAdd)
        {
            var userId = Guid.Parse("C118D34E-5077-4F02-8C68-6BFBC61A4DF0");

            var article = new Article
            {
                Title = vmArticleAdd.Title,
                Content = vmArticleAdd.Content,
                CategoryId = vmArticleAdd.CategoryId,
                UserId = userId
            };

            await unitOfWork.GetRepository<Article>().AddAsync(article);
            await unitOfWork.SaveAsync();

        }

        public async Task<List<VMArticle>> GetAllArticlesWithCategoryNonDeletedAsync()
        {

            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.IsDeleted, x => x.Category);
            var map = mapper.Map<List<VMArticle>>(articles);

            return map;
        }
        public async Task<VMArticle> GetArticlesWithCategoryNonDeletedAsync(Guid articleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted & x.Id == articleId, x => x.Category);
            var map = mapper.Map<VMArticle>(article);

            return map;
        }
        public async Task UpdateArticleAsync(VMArticleUpdate vmArticleUpdate)
        {
            var article = await unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted & x.Id == vmArticleUpdate.Id, x => x.Category);
            
            article.Title = vmArticleUpdate.Title;
            article.Content = vmArticleUpdate.Content;
            article.CategoryId = vmArticleUpdate.CategoryId;

            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
        }

        public async Task SafeDeleteArticleAsync(Guid articleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);
            article.IsDeleted = true;
            article.DeletedDate = DateTime.Now;

            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

        }


    }
}
