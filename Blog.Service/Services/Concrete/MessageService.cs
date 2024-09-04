using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Messages;
using Blog.Service.Services.Abstractions;

namespace Blog.Service.Services.Concrete
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<VMMessages>> GetAllMessagesNonDeleted()
        {
            var messages = await unitOfWork.GetRepository<ContactMessages>().GetAllAsync(x => !x.IsDeleted);
            var map = mapper.Map<List<VMMessages>>(messages);

            return map;
        } 

        public async Task CreateMessageAsync(VMMessagesAdd vmMessagesAdd)
        {
            var message = mapper.Map<ContactMessages>(vmMessagesAdd);
            await unitOfWork.GetRepository<ContactMessages>().AddAsync(message);
            await unitOfWork.SaveAsync();
        }

        public async Task<ContactMessages> GetMessageByIdAsync (int messageId)
        {
            var message = await unitOfWork.GetRepository<ContactMessages>().GetByIdAsync(messageId);
            return message;
        }

        public async Task<string> SafeDeleteMessageAsync(int messageId)
        {
            var message = await unitOfWork.GetRepository<ContactMessages>().GetByIdAsync(messageId);

            message.IsDeleted = true;

            await unitOfWork.GetRepository<ContactMessages>().UpdateAsync(message);
            await unitOfWork.SaveAsync();

            return message.Name;
        }

        public async Task<List<VMMessages>> GetAllMessagesDeleted()
        {
            var messages = await unitOfWork.GetRepository<ContactMessages>().GetAllAsync(x => x.IsDeleted);
            var map = mapper.Map<List<VMMessages>>(messages);

            return map;
        }

        public async Task<string> UndoDeleteMessageAsync(int messageId)
        {
            var message = await unitOfWork.GetRepository<ContactMessages>().GetByIdAsync(messageId);

            message.IsDeleted = false;

            await unitOfWork.GetRepository<ContactMessages>().UpdateAsync(message);
            await unitOfWork.SaveAsync();

            return message.Name;
        }

    }
}
