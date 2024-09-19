using AutoMapper;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Educationals;
using Blog.Service.Services.Abstractions;
using Blog.Service.Services.Concrete;
using Blog.Web.Consts;
using Blog.Web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Blog.Service.Extensions;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EducationalController : Controller
    {
        private readonly IEducationalService educationalService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        private readonly IValidator<Educational> validator;
        private readonly IToastNotification toast;

        public EducationalController(IEducationalService educationalService, ICategoryService categoryService, IMapper mapper, IValidator<Educational> validator, IToastNotification toast)
        {
            this.educationalService = educationalService;
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.validator = validator;
            this.toast = toast;
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.validator = validator;
            this.toast = toast;
            this.educationalService = educationalService;
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}, {RoleConsts.Role3}")]
        public async Task<IActionResult> Index()
        {
            var educationals = await educationalService.GetAllEducationalsWithCategoryNonDeletedAsync();
            return View(educationals);
        }


        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> DeletedEducationals()
        {
            var educationals = await educationalService.GetAllEducationalsWithCategoryDeletedAsync();
            return View(educationals);
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> Add()
        {
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(new VMEducationalAdd { Categories = categories });
        }


        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> Add(VMEducationalAdd vmEducationalAdd)
        {
            var map = mapper.Map<Educational>(vmEducationalAdd);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await educationalService.CreateEducationalAsync(vmEducationalAdd);
                toast.AddSuccessToastMessage(Messages.Article.Add(vmEducationalAdd.Title), new ToastrOptions() { Title = "İşlem Başarılı!" });
                return RedirectToAction("Index", "Educational", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
            }

            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(new VMEducationalAdd { Categories = categories });

        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> Update(Guid educationalId)
        {
            var educational = await educationalService.GetEducationalsWithCategoryNonDeletedAsync(educationalId);
            var categories = await categoryService.GetAllCategoriesNonDeleted();

            var vmEducationalUpdate = mapper.Map<VMEducationalUpdate>(educational);
            vmEducationalUpdate.Categories = categories;

            return View(vmEducationalUpdate);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> Update(VMEducationalUpdate vmEducationalUpdate)
        {
            var map = mapper.Map<Educational>(vmEducationalUpdate);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var title = await educationalService.UpdateEducationalAsync(vmEducationalUpdate);
                toast.AddSuccessToastMessage(Messages.Article.Update(title), new ToastrOptions() { Title = "İşlem Başarılı!" });
                return RedirectToAction("Index", "Educational", new { Area = "Admin" });

            }
            else
            {
                result.AddToModelState(this.ModelState);
            }

            var categories = await categoryService.GetAllCategoriesNonDeleted();

            vmEducationalUpdate.Categories = categories;

            return View(vmEducationalUpdate);
        }
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> Delete(Guid educationalId)
        {
            var title = await educationalService.SafeDeleteEducationalAsync(educationalId);
            toast.AddSuccessToastMessage(Messages.Article.Delete(title), new ToastrOptions() { Title = "İşlem Başarılı!" });
            return RedirectToAction("Index", "Educational", new { Area = "Admin" });
        }
        [Authorize(Roles = $"{RoleConsts.Role1}, {RoleConsts.Role2}")]
        public async Task<IActionResult> UndoDelete(Guid educationalId)
        {
            var title = await educationalService.UndoDeleteEducationalAsync(educationalId);
            toast.AddSuccessToastMessage(Messages.Article.UndoDelete(title), new ToastrOptions() { Title = "İşlem Başarılı!" });
            return RedirectToAction("Index", "Educational", new { Area = "Admin" });
        }

    }
}
