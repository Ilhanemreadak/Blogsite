using Blog.Entity.Enums;
using Blog.Entity.ViewModels.Images;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Helpers.Images
{
    public interface IImageHelper
    {
        Task<VMImageUploded> Upload(string name, IFormFile imageFile, ImageType imageType, string folderName = null);
        void Delete(string imageName);
    }
}
