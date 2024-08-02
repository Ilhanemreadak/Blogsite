using AutoMapper;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Articles;
using Blog.Service.Extensions;
using Blog.Service.Services.Abstractions;
using Blog.Web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IValidator<Article> validator;
        private readonly IToastNotification toast;

        public ArticleController(IArticleService articleService, ICategoryService categoryService, IMapper mapper, IValidator<Article> validator, IToastNotification toast)
        {
            this.articleService = articleService;
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.validator = validator;
            this.toast = toast;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await articleService.GetAllArticlesWithCategoryNonDeletedAsync();
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(new VMArticleAdd { Categories = categories });
        }

        [HttpPost]
        public async Task<IActionResult> Add(VMArticleAdd vmArticleAdd)
        {
            var map = mapper.Map<Article>(vmArticleAdd);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await articleService.CreateArticleAsync(vmArticleAdd);
                toast.AddSuccessToastMessage(Messages.Article.Add(vmArticleAdd.Title), new ToastrOptions() { Title = "İşlem Başarılı!"});
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
            }

            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(new VMArticleAdd { Categories = categories });

        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid articleId)
        {
            var article = await articleService.GetArticlesWithCategoryNonDeletedAsync(articleId);
            var categories = await categoryService.GetAllCategoriesNonDeleted();

            var vmArticleUpdate = mapper.Map<VMArticleUpdate>(article);
            vmArticleUpdate.Categories = categories;

            return View(vmArticleUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Update(VMArticleUpdate vmArticleUpdate)
        {
            var map = mapper.Map<Article>(vmArticleUpdate);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid) 
            {
                var title = await articleService.UpdateArticleAsync(vmArticleUpdate);
                toast.AddSuccessToastMessage(Messages.Article.Update(title), new ToastrOptions() { Title = "İşlem Başarılı!" });
                return RedirectToAction("Index", "Article", new { Area = "Admin" });

            }
            else
            {
                result.AddToModelState(this.ModelState);
            }

            var categories = await categoryService.GetAllCategoriesNonDeleted();

            vmArticleUpdate.Categories = categories;

            return View(vmArticleUpdate);
        }
        public async Task<IActionResult> Delete(Guid articleId)
        {
            var title = await articleService.SafeDeleteArticleAsync(articleId);
            toast.AddSuccessToastMessage(Messages.Article.Delete(title), new ToastrOptions() { Title = "İşlem Başarılı!" });
            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }
    }
}
