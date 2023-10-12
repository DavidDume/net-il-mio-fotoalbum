using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System.Data;

namespace net_il_mio_fotoalbum.Controllers
{
    public class MessageController : Controller
    {
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Homepage";
            using var ctx = new PhotoContext();

            var messages = ctx.Messages.ToArray();
            if (!ctx.Messages.Any())
            {
                ViewData["Message"] = "Nessun messaggio trovato";
            }
            return View("Index", messages);
        }
    }
}