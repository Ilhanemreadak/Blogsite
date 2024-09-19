using AutoMapper;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Educationals;

namespace Blog.Service.AutoMapper.Educationals
{
    public class EducationalProfile : Profile
    {
        public EducationalProfile()
        {
            CreateMap<VMEducational, Educational>().ReverseMap();
            CreateMap<VMEducationalUpdate, Educational>().ReverseMap();
            CreateMap<VMEducationalUpdate, VMEducational>().ReverseMap();
            CreateMap<VMEducationalAdd, Educational>().ReverseMap();
        }
        
    }
}
