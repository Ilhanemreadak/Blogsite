﻿using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Articles;
using Blog.Service.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Blog.Service.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Blog.Service.Services.Concrete
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
        }

        public async Task CreateArticleAsync(VMArticleAdd vmArticleAdd)
        {
            var userId = _user.GetLoggedInUserId();
            var useremail = _user.GetLoggedInEmail();

            var imageId = Guid.Parse("d20eeb5b-2979-4068-a91f-42ebf3b9b03e");

            var article = new Article(vmArticleAdd.Title, vmArticleAdd.Content, userId, useremail, vmArticleAdd.CategoryId, imageId);
          
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
        public async Task<string> UpdateArticleAsync(VMArticleUpdate vmArticleUpdate)
        {
            var article = await unitOfWork.GetRepository<Article>().GetAsync(x => !x.IsDeleted & x.Id == vmArticleUpdate.Id, x => x.Category);
            
            article.Title = vmArticleUpdate.Title;
            article.Content = vmArticleUpdate.Content;
            article.CategoryId = vmArticleUpdate.CategoryId;
            article.ModifiedDate = DateTime.Now;
            article.ModifiedBy = _user.GetLoggedInEmail();


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
