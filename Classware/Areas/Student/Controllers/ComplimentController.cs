using Classware.Areas.Student.Models.Compliment;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;

namespace Classware.Areas.Student.Controllers
{
	public class ComplimentController : BaseController
	{
		private readonly IStudentService studentService;
		private readonly IComplimentService complimentService;

		public ComplimentController(IStudentService _studentService,
			IComplimentService _complimentService)
        {
            studentService = _studentService;
			complimentService = _complimentService;
        }
        [HttpGet]
		public async Task<IActionResult> All()
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
						Id = compliment.Id.ToString(),
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

		[HttpGet]
		public async Task<IActionResult> ComplimentInformation(string id)
		{
			try
			{
				var compliment = await complimentService.GetComplimentByIdAsync(id);

				var student = await studentService.GetStudentByUserIdAsync(User.Id());

				var model = new ComplimentInformationViewModel()
				{
					Title = compliment.Title,
					Description = compliment.Description,
					FirstName = student.User.FirstName,
					MiddleName = student.User.MiddleName,
					LastName = student.User.LastName,
					SubjectName = compliment.Subject.Name,
					TeacherName = $"{compliment.Teacher.User.FirstName} {compliment.Teacher.User.MiddleName} {compliment.Teacher.User.LastName}"
				};

				return View(model);
			}
			catch (NullReferenceException e)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("All", "Compliment", new
				{
					area = "Student"
				});

			}
		}
	}
}
