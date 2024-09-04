using Blog.Service.Services.Abstractions;
using Blog.Service.Services.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class AboutController : Controller
    {
        private readonly ISocialMediaService socialMediaService;
        private readonly IAboutService aboutService;

        public AboutController(IAboutService aboutService, ISocialMediaService socialMediaService) 
        {
            this.socialMediaService = socialMediaService;
            this.aboutService = aboutService;
        }

        public async Task<IActionResult> Index()
        {
            var about = await aboutService.GetAboutAsync(1);
            return View(about);
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
