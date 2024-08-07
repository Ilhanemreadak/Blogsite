using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.Enums;
using Blog.Entity.ViewModels.Articles;
using Blog.Entity.ViewModels.Users;
using Blog.Service.Extensions;
using Blog.Service.Helpers.Images;
using Blog.Web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NToastNotify;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IMapper mapper;
        private readonly IValidator<AppUser> validator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IImageHelper imageHelper;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IToastNotification toast;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper, IValidator<AppUser> validator, IUnitOfWork unitOfWork, IImageHelper imageHelper, SignInManager<AppUser> signInManager, IToastNotification toast)
        {
            this.userManager = userManager;
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
            var users = await userManager.Users.ToListAsync();
            var map = mapper.Map<List<VMUser>>(users);

            foreach (var item in map)
            {
                var findUser = await userManager.FindByIdAsync(item.Id.ToString());
                var role = string.Join("", await userManager.GetRolesAsync(findUser));

                item.Role = role;
            }

            return View(map);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return View(new VMUserAdd { Roles = roles });
        }
        [HttpPost]
        public async Task<IActionResult> Add(VMUserAdd vmUserAdd)
        {
            var map = mapper.Map<AppUser>(vmUserAdd);
            var validation = await validator.ValidateAsync(map);
            var roles = await roleManager.Roles.ToListAsync();
            if (ModelState.IsValid)
            {
                map.UserName = vmUserAdd.Email;
                var result = await userManager.CreateAsync(map, string.IsNullOrEmpty(vmUserAdd.Password) ? "" : vmUserAdd.Password);
                if (result.Succeeded)
                {
                    var findRole = await roleManager.FindByIdAsync(vmUserAdd.RoleId.ToString());
                    await userManager.AddToRoleAsync(map, findRole.ToString());
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
            var user = await userManager.FindByIdAsync(userId.ToString());

            var roles = await roleManager.Roles.ToListAsync();

            var map = mapper.Map<VMUserUpdate>(user);
            map.Roles = roles;

            return View(map);
        }

        [HttpPost]
        public async Task<IActionResult> Update(VMUserUpdate vmUserUpdate)
        {
            var user = await userManager.FindByIdAsync(vmUserUpdate.Id.ToString());

            if (user != null)
            {
                var userRole = string.Join("", await userManager.GetRolesAsync(user));
                var roles = await roleManager.Roles.ToListAsync();

                if (ModelState.IsValid)
                {
                    var map = mapper.Map(vmUserUpdate, user);
                    var validation = await validator.ValidateAsync(map);

                    if (validation.IsValid) 
                    {
                        user.UserName = vmUserUpdate.Email;
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        var result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            await userManager.RemoveFromRoleAsync(user, userRole);

                            var findRole = await roleManager.FindByIdAsync(vmUserUpdate.RoleId.ToString());
                            await userManager.AddToRoleAsync(user, findRole.ToString());
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
            var user = await userManager.FindByIdAsync(userId.ToString());
            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                toast.AddSuccessToastMessage(Messages.User.Delete(user.Email), new ToastrOptions() { Title = "İşlem Başarılı!" });
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            else
            {
                result.AddToIdentityModelState(this.ModelState);
            }
            return NotFound();

        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var getImage = await unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == user.Id, x => x.Image);
            var map = mapper.Map<VMUserProfile>(user);
            map.Image.FileName = getImage.Image.FileName;

            return View(map);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(VMUserProfile vmUserProfile)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid) 
            {
                var isVerified = await userManager.CheckPasswordAsync(user, vmUserProfile.CurrentPassword);
                if (isVerified && vmUserProfile.NewPassword !=null && vmUserProfile.Photo != null) 
                {
                    var result = await userManager.ChangePasswordAsync(user, vmUserProfile.CurrentPassword, vmUserProfile.NewPassword);
                    if (result.Succeeded)
                    {
                        await userManager.UpdateSecurityStampAsync(user);
                        await signInManager.SignOutAsync();
                        await signInManager.PasswordSignInAsync(user, vmUserProfile.NewPassword, true, false);

                        user.FirstName = vmUserProfile.FirstName;
                        user.LastName = vmUserProfile.LastName;
                        user.PhoneNumber = vmUserProfile.PhoneNumber;

                        var imageUpload = await imageHelper.Upload($"{vmUserProfile.FirstName}{vmUserProfile.LastName}", vmUserProfile.Photo, ImageType.User);
                        Image image = new(imageUpload.FullName, vmUserProfile.Photo.ContentType, user.Email);
                        await unitOfWork.GetRepository<Image>().AddAsync(image);

                        user.ImageId = image.Id;

                        await userManager.UpdateAsync(user);

                        await unitOfWork.SaveAsync();

                        toast.AddSuccessToastMessage("Bilgileriniz ve şifreniz başarıyla güncellenmiştir.");
                        return RedirectToAction("Index", "User", new { Area = "Admin" });
                    }
                    else
                    {
                        result.AddToIdentityModelState(ModelState);
                        return RedirectToAction("Index", "User", new { Area = "Admin" });
                    }
                }
                else if(isVerified && vmUserProfile.Photo != null)
                {
                    await userManager.UpdateSecurityStampAsync(user);

                    user.FirstName = vmUserProfile.FirstName;
                    user.LastName = vmUserProfile.LastName;
                    user.PhoneNumber = vmUserProfile.PhoneNumber;

                    var imageUpload = await imageHelper.Upload($"{vmUserProfile.FirstName} {vmUserProfile.LastName}", vmUserProfile.Photo, ImageType.User);
                    Image image = new(imageUpload.FullName, vmUserProfile.Photo.ContentType, user.Email);
                    await unitOfWork.GetRepository<Image>().AddAsync(image);

                    user.ImageId = image.Id;

                    await userManager.UpdateAsync(user);
                    await unitOfWork.SaveAsync();

                    toast.AddSuccessToastMessage("Bilgileriniz başarıyla güncellenmiştir.");
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                    toast.AddErrorToastMessage("Bilgileriniz güncellenirken bir hata oluştu.");
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
            }

            return View();
        }

    }

}
