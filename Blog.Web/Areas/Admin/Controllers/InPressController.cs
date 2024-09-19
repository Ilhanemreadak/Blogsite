using AutoMapper;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Educationals;
using Blog.Entity.ViewModels.InPresses;
using Blog.Service.Extensions;
using Blog.Service.Services.Abstractions;
using Blog.Web.Consts;
using Blog.Web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InPressController : Controller
    {
        private readonly IInPressService inPressService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IValidator<InPress> validator;
        private readonly IToastNotification toast;

        public InPressController(IInPressService inPressService, ICategoryService categoryService, IMapper mapper, IValidator<InPress> validator, IToastNotification toast)
        {
            this.inPressService = inPressService;
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.validator = validator;
            this.toast = toast;
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}, {RoleConsts.Role3}")]
        public async Task<IActionResult> Index()
        {
            var inpresses = await inPressService.GetAllInPressesWithCategoryNonDeletedAsync();
            return View(inpresses);
        }


        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> DeletedInPress()
        {
            var inpresses = await inPressService.GetAllInPressesWithCategoryDeletedAsync();
            return View(inpresses);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> Add()
        {
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(new VMInPressAdd { Categories = categories });
        }


        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> Add(VMInPressAdd vmInPressAdd)
        {
            var map = mapper.Map<InPress>(vmInPressAdd);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await inPressService.CreateInPressAsync(vmInPressAdd);
                toast.AddSuccessToastMessage(Messages.Article.Add(vmInPressAdd.Title), new ToastrOptions() { Title = "İşlem Başarılı!" });
                return RedirectToAction("Index", "InPress", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
            }

            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(new VMInPressAdd { Categories = categories });

        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> Update(Guid inpressId)
        {
            var inpress = await inPressService.GetInPressesWithCategoryNonDeletedAsync(inpressId);
            var categories = await categoryService.GetAllCategoriesNonDeleted();

            var vmInPressUpdate = mapper.Map<VMInPressUpdate>(inpress);
            vmInPressUpdate.Categories = categories;

            return View(vmInPressUpdate);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> Update(VMInPressUpdate vmInPressUpdate)
        {
            var map = mapper.Map<InPress>(vmInPressUpdate);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var title = await inPressService.UpdateInPressAsync(vmInPressUpdate);
                toast.AddSuccessToastMessage(Messages.Article.Update(title), new ToastrOptions() { Title = "İşlem Başarılı!" });
                return RedirectToAction("Index", "InPress", new { Area = "Admin" });

            }
            else
            {
                result.AddToModelState(this.ModelState);
            }

            var categories = await categoryService.GetAllCategoriesNonDeleted();

            vmInPressUpdate.Categories = categories;

            return View(vmInPressUpdate);
        }
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> Delete(Guid inpressId)
        {
            var title = await inPressService.SafeDeleteInPressAsync(inpressId);
            toast.AddSuccessToastMessage(Messages.Article.Delete(title), new ToastrOptions() { Title = "İşlem Başarılı!" });
            return RedirectToAction("Index", "InPress", new { Area = "Admin" });
        }
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> UndoDelete(Guid inpressId)
        {
            var title = await inPressService.UndoDeleteInPressAsync(inpressId);
            toast.AddSuccessToastMessage(Messages.Article.UndoDelete(title), new ToastrOptions() { Title = "İşlem Başarılı!" });
            return RedirectToAction("Index", "InPress", new { Area = "Admin" });
        }
    }
}
