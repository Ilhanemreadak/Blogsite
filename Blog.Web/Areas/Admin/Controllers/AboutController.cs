using AutoMapper;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Others;
using Blog.Service.Extensions;
using Blog.Service.Services.Abstractions;
using Blog.Web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NToastNotify;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly IAboutService aboutService;
        private readonly IValidator<About> validator;
        private readonly IMapper mapper;
        private readonly IToastNotification toast;

        public AboutController(IAboutService aboutService, IValidator<About> validator, IMapper mapper, IToastNotification toast)
        {
            this.aboutService = aboutService;
            this.validator = validator;
            this.mapper = mapper;
            this.toast = toast;
        }

        public async Task<IActionResult> Index()
        {
            var abouts = await aboutService.GetAllAbouts();
            return View(abouts);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int aboutId)
        {
            var about = await aboutService.GetAboutAsync(aboutId);
            var map = mapper.Map<About, VMAboutUpdate>(about);

            return View(map); 
        }

        [HttpPost]

        public async Task<IActionResult> Update(VMAboutUpdate vmAboutUpdate)
        {
            var map = mapper.Map<About>(vmAboutUpdate);

            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var title = await aboutService.UpdateAboutAsync(vmAboutUpdate);

                toast.AddSuccessToastMessage(Messages.About.Update(title), new ToastrOptions() { Title = "İşlem Başarılı!" });
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            }

            result.AddToModelState(this.ModelState);

            return View(vmAboutUpdate);

        }
    }
}
