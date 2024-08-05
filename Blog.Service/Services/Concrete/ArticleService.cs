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
            this.imageHelper = imageHelper;
            _user = httpContextAccessor.HttpContext.User;
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
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);
            article.IsDeleted = true;
            article.DeletedDate = DateTime.Now;
            article.DeletedBy = _user.GetLoggedInEmail();

            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();

            return article.Title;
        }


    }
}
