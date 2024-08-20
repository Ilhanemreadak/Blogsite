using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Articles;
using Blog.Service.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Blog.Service.Extensions;
using Microsoft.AspNetCore.Identity;
using Blog.Service.Helpers.Images;
using Blog.Entity.Enums;

namespace Blog.Service.Services.Concrete
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IImageHelper imageHelper;
        private readonly ClaimsPrincipal _user;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper) {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
            this.imageHelper = imageHelper;
        }

        public async Task<VMArticleList> GetAllByPagingAsync(Guid? categoryId, int currentPage=1, int pageSize = 6, bool isAscending = false)
        {

            pageSize = pageSize > 20 ? 20 : pageSize;

            var articles = categoryId == null
                ? await unitOfWork.GetRepository<Article>().GetAllAsync(a => !a.IsDeleted, a => a.Category, i => i.Image)
                : await unitOfWork.GetRepository<Article>().GetAllAsync(a => a.CategoryId == categoryId && !a.IsDeleted, x => x.Category, i => i.Image);

            var sortedArticles = isAscending
                ? articles.OrderBy(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new VMArticleList
            {
                Articles = sortedArticles,
                CategoryId = categoryId == null ? null : categoryId.Value,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            };

        }

        public async Task CreateArticleAsync(VMArticleAdd vmArticleAdd)
        {
            var userId = _user.GetLoggedInUserId();
            var useremail = _user.GetLoggedInEmail();

            var imageUpload = await imageHelper.Upload(vmArticleAdd.Title, vmArticleAdd.Photo, ImageType.Post);
            Image image = new(imageUpload.FullName, vmArticleAdd.Photo.ContentType, useremail);
            await unitOfWork.GetRepository<Image>().AddAsync(image);

            var article = new Article(vmArticleAdd.Title, vmArticleAdd.Content, userId, useremail, vmArticleAdd.CategoryId, image.Id);
          
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
            var article = await unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted & x.Id == articleId, x => x.Category, i => i.Image);
            var map = mapper.Map<VMArticle>(article);

            return map;
        }
        public async Task<string> UpdateArticleAsync(VMArticleUpdate vmArticleUpdate)
        {
            var useremail = _user.GetLoggedInEmail();
            var article = await unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted & x.Id == vmArticleUpdate.Id, x => x.Category, i => i.Image);

            if(vmArticleUpdate.Photo != null)
            {
                imageHelper.Delete(article.Image.FileName);

                var imageUpload = await imageHelper.Upload(vmArticleUpdate.Title, vmArticleUpdate.Photo, ImageType.Post);
                Image image = new(imageUpload.FullName, vmArticleUpdate.Photo.ContentType, useremail);
                await unitOfWork.GetRepository<Image>().AddAsync(image);

                article.ImageId = image.Id;
            }


            
            article.Title = vmArticleUpdate.Title;
            article.Content = vmArticleUpdate.Content;
            article.CategoryId = vmArticleUpdate.CategoryId;
            article.ModifiedDate = DateTime.Now;
            article.ModifiedBy = useremail;


            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

            return article.Title;
        }

        public async Task<string> SafeDeleteArticleAsync(Guid articleId)
        {   
            var userEmail = _user.GetLoggedInEmail();
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);

            article.IsDeleted = true;
            article.DeletedDate = DateTime.Now;
            article.DeletedBy = userEmail;

            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

            return article.Title;
        }

        public async Task<List<VMArticle>> GetAllArticlesWithCategoryDeletedAsync()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => x.IsDeleted, x => x.Category);
            var map = mapper.Map<List<VMArticle>>(articles);

            return map;
        }

        public async Task<string> UndoDeleteArticleAsync(Guid articleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);

            article.IsDeleted = false;
            article.DeletedDate = null;
            article.DeletedBy = null;

            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

            return article.Title;
        }

    }
}
