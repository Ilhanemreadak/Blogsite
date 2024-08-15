using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Categories;
using Blog.Entity.ViewModels.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Abstractions
{
    public interface ISocialMediaService
    {
        Task<SocialMedia> GetSocialMediasAsync(int socialId);
        Task<List<VMSocialMedia>> GetAllSocialMedias();
        Task<string> UpdateSocialMediaAsync(VMSocialMediaUpdate vmSocialMediaUpdate);
    }
}
