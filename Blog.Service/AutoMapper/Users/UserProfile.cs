using AutoMapper;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.AutoMapper.Users
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        { 
            CreateMap<AppUser, VMUser>().ReverseMap();
            CreateMap<AppUser, VMUserAdd>().ReverseMap();
            CreateMap<AppUser, VMUserUpdate>().ReverseMap();
            CreateMap<AppUser, VMUserProfile>().ReverseMap();
        }
    }
}
