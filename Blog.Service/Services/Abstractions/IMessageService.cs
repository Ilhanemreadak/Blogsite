using Blog.Entity.Entities;
using Blog.Entity.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Abstractions
{
    public interface IMessageService
    {
        Task<List<VMMessages>> GetAllMessagesNonDeleted();
        Task CreateMessageAsync(VMMessagesAdd vmMessagesAdd);
        Task<ContactMessages> GetMessageByIdAsync(int messageId);
        Task<string> SafeDeleteMessageAsync(int messageId);
        Task<List<VMMessages>> GetAllMessagesDeleted();
        Task<string> UndoDeleteMessageAsync(int messageId);
    }
}
