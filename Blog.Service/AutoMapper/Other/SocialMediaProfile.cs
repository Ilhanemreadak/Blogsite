using AutoMapper;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Others;

namespace Blog.Service.AutoMapper.Other
{
    public class SocialMediaProfile : Profile
    {
        public SocialMediaProfile()
        {
            CreateMap<SocialMedia, VMSocialMedia>().ReverseMap();
            CreateMap<SocialMedia, VMSocialMediaUpdate>().ReverseMap();
        }
        
    }
}
