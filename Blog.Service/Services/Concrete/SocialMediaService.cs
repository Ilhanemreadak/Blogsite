using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Categories;
using Blog.Entity.ViewModels.Others;
using Blog.Service.Services.Abstractions;

namespace Blog.Service.Services.Concrete
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SocialMediaService(IUnitOfWork unitOfWork, IMapper mapper) {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<SocialMedia> GetSocialMediasAsync(int socialId)
        {
            var SocialMedia = await unitOfWork.GetRepository<SocialMedia>().GetByIdAsync(socialId);
            return SocialMedia;
        }

        public async Task<List<VMSocialMedia>> GetAllSocialMedias()
        {
            var socials = await unitOfWork.GetRepository<SocialMedia>().GetAllAsync();
            var map = mapper.Map<List<VMSocialMedia>>(socials);

            return map;
        }

        public async Task<string> UpdateSocialMediaAsync(VMSocialMediaUpdate vmSocialMediaUpdate)
        {
            var socialMedia = await unitOfWork.GetRepository<SocialMedia>().GetAsync(x => x.Id == vmSocialMediaUpdate.Id);
            
            socialMedia.Link = vmSocialMediaUpdate.Link;

            await unitOfWork.GetRepository<SocialMedia>().UpdateAsync(socialMedia);
            await unitOfWork.SaveAsync();

            return GetSocialMediaTypeMessage(socialMedia.Id);
        }

        private string GetSocialMediaTypeMessage(int typeId)
        {
            if (typeId == 1)
                return "LinkedIn";
            else if (typeId == 2)
                return "Instagram";
            else if (typeId == 3)
                return "Facebook";
            else if (typeId == 4)
                return "X";
            else return "Hata";
        }


    }
}
