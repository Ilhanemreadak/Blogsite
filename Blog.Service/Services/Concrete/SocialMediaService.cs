using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Others;
using Blog.Service.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<SocialMedia> GetSocialMediasAsync()
        {
            var SocialMedia = await unitOfWork.GetRepository<SocialMedia>().GetByIdAsync(1);
            return SocialMedia;
        }

        public async Task CreateSocialMediaAsync(VMSocialMediaAdd vmSocialMediaAdd)
        {
            var map = mapper.Map<SocialMedia>(vmSocialMediaAdd);
            SocialMedia media = new(map.SocialMediaType, map.Link);
            await unitOfWork.GetRepository<SocialMedia>().AddAsync(media);
        }

        public async Task<string> UpdateSocialMediaAsync(VMSocialMediaUpdate vmSocialMediaUpdate)
        {
            var medias = await unitOfWork.GetRepository<SocialMedia>().GetByIdAsync(1);

            medias.SocialMediaType = vmSocialMediaUpdate.SocialMediaType;
            medias.Link = vmSocialMediaUpdate.Link;

            await unitOfWork.GetRepository<SocialMedia>().UpdateAsync(medias);
            await unitOfWork.SaveAsync();

            return await GetSocialMediaTypeMessage(medias.SocialMediaType);
        }

        private async Task<string> GetSocialMediaTypeMessage(int typeId)
        {
            if (typeId == 0)
                return "LinkedIn";
            else if (typeId == 1)
                return "Instagram";
            else if (typeId == 1)
                return "Facebook";
            else if (typeId == 1)
                return "X";
            else return "Hata";
        }


    }
}
