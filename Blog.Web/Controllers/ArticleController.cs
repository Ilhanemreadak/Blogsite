using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Messages;
using Blog.Service.Services.Abstractions;
using Blog.Service.Services.Concrete;
using Blog.Web.ResultMessages;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.ComponentModel.DataAnnotations;
using static Blog.Web.ResultMessages.Messages;

namespace Blog.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly ISocialMediaService socialMediaService;
        private readonly IAboutService aboutService;
        private readonly IUserService userService;
		private readonly IMapper mapper;
		private readonly IValidator<ContactMessages> validator;
		private readonly IMessageService messageService;
		private readonly IToastNotification toast;

		public ArticleController(IArticleService articleService, ISocialMediaService socialMediaService, IAboutService aboutService, IUserService userService, IMapper mapper, IValidator<ContactMessages> validator, IMessageService messageService, IToastNotification toast)
        {
            this.articleService = articleService;
            this.socialMediaService = socialMediaService;
            this.aboutService = aboutService;
            this.userService = userService;
			this.mapper = mapper;
			this.validator = validator;
			this.messageService = messageService;
			this.toast = toast;
		}
        public async Task<IActionResult> Index(Guid? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
			var articles = await articleService.GetAllByPagingAsync(categoryId, currentPage, pageSize, isAscending);
            return View(articles);
        }

		[HttpGet]

		public async Task<IActionResult> GetSocialMediaLinks()
		{
			var socials = await socialMediaService.GetAllSocialMedias();
			List<string> links = new List<string>();

			foreach (var social in socials)
			{
				if (social.Link != null)
				{
					links.Add(social.Link);
				}
			}

			var jsons = Json(links);
			return jsons;
		}

        [HttpGet]

        public async Task<IActionResult> GetAbout()
        {
            var about = await aboutService.GetAboutAsync(1);
            return Json(about);
        }
        public async Task<IActionResult> Detail(Guid Id)
		{
			var article = await articleService.GetArticlesWithCategoryNonDeletedAsync(Id);

			var VisitorResult = await articleService.ArticleVisitorCheckerAsync(Id);

			if (VisitorResult)
				return View(article);
			else
				return View(article);
			
		}

		[HttpGet]
		public async Task<IActionResult> GetWriterPP(Guid Id)
		{
			var article = await articleService.GetArticlesWithCategoryNonDeletedAsync(Id);

            var userImage = await userService.GetAppUsersImageByIdAsync(article.User.Id);

            article.User.Image = userImage;


            if (article != null && article.User != null && article.User.Image != null)
			{
				string filename = article.User.Image.FileName;

				return Json(new { success = true, filename });
			}

			return Json(new { success = false, message = "Article or user image not found" });
		}

    }
}
