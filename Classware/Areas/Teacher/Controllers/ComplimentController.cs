using Classware.Areas.Teacher.Models.Compliment;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Teacher.Controllers
{
	public class ComplimentController : BaseController
	{
		private readonly IStudentService studentService;
		private readonly ITeacherService teacherService;
		private readonly IComplimentService complimentService;

		public ComplimentController(IStudentService _studentService,
			ITeacherService _teacherService,
			IComplimentService _complimentService)
		{
			studentService = _studentService;
			teacherService = _teacherService;
			complimentService = _complimentService;
		}

		/// <summary>
		/// Adds a compliment to a given student
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Add()
		{
			try
			{
				if (!await teacherService.TeacherHasASubjectAsync(User.Id()))
				{
					TempData[UserMessagesConstants.ERROR_MESSAGE] = "You don't have a assigned subject";

					return RedirectToAction("Index", "Home", new { area = "Teacher" });
				}

				var students = await studentService.GetAllStudentsAsync();

				ICollection<StudentViewModel> studentViewModels = new List<StudentViewModel>();

				foreach (var student in students)
				{
					studentViewModels.Add(new StudentViewModel
					{
						Id = student.Id,
						User = student.User,
						UserId = student.Userid,
						Class = student.Class,
						ClassId = student.ClassId
					});
				}

				var model = new AddComplimentViewModel()
				{
					Students = studentViewModels
				};

				return View(model);
			}
			catch (Exception)
			{

				throw;
			}
		
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddComplimentViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}

				var student = await studentService.GetStudentByIdAsync(model.StudentId);

				var teacher = await teacherService.GetTeacherByUserIdAsync(User.Id());

				if (!await studentService.StudentHasASubjectAsync(student, teacher.Subject.Name))
				{
					TempData[UserMessagesConstants.ERROR_MESSAGE] = "Student doesn't have such subject";

					return RedirectToAction("Index", "Home", new { area = "Teacher" });
				}

				await complimentService.AddComplimentAsync(student.Id, teacher.Subject.Id, model.Title, model.Description ?? null);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Compliment added successfully";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (Exception)
			{

				throw;
			}
		}

		/// <summary>
		/// Gets all the compliments of a given student
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> All(int id)
		{
			try
			{
				var student = await studentService.GetStudentByIdAsync(id);

				var compliments = student.Compliments
					.Where(c => c.IsActive)
					.ToList();

				ICollection<ComplimentViewModel> complimentViewModels = new List<ComplimentViewModel>();

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
					FirstName = student.User.FirstName,
					MiddleName = student.User.MiddleName,
					LastName = student.User.LastName,
					ClassName = student.Class.Name,
					StudentId = id,
					Compliments = complimentViewModels
				};

				return View(model);
			}
			catch (Exception)
			{

				throw;
			}
		
		}
		/// <summary>
		/// Sets a given compliment to not active
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await complimentService.DeleteComplimentByIdAsync(id);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Compliment deleted successfully";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (Exception)
			{

				throw;
			}

		}

		/// <summary>
		/// Edits a given compliment
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var compliment = await complimentService.GetComplimentByIdAsync(id);

				var model = new EditComplimentViewModel()
				{
					Id = compliment.Id,
					Title = compliment.Title,
					Description = compliment.Description
				};

				return View(model);
			}
			catch (Exception)
			{

				throw;
			}
	
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditComplimentViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}

				await complimentService.EditComplimentByIdAsync(model.Id, model.Title, model.Description);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Compliment edited successfully";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (Exception)
			{

				throw;
			}
		
		}
	} 
}
