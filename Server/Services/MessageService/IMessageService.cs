using Microsoft.AspNetCore.SignalR;

namespace LouiseTieDyeStore.Server.Services.MessageService
{
    public interface IMessageService
    {
        Task<ServiceResponse<bool>> SaveMessage(Message message);
        Task<ServiceResponse<bool>> DeleteMessage(int id);
        Task<ServiceResponse<Message>> GetMessage(int id);
        Task<ServiceResponse<MessagePageResults>> GetMessages(bool unreadOnly, int page);
        Task<string> SendMessageNotification(Message message);
    }
}
