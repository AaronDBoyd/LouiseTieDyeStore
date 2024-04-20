namespace LouiseTieDyeStore.Client.Services.MessageService
{
    public interface IMessageService
    {
        List<Message> MessageList { get; set; }
        string LoadingMessage { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        bool UnreadOnly { get; set; }
        int UnreadMessages { get; set; }
        event Action OnChange;
        Task<bool> SaveMessage(Message message);
        Task GetMessages(int page);
        Task<ServiceResponse<Message>> GetMessage(int id);
        Task<bool> DeleteMessage(int id);
        Task GetUnreadMessagesCount();
    }
}
