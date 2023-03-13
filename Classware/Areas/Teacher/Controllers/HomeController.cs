using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Teacher.Controllers
{
	public class HomeController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
