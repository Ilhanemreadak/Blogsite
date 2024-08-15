using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Abstractions
{
    public interface IAboutService
    {
        Task<About> GetAboutAsync(int aboutId);
        Task<List<VMAbout>> GetAllAbouts();
        Task<string> UpdateAboutAsync(VMAboutUpdate vmAboutUpdate);
    }
}
