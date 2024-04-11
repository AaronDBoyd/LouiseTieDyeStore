
using Newtonsoft.Json;

namespace LouiseTieDyeStore.Server.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly DataContext _context;

        public MessageService(DataContext context)
        {
            _context = context;
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
                message.Date = DateTime.Now;

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

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
    }
}
