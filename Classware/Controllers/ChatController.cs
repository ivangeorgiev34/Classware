
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Controllers
{
    public class ChatController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SendMessage()
        {
            return View();
        }

    }
}
