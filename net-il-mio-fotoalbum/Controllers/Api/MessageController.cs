using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetMessages()
        {
            using var ctx = new PhotoContext();
            var messages = ctx.Messages.ToList();

            return Ok(messages);

        }

        [HttpPost]
        public IActionResult SendMessage(Message message)
        {
            using var ctx = new PhotoContext();

            ctx.Messages.Add(message);
            ctx.SaveChanges();

            return Ok();
        }
    }
}