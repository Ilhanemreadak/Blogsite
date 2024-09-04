using AutoMapper;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Messages;

namespace Blog.Service.AutoMapper.Messages
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<VMMessages, ContactMessages>().ReverseMap();
            CreateMap<VMMessagesAdd, ContactMessages>().ReverseMap();
        }
    }
}
