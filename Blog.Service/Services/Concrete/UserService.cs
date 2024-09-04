using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.Enums;
using Blog.Entity.ViewModels.Users;
using Blog.Service.Extensions;
using Blog.Service.Helpers.Images;
using Blog.Service.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog.Service.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly IImageHelper imageHelper;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ClaimsPrincipal _user;

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper, IImageHelper imageHelper, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            this.unitOfWork = unitOfWork;
            _user = httpContextAccessor.HttpContext.User;
            this.mapper = mapper;
            this.imageHelper = imageHelper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(VMUserAdd vmUserAdd)
        {

            if (vmUserAdd.Image == null) {
                var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user-images", "default-image.png");
                var fileName = "default-image.png";

                byte[] imageBytes = await File.ReadAllBytesAsync(defaultImagePath);
                using var stream = new MemoryStream(imageBytes);

                var image = new FormFile(stream, 0, stream.Length, "image", "default.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                var fullName = await imageHelper.Upload(fileName, image, ImageType.User);

                using var ConvertedImage = System.Drawing.Image.FromStream(stream);

                var newImage = new Blog.Entity.Entities.Image
                {
                    FileName = "user-images/default-image.png",
                    FileType = image.ContentType,
                    CreatedBy = "System"
                };

                vmUserAdd.Image = newImage;

            }

            var map = mapper.Map<AppUser>(vmUserAdd);

            map.UserName = vmUserAdd.Email;
            var result = await userManager.CreateAsync(map, string.IsNullOrEmpty(vmUserAdd.Password) ? "" : vmUserAdd.Password);
            
            if (result.Succeeded)
            {
                var findRole = await roleManager.FindByIdAsync(vmUserAdd.RoleId.ToString());
                await userManager.AddToRoleAsync(map, findRole.ToString());
                return(result);
            }
            else
                return result;
        }

        public async Task<(IdentityResult IdentityResult, string? email)> DeleteUserAsync(Guid userId)
        {
            var user = await GetAppUserByIdAsync(userId);
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded) {
                return (result, user.Email);
            }
            else return (result, user.Email);
        }

        public async Task<List<VMUser>> GetAllUsersWithRoleAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var map = mapper.Map<List<VMUser>>(users);

            foreach (var item in map)
            {
                var findUser = await userManager.FindByIdAsync(item.Id.ToString());
                var role = string.Join("", await userManager.GetRolesAsync(findUser));

                item.Role = role;
            }

            return (map);
        }

        public async Task<List<AppRole>> GetAllRolesAsync()
        {
            return await roleManager.Roles.ToListAsync();

        }

        public async Task<AppUser> GetAppUserByIdAsync(Guid userId)
        {
            return await userManager.FindByIdAsync(userId.ToString());
        }
        
        public async Task<Image> GetAppUsersImageByIdAsync(Guid userId)
        {
            var user = await unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == userId);
            var image = await unitOfWork.GetRepository<Image>().GetAsync(x => x.Id == user.ImageId);
            return image;
        }
        
        public async Task<string> GetUserRoleAsync(AppUser user)
        {
            return string.Join("", await userManager.GetRolesAsync(user));
        }

        public async Task<IdentityResult> UpdateUserAsync(VMUserUpdate vmUserUpdate)
        {
            var user = await GetAppUserByIdAsync(vmUserUpdate.Id);
            var userRole = await GetUserRoleAsync(user);
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await userManager.RemoveFromRoleAsync(user, userRole);
                var findRole = await roleManager.FindByIdAsync(vmUserUpdate.RoleId.ToString());
                await userManager.AddToRoleAsync(user, findRole.Name);
                return result;
            }
            else
                return result;
        }

        public async Task<VMUserProfile> GetUserProfileAsync()
        {
            var userId = _user.GetLoggedInUserId();
            var getUserWithImage = await unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == userId, x => x.Image);
            var map = mapper.Map<VMUserProfile>(getUserWithImage);
            map.Image.FileName = getUserWithImage.Image.FileName;

            return map;
        }

        private async Task<(Guid ImageId, Image Image)> UploadImageForUser(VMUserProfile vmUserProfile)
        {
            var userEmail = _user.GetLoggedInEmail();
            var imageUpload = await imageHelper.Upload($"{vmUserProfile.FirstName} {vmUserProfile.LastName}", vmUserProfile.Photo, ImageType.User);
            Image image = new(imageUpload.FullName, vmUserProfile.Photo.ContentType, userEmail);
            await unitOfWork.GetRepository<Image>().AddAsync(image);
            await unitOfWork.SaveAsync();
            vmUserProfile.Image = image;

            return (image.Id, image);
        }

        public async Task<bool> UserProfileUpdateAsync(VMUserProfile vmUserProfile)
        {
            var userId = _user.GetLoggedInUserId();
            var user = await GetAppUserByIdAsync(userId);

            var isVerified = await userManager.CheckPasswordAsync(user, vmUserProfile.CurrentPassword);
            if (isVerified && vmUserProfile.NewPassword != null)
            {
                var result = await userManager.ChangePasswordAsync(user, vmUserProfile.CurrentPassword, vmUserProfile.NewPassword);
                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    await signInManager.SignOutAsync();
                    await signInManager.PasswordSignInAsync(user, vmUserProfile.NewPassword, true, false);

                    mapper.Map(vmUserProfile, user);
                    if (vmUserProfile.Photo != null)
                    {
                        (user.ImageId , user.Image) = await UploadImageForUser(vmUserProfile);
                    }

                    await userManager.UpdateAsync(user);
                    await unitOfWork.SaveAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (isVerified)
            {
                await userManager.UpdateSecurityStampAsync(user);

                mapper.Map(vmUserProfile, user);
                if (vmUserProfile.Photo != null)
                {
                    (user.ImageId, user.Image) = await UploadImageForUser(vmUserProfile);
                }

                await userManager.UpdateAsync(user);
                await unitOfWork.SaveAsync();

                return true;
            }
            else
            {
                return false;
            }
            return true;

        }


    }   

}
