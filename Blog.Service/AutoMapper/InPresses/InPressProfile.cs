using AutoMapper;
using Blog.Entity.ViewModels.InPresses;
using Blog.Entity.Entities;

namespace Blog.Service.AutoMapper.InPresses
{
    public class InPressProfile : Profile
    {
        public InPressProfile()
        {
            CreateMap<VMInPress, InPress>().ReverseMap();
            CreateMap<VMInPressAdd, InPress>().ReverseMap();
            CreateMap<VMInPressUpdate, InPress>().ReverseMap();
            CreateMap<VMInPressUpdate, VMInPress>().ReverseMap();
        }
    }
}
