using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Abstractions
{
    public interface IUserService
    {
        Task<List<VMUser>> GetAllUsersWithRoleAsync();
        Task<List<AppRole>> GetAllRolesAsync();
        Task<IdentityResult> CreateUserAsync(VMUserAdd vmUserAdd);
        Task<IdentityResult> UpdateUserAsync(VMUserUpdate vmUserUpdate);
        Task<(IdentityResult IdentityResult, string? email)> DeleteUserAsync(Guid userId);
        Task<AppUser> GetAppUserByIdAsync(Guid userId);
        Task<string> GetUserRoleAsync(AppUser user);
        Task<VMUserProfile> GetUserProfileAsync();
        Task<bool> UserProfileUpdateAsync(VMUserProfile vmUserProfile);
        Task<Image> GetAppUsersImageByIdAsync(Guid userId);
    }
}
