using Classware.Areas.Administrator.Models.Class;
using Classware.Core.Contracts;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Administrator.Controllers
{
	public class ClassController : BaseController
	{
		private readonly IClassService classService;
		private readonly ILogger<ClassController> logger;
        public ClassController(
			IClassService _classService,
			ILogger<ClassController> _logger)
        {
			classService = _classService;
			logger = _logger;
        }
        /// <summary>
        /// adds class 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
		public IActionResult Add()
		{
			var model = new AddClassViewModel();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddClassViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			if (await classService.ClassExistsByNameAsync(model.Name))
			{
				ModelState.AddModelError("", "A class with this name already exists!");

				return View(model);
			}

			var _class = new Class()
			{
				Name = model.Name
			};

			await classService.AddClassAsync(_class);

			logger.LogInformation("Class with name:{0} has been added!",_class.Name);

			return RedirectToAction("Index","Home",new {area="Administrator"});
		}
	}
}
