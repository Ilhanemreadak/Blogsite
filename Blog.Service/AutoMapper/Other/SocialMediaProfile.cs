using AutoMapper;
using Blog.Data.Migrations;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.AutoMapper.Other
{
    public class SocialMediaProfile : Profile
    {
        public SocialMediaProfile()
        {
            CreateMap<SocialMedia, VMSocialMedia>().ReverseMap();
            CreateMap<SocialMedia, VMSocialMediaUpdate>().ReverseMap();
            CreateMap<SocialMedia, VMSocialMediaAdd>().ReverseMap();
        }
        
    }
}
