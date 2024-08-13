using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Categories;
using Blog.Entity.ViewModels.Others;
using Blog.Service.Extensions;
using Blog.Service.Services.Abstractions;
using Blog.Service.Services.Concrete;
using Blog.Web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SocialMediaController : Controller
    {
        private readonly ISocialMediaService socialMediaService;
        private readonly IValidator<SocialMedia> validator;
        private readonly IMapper mapper;
        private readonly IToastNotification toast;

        public SocialMediaController(ISocialMediaService socialMediaService, IValidator<SocialMedia> validator, IMapper mapper, IToastNotification toast)
        {
            this.socialMediaService = socialMediaService;
            this.validator = validator;
            this.mapper = mapper;
            this.toast = toast;
        }

        public async Task<IActionResult> Index()
        {
            var socials = await socialMediaService.GetAllSocialMedias();
            return View(socials);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int socialId)  // Change socialId to id for consistency
        {
            var socialMedia = await socialMediaService.GetSocialMediasAsync(socialId);
            var map = mapper.Map<SocialMedia, VMSocialMediaUpdate>(socialMedia);
            return View(map);
        }


        [HttpPost]
        public async Task<IActionResult> Update(VMSocialMediaUpdate vmSocialMediaUpdate)
        {
            var map = mapper.Map<SocialMedia>(vmSocialMediaUpdate);

            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var name = await socialMediaService.UpdateSocialMediaAsync(vmSocialMediaUpdate);

                toast.AddSuccessToastMessage(Messages.SocialMedia.Update(vmSocialMediaUpdate.GetTypeName()), new ToastrOptions() { Title = "İşlem Başarılı!" });
                return RedirectToAction("Index", "SocialMedia", new { Area = "Admin" });
            }

            result.AddToModelState(this.ModelState);
            return View(vmSocialMediaUpdate);
        }
    }
}
