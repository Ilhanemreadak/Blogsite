using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.Enums;
using Blog.Entity.ViewModels.Articles;
using Blog.Entity.ViewModels.Users;
using Blog.Service.Extensions;
using Blog.Service.Helpers.Images;
using Blog.Service.Services.Abstractions;
using Blog.Web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NToastNotify;
using static Blog.Web.ResultMessages.Messages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUserService userService;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IMapper mapper;
        private readonly IValidator<AppUser> validator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IImageHelper imageHelper;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IToastNotification toast;

        public UserController(UserManager<AppUser> userManager, IUserService userService, RoleManager<AppRole> roleManager, IMapper mapper, IValidator<AppUser> validator, IUnitOfWork unitOfWork, IImageHelper imageHelper, SignInManager<AppUser> signInManager, IToastNotification toast)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.validator = validator;
            this.unitOfWork = unitOfWork;
            this.imageHelper = imageHelper;
            this.signInManager = signInManager;
            this.toast = toast;
        }

        public async Task<IActionResult> Index()
        {
            var result = await userService.GetAllUsersWithRoleAsync();

            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var roles = await userService.GetAllRolesAsync();
            return View(new VMUserAdd { Roles = roles });
        }
        [HttpPost]
        public async Task<IActionResult> Add(VMUserAdd vmUserAdd)
        {
            var map = mapper.Map<AppUser>(vmUserAdd);
            var validation = await validator.ValidateAsync(map);
            var roles = await userService.GetAllRolesAsync();
            if (ModelState.IsValid)
            {
                var result = await userService.CreateUserAsync(vmUserAdd);
                if (result.Succeeded)
                {
                    toast.AddSuccessToastMessage(Messages.User.Add(vmUserAdd.Email), new ToastrOptions() { Title = "İşlem Başarılı!" });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                    result.AddToIdentityModelState(this.ModelState);
                    validation.AddToModelState(this.ModelState);
                    return View(new VMUserAdd { Roles = roles });
                }
            }
            return View(new VMUserAdd { Roles = roles });
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid userId)
        {
            var user = await userService.GetAppUserByIdAsync(userId);

            var roles = await userService.GetAllRolesAsync();

            var map = mapper.Map<VMUserUpdate>(user);
            map.Roles = roles;

            return View(map);
        }

        [HttpPost]
        public async Task<IActionResult> Update(VMUserUpdate vmUserUpdate)
        {
            var user = await userService.GetAppUserByIdAsync(vmUserUpdate.Id);

            if (user != null)
            {
                
                var roles = await userService.GetAllRolesAsync();

                if (ModelState.IsValid)
                {
                    var map = mapper.Map(vmUserUpdate, user);
                    var validation = await validator.ValidateAsync(map);

                    if (validation.IsValid) 
                    {
                        user.UserName = vmUserUpdate.Email;
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        var result = await userService.UpdateUserAsync(vmUserUpdate);
                        if (result.Succeeded)
                        {
                            toast.AddSuccessToastMessage(Messages.User.Update(vmUserUpdate.Email), new ToastrOptions() { Title = "İşlem Başarılı!" });
                            return RedirectToAction("Index", "User", new { Area = "Admin" });
                        }
                        else
                        {
                            result.AddToIdentityModelState(this.ModelState);
                            return View(new VMUserUpdate { Roles = roles });
                        }
                    }
                    else
                    {
                        validation.AddToModelState(this.ModelState);
                        return View(new VMUserUpdate { Roles = roles });
                    }
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> Delete(Guid userId)
        {
           
            var result = await userService.DeleteUserAsync(userId);

            if (result.IdentityResult.Succeeded)
            {
                toast.AddSuccessToastMessage(Messages.User.Delete(result.email), new ToastrOptions() { Title = "İşlem Başarılı!" });
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            else
            {
                result.IdentityResult.AddToIdentityModelState(this.ModelState);
            }
            return NotFound();

        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var profile = await userService.GetUserProfileAsync();
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(VMUserProfile vmUserProfile)
        {
            if (ModelState.IsValid) 
            {
               var result = await userService.UserProfileUpdateAsync(vmUserProfile);
               if (result)
               {
                   toast.AddSuccessToastMessage("Profile güncelleme işleme başarıyla tamamlanmıştır.", new ToastrOptions() { Title = "İşlem Başarılı!" });
                   return RedirectToAction("Index", "Home", new { Area = "Admin" });
               }
               else
               {
                    var profile = await userService.GetUserProfileAsync();
                    toast.AddErrorToastMessage("Profil güncelleme işlemi sırasında bir hatayla karşılaşılmıştır.", new ToastrOptions() { Title = "İşlem Başarısız !" });
                    return View(profile);
               }

            }
            else
            {
                
                return NotFound();
            }
                
        }

    }

}
