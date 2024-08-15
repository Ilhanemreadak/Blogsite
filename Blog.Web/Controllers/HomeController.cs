using Blog.Service.Services.Abstractions;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService articleService;
        private readonly IAboutService aboutService;
        private readonly ISocialMediaService socialMediaService;

        public HomeController(ILogger<HomeController> logger, IArticleService articleService, IAboutService aboutService, ISocialMediaService socialMediaService)
        {
            _logger = logger;
            this.articleService = articleService;
            this.aboutService = aboutService;
            this.socialMediaService = socialMediaService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await articleService.GetAllArticlesWithCategoryNonDeletedAsync();
            return View(articles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]

        public async Task<IActionResult> GetAbout()
        {
            var about = await aboutService.GetAboutAsync(1);
            return Json(about);
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

    }
}
