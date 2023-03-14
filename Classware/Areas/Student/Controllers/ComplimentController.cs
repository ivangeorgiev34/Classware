using Classware.Areas.Student.Models.Compliment;
using Classware.Core.Contracts;
using Classware.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Student.Controllers
{
	public class ComplimentController : BaseController
	{
		private readonly IStudentService studentService;

        public ComplimentController(IStudentService _studentService)
        {
            studentService = _studentService;
        }
        [HttpGet]
		public async Task<IActionResult> All()
		{
			try
			{
				var student = await studentService.GetStudentByUserIdAsync(User.Id());

				ICollection<ComplimentViewModel> complimentViewModels = new List<ComplimentViewModel>();

				var compliments = student.Compliments
					.Where(c => c.IsActive == true)
					.ToList();

				foreach (var compliment in compliments)
				{
					complimentViewModels.Add(new ComplimentViewModel()
					{
						Id = compliment.Id,
						Title = compliment.Title,
						Description = compliment.Description
					});
				}

				var model = new AllComplimentsViewModel()
				{
					Compliments = complimentViewModels
				};

				return View(model);
			}
			catch (Exception)
			{

				throw;
			}
		
		}
	}
}
