
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace LouiseTieDyeStore.Server.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public MessageService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<bool>> DeleteMessage(int id)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Message Not Found"
                };
            }
            else
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();

                return new ServiceResponse<bool>
                {
                    Success = true,
                    Message = "Message Successfully Deleted"
                };
            }          
        }

        public async Task<ServiceResponse<Message>> GetMessage(int id)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
            if (message == null)
            {
                return new ServiceResponse<Message>
                {
                    Success = false,
                    Message = "Message Not Found"
                };
            }
            else
            {
                message.Read = true;
                await _context.SaveChangesAsync();

                return new ServiceResponse<Message> { Data = message };
            }
        }

        public async Task<ServiceResponse<MessagePageResults>> GetMessages(bool unreadOnly, int page)
        {
            var pageResults = 20f;
            int count;

            if (unreadOnly)
            {
                count = await _context.Messages.Where(m => m.Read == false).CountAsync();
            }
            else
            {
                count = await _context.Messages.CountAsync();
            }

            if (count == 0)
            {
                return new ServiceResponse<MessagePageResults>
                {
                    Success = false,
                    Message = "No Messages Found"
                };
            }

            var pageCount = Math.Ceiling(count / pageResults);

            List<Message> messages = new();

            if (unreadOnly)
            {
                messages = await _context.Messages
                    .Where(m => m.Read == false)
                    .OrderByDescending(m => m.Date)
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();
            }
            else
            {
                messages = await _context.Messages
                    .OrderByDescending(m => m.Date)
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();
            }

            return new ServiceResponse<MessagePageResults>
            {
                Data = new MessagePageResults
                {
                    Messages = messages,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };           
        }

        public async Task<ServiceResponse<bool>> SaveMessage(Message message)
        {
            try
            {
                message.Date = DateTime.UtcNow;

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                await SendMessageNotification(message);

                return new ServiceResponse<bool>();
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    Success = false
                };
            }
        }

        // SendGrid.com
        public async Task<string> SendMessageNotification(Message message)
        {
            var apiKey = Environment.GetEnvironmentVariable("SendGrid_ApiKey") ?? _configuration["SendGrid_ApiKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(Environment.GetEnvironmentVariable("SendGrid_Email") ?? _configuration["SendGrid_Email"], "Z Creates");
            var to = new EmailAddress(Environment.GetEnvironmentVariable("SendGrid_Email") ?? _configuration["SendGrid_Email"], "Z Creates Admin");

            var subject = "Contact Message Received";
            var plainTextContent = $"Subject: {message.Subject}";
            var htmlContent = $"<p><strong>{TimeZoneInfo.ConvertTime(message.Date, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"))}</strong></p>"
                + $"<p>{message.FirstName} {message.LastName}</p>"
                + $"<p>{message.Email}</p>"
                + $"<p>{message.PhoneNumber}</p>"
                + $"<h3>{message.Subject}</h3><hr />";
            List<string> body = message.Body.Split("\n").ToList();

            foreach(var line in body)
            {
                htmlContent += $"<p>{line}</p>";
            }           
            htmlContent += "<a href=\"https://tiedyestore.onrender.com/admin/messages/1\" > View Messages</a>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            //Console.WriteLine(JsonConvert.SerializeObject(response));

            return JsonConvert.SerializeObject(response);
        }
    }
}
