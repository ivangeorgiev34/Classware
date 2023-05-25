using Classware.Models.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Controllers
{
    public class ChatController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SendMessage()
        {
            return View();
        }

    }
}
