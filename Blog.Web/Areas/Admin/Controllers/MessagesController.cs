using AutoMapper;
using Blog.Service.Services.Abstractions;
using Blog.Web.ResultMessages;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MessagesController : Controller
    {
        private readonly IMessageService messageService;
        private readonly IMapper mapper;
        private readonly IToastNotification toast;

        public MessagesController(IMessageService messageService, IMapper mapper, IToastNotification toast) 
        {
            this.messageService = messageService;
            this.mapper = mapper;
            this.toast = toast;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await messageService.GetAllMessagesNonDeleted();
            return View(messages);
        }

        public async Task<IActionResult> DeletedMessages()
        {
            var messages = await messageService.GetAllMessagesDeleted();
            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> Full(int messageId)
        {
			var message = await messageService.GetMessageByIdAsync(messageId);
			return View(message);
        }

        public async Task<IActionResult> Delete(int messageId)
        {
            var name = await messageService.SafeDeleteMessageAsync(messageId);
            toast.AddSuccessToastMessage(Messages.ContactMessage.Delete(name), new ToastrOptions() { Title = "İşlem Başarılı!" });
            return RedirectToAction("Index", "Messages", new { Area = "Admin" });
        }
        public async Task<IActionResult> UndoDelete(int messageId)
        {
            var name = await messageService.UndoDeleteMessageAsync(messageId);
            toast.AddSuccessToastMessage(Messages.ContactMessage.UndoDelete(name), new ToastrOptions() { Title = "İşlem Başarılı!" });
            return RedirectToAction("Index", "Messages", new { Area = "Admin" });
        }
    }
}
