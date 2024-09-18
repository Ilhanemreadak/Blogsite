using AutoMapper;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Messages;
using Blog.Service.Services.Abstractions;
using Blog.Web.ResultMessages;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Blog.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IMessageService messageService;
        private readonly ISocialMediaService socialMediaService;
        private readonly IMapper mapper;
        private readonly IValidator<ContactMessages> validator;
        private readonly IToastNotification toast;

        public ContactController(IMessageService messageService, ISocialMediaService socialMediaService, IMapper mapper, IValidator<ContactMessages> validator, IToastNotification toast) 
        {
            this.messageService = messageService;
            this.socialMediaService = socialMediaService;
            this.mapper = mapper;
            this.validator = validator;
            this.toast = toast;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(VMMessagesAdd vmMessagesAdd)
        {
            var map = mapper.Map<ContactMessages>(vmMessagesAdd);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await messageService.CreateMessageAsync(vmMessagesAdd);
                toast.AddSuccessToastMessage(Messages.ContactMessage.Add(), new ToastrOptions() { Title = "İşlem Başarılı!" });
                return RedirectToAction("Index", "Contact");
            }
            result.AddToModelState(this.ModelState);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddIndex([FromBody] VMMessagesAdd vmMessagesAdd)
        {
            var map = mapper.Map<ContactMessages>(vmMessagesAdd);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await messageService.CreateMessageAsync(vmMessagesAdd);
                return Json(new { success = true, message = "Mesajınız başarıyla gönderildi." });
            }

            return Json(new { success = false, message = "Mesaj gönderilirken bir hata oluştu." });
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
