using AutoMapper;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.AutoMapper.Other
{
    public class AboutProfile : Profile
    {
        public AboutProfile()
        {
            CreateMap<About, VMAbout>().ReverseMap();
            CreateMap<About, VMAboutAdd>().ReverseMap();
            CreateMap<About, VMAboutUpdate>().ReverseMap();
        }
    }
}
