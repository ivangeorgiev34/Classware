using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Student.Controllers
{
	public class HomeController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
