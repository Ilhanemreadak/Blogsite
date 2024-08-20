using Blog.Service.Services.Abstractions;
using Blog.Service.Services.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class ArticleController : Controller
    {
		private readonly ISocialMediaService socialMediaService;

		public ArticleController(ISocialMediaService socialMediaService)
        {
			this.socialMediaService = socialMediaService;
		}
        public IActionResult Index()
        {
            return View();
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
