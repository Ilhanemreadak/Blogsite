using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Service.Services.Abstractions;
using Blog.Service.Services.Concrete;
using Microsoft.AspNetCore.Mvc;
using static Blog.Web.ResultMessages.Messages;

namespace Blog.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly ISocialMediaService socialMediaService;
        private readonly IAboutService aboutService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;

        public ArticleController(IArticleService articleService, ISocialMediaService socialMediaService, IAboutService aboutService, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IUserService userService)
        {
            this.articleService = articleService;
            this.socialMediaService = socialMediaService;
            this.aboutService = aboutService;
            this.httpContextAccessor = httpContextAccessor;
            this.unitOfWork = unitOfWork;
            this.userService = userService;
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
			var ipAdress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
			var articleVisitors = await unitOfWork.GetRepository<ArticleVisitor>().GetAllAsync(null, x => x.Visitor, y => y.Article);
			var article = await unitOfWork.GetRepository<Blog.Entity.Entities.Article>().GetAsync(x=>x.Id == Id);


			var result = await articleService.GetArticlesWithCategoryNonDeletedAsync(Id);

            var visitor = await unitOfWork.GetRepository<Visitor>().GetAsync(x => x.IpAddress == ipAdress);

			var addArticleVisitors = new ArticleVisitor(article.Id, visitor.Id);

			if(articleVisitors.Any(x=>x.VisitorId == addArticleVisitors.VisitorId && x.ArticleId == addArticleVisitors.ArticleId))
				return View(result);
			else
			{
				await unitOfWork.GetRepository<ArticleVisitor>().AddAsync(addArticleVisitors);
				article.ViewCount += 1;
				await unitOfWork.GetRepository<Blog.Entity.Entities.Article>().UpdateAsync(article);
				await unitOfWork.SaveAsync();
			}

			return View(result);
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
