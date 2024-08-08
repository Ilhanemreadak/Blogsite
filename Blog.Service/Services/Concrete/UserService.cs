using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Users;
using Blog.Service.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateUserAsync(VMUserAdd vmUserAdd)
        {
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

        
    }   

}
