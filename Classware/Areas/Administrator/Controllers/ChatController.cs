using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Administrator.Controllers
{
	public class ChatController : BaseController
	{
		[HttpGet]
		public async Task<IActionResult> AllMessages()
		{

			return View();

		}
	}
}
