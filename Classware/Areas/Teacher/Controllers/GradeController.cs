
using Classware.Areas.Teacher.Models.Class;
using Classware.Areas.Teacher.Models.Grade;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Extensions;
using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Configuration;

namespace Classware.Areas.Teacher.Controllers
{
	public class GradeController : BaseController
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly ITeacherService teacherService;
		private readonly IStudentService studentService;
		private readonly IClassService classService;
		private readonly IGradeService gradeService;
		private readonly ISubjectService subjectService;

		public GradeController(UserManager<ApplicationUser> _userManager,
			ITeacherService _teacherService,
			IStudentService _studentService,
			IClassService _classService,
			IGradeService _gradeService,
			ISubjectService _subjectService)
		{
			userManager = _userManager;
			teacherService = _teacherService;
			studentService = _studentService;
			classService = _classService;
			gradeService = _gradeService;
			subjectService = _subjectService;
		}
		/// <summary>
		/// Adds a grade a given student from the given subject's teacher
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			try
			{
				if (!(await teacherService.TeacherHasASubjectAsync(User.Id())))
				{
					TempData[UserMessagesConstants.ERROR_MESSAGE] = "You don't have a assigned subject";

					return RedirectToAction("Index", "Home", new { area = "Teacher" });
				}

				var students = await studentService.GetAllStudentsAsync();

				ICollection<Models.Grade.StudentViewModel> studentViewModels = new List<Models.Grade.StudentViewModel>();

				foreach (var student in students)
				{
					studentViewModels.Add(new Models.Grade.StudentViewModel()
					{
						Id = student.Id,
						User = student.User,
						UserId = student.Userid,
						Class = student.Class,
						ClassId = student.ClassId
					});
				}

				var model = new AddGradeViewModel()
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
		public async Task<IActionResult> Add(AddGradeViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}
				var student = await studentService.GetStudentByIdAsync(model.StudentId);

				var teacher = await teacherService.GetTeacherByUserIdAsync(User.Id());

				if (!(await studentService.StudentHasASubjectAsync(student, teacher.Subject.Name)))
				{
					TempData[UserMessagesConstants.ERROR_MESSAGE] = "Student doesn't have such subject";

					return RedirectToAction("Index", "Home", new { area = "Teacher" });
				}

				var subject = await subjectService.GetSubjectByNameAsync(teacher.Subject.Name);

				await gradeService.AddGradeAsync(student.Id, subject.Id, model.Type);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Grade added successfully";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (Exception)
			{

				throw;
			}
			
		}

		/// <summary>
		/// Gets the information about the given grade
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GradeInformation(int id)
		{
			try
			{
				var grade = await gradeService.GetGradeByIdAsync(id);

				var model = new GradeInformationViewModel()
				{
					Id = grade.Id,
					Grade = grade.Type,
					FirstName = grade.Student?.User.FirstName,
					MiddleName = grade.Student?.User.MiddleName,
					LastName = grade.Student?.User.LastName,
					SubjectName = grade.Subject!.Name!,
					ClassName = grade.Student!.Class.Name
				};

				return View(model);
			}
			catch (Exception)
			{

				throw;
			}
		
		}

		/// <summary>
		/// Sets a given grade to not active
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await gradeService.DeleteGradeByIdAsync(id);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Grade has been deleted successfuly";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (Exception)
			{

				throw;
			}
			
		}

		/// <summary>
		/// Edits a given grade
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var grade = await gradeService.GetGradeByIdAsync(id);

				var model = new EditGradeViewModel()
				{
					Id = grade.Id,
					Grade = grade.Type
				};

				return View(model);
			}
			catch (Exception)
			{

				throw;
			}

		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditGradeViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}
				await gradeService.EditGradeByIdAsync(model.Id, model.Grade);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Grade has been edited successfuly";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (Exception)
			{

				throw;
			}
			
		}
	}
}

