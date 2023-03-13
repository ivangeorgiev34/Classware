using Classware.Areas.Administrator.Models.Class;
using Classware.Core.Constants;
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
        /// Adds a class 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
		public IActionResult Add()
		{
			try
			{
				var model = new AddClassViewModel();

				return View(model);
			}
			catch (Exception)
			{

				throw;
			}

		}

		[HttpPost]
		public async Task<IActionResult> Add(AddClassViewModel model)
		{
			try
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

				logger.LogInformation("Class with name:{0} has been added!", _class.Name);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = $"Class {_class.Name} has been added successfully!";

				return RedirectToAction("All", "Class", new { area = "Administrator" });
			}
			catch (Exception)
			{

				throw;
			}
			
		}

		/// <summary>
		/// Gets all classes and gets the name of the class and number of their students in that class
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> All()
		{
			try
			{
				ICollection<AllClassesViewModel> models = new List<AllClassesViewModel>();

				var classes = await classService.GetAllClassesAsync();

				foreach (var _class in classes)
				{
					models.Add(new AllClassesViewModel()
					{
						Id = _class.Id,
						Name = _class.Name,
						StudentsCount = _class.Students.Count
					});
				}

				return View(models);
			}
			catch (Exception)
			{

				throw;
			}
			
		}
		
		/// <summary>
		/// Sets the given class to not active
		/// </summary>
		/// <returns></returns>

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await classService.DeleteClassByIdAsync(id);


				logger.LogInformation("Class with id:{0} has been deleted!", id);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Class deleted successfully";

				return RedirectToAction("Index", "Home", new { area = "Administrator" });
			}
			catch (Exception)
			{

				throw;
			}
			
		}
	}
}
