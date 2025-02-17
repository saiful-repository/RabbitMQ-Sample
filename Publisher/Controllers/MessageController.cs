using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Publisher.Models;
using Publisher.RabbitMQ;

namespace Publisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;
        public MessageController(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(Message message)
        {
            // Send the message to RabbitMQ
            await _messageProducer.SendMessage(message);

            return Ok("Message has been sent to RabbitMQ.");
        }
    }
}
