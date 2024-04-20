
using Blazored.LocalStorage;

namespace LouiseTieDyeStore.Client.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _publicClient;
        private readonly HttpClient _privateClient;

        public MessageService(PublicClient publicClient, HttpClient privateClient)
        {
            _publicClient = publicClient.Client;
            _privateClient = privateClient;
        }

        public List<Message> MessageList { get; set; } = new List<Message>();
        public string LoadingMessage { get; set; } = "Loading Messages...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public bool UnreadOnly { get; set; } = false;
        public int UnreadMessages { get; set; }

        public event Action OnChange;

        public async Task<bool> DeleteMessage(int id)
        {
            var result = await _privateClient.DeleteAsync($"api/message/{id}");
            var success = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<bool>>()).Success;
            return success;
        }

        public async Task<ServiceResponse<Message>> GetMessage(int id)
        {
            var result = await _privateClient.GetFromJsonAsync<ServiceResponse<Message>>($"api/message/{id}");
            return result;
        }

        public async Task GetMessages(int page)
        {
            var result = await _privateClient.GetFromJsonAsync<ServiceResponse<MessagePageResults>>($"api/message/{UnreadOnly}/{page}");

            if (result == null || result.Success == false)
            {
                MessageList.Clear();
                LoadingMessage = "No Messages Found";
            }
            else
            {
                MessageList = result.Data.Messages;
                PageCount = result.Data.Pages;
                CurrentPage = result.Data.CurrentPage;
                LoadingMessage = "Loading Messages...";
            }
        }

        public async Task GetUnreadMessagesCount()
        {
            var result = await _privateClient.GetFromJsonAsync<ServiceResponse<int>>("api/message/count");
            var count = result.Data;

            UnreadMessages = count;
            OnChange.Invoke();
        }

        public async Task<bool> SaveMessage(Message message)
        {
            var result = await _publicClient.PostAsJsonAsync("api/message", message);
            var success = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<bool>>()).Success;
            return success;
        }
    }
}
