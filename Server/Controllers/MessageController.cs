using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LouiseTieDyeStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> SaveMessage(Message message)
        {
            var result = await _messageService.SaveMessage(message);
            return Ok(result);
        }

        [HttpGet("{unreadOnly}/{page}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<MessagePageResults>>> GetMessages(bool unreadOnly, int page)
        {
            var result = await _messageService.GetMessages(unreadOnly, page);
            return Ok(result);
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Message>>> GetMessage(int id)
        {
            var result = await _messageService.GetMessage(id);
            return Ok(result);
        }

        [HttpGet("count"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<int>>> GetUnreadMessagesCount()
        {
            return await _messageService.GetUnreadMessagesCount();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteMessage(int id)
        {
            var result = await (_messageService.DeleteMessage(id));
            return Ok(result);
        }

        // To test the SendGrid service
        [HttpPost("notify-email")]
        public async Task<ActionResult<string>> SendEmail(Message message)
        {
            var result = await _messageService.SendMessageNotification(message);
            return Ok(result);
        }
    }
}
