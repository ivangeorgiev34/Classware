using Classware.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Classware.Controllers
{
	public class HomeController : BaseController
    {
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error([FromQuery] int statusCode)
		{
			if (statusCode == 400)
			{
				return View("Error400");
			}
			else if (statusCode == 401)
			{
				return View("Error401");
			}
			else if (statusCode == 404)
			{
				return View("Error404");
			}

			return View();
		}
	}
}