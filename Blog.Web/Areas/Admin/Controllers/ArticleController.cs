using AutoMapper;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Articles;
using Blog.Service.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        public ArticleController(IArticleService articleService, ICategoryService categoryService, IMapper mapper)
        {
            this.articleService = articleService;
            this.categoryService = categoryService;
            this.mapper = mapper;
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
            await articleService.CreateArticleAsync(vmArticleAdd);
            RedirectToAction("Index", "Article", new { Area = "Admin" });

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
            await articleService.UpdateArticleAsync(vmArticleUpdate);

            var categories = await categoryService.GetAllCategoriesNonDeleted();

            vmArticleUpdate.Categories = categories;

            return View(vmArticleUpdate);
        }
        public async Task<IActionResult> Delete(Guid articleId)
        {
            await articleService.SafeDeleteArticleAsync(articleId);
            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }
    }
}
