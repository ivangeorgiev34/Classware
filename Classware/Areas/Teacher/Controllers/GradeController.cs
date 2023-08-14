
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
using System.Security.Claims;

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
			if (!(await teacherService.TeacherHasASubjectAsync(User.Id())))
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = "You don't have a assigned subject";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}

			var students = await studentService.GetAllStudentsAsync();

			ICollection<Models.Grade.StudentViewModel> studentViewModels = new List<Models.Grade.StudentViewModel>();

			var teacherUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			var teacher = await teacherService.GetTeacherByUserIdAsync(teacherUserId!);

			foreach (var student in students)
			{
				if (student.StudentSubjects.Any(ss => ss.Subject?.Name == teacher.Subject?.Name))
				{
					studentViewModels.Add(new Models.Grade.StudentViewModel()
					{
						Id = student.Id.ToString(),
						User = student.User,
						UserId = student.UserId,
						Class = student.Class,
						ClassId = student.ClassId.ToString()
					});
				}
			}

			var model = new AddGradeViewModel()
			{
				Students = studentViewModels
			};

			return View(model);


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

				await gradeService.AddGradeAsync(student.Id.ToString(), teacher.Id.ToString(), subject.Id.ToString(), model.Type);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Grade added successfully";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (InvalidOperationException e)
			{

				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (NullReferenceException e)
			{

				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}

		}

		/// <summary>
		/// Gets the information about the given grade
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GradeInformation(string id)
		{
			try
			{
				var grade = await gradeService.GetGradeByIdAsync(id);

				var teacher = await teacherService.GetTeacherByUserIdAsync(User.Id());

				if (teacher.SubjectId != grade.SubjectId)
				{
					return BadRequest();
				}

				var model = new GradeInformationViewModel()
				{
					Id = grade.Id.ToString(),
					Grade = grade.Type,
					FirstName = grade.Student?.User.FirstName,
					MiddleName = grade.Student?.User.MiddleName,
					LastName = grade.Student?.User.LastName,
					SubjectName = grade.Subject!.Name!,
					ClassName = grade.Student!.Class.Name,
					TeacherName = $"{teacher.User.FirstName} {teacher.User.MiddleName}  {teacher.User.LastName}",
					TeacherId = teacher.Id.ToString(),
					GradeTeacherId = grade.TeacherId.ToString(),
				};

				return View(model);
			}
			catch (NullReferenceException e)
			{

				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}

		}

		/// <summary>
		/// Sets a given grade to not active
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			try
			{
				await gradeService.DeleteGradeByIdAsync(id);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Grade has been deleted successfuly";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (NullReferenceException e)
			{

				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}

		}

		/// <summary>
		/// Edits a given grade
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			try
			{
				var grade = await gradeService.GetGradeByIdAsync(id);

				var model = new EditGradeViewModel()
				{
					Id = grade.Id.ToString(),
					Grade = grade.Type
				};

				return View(model);
			}
			catch (NullReferenceException e)
			{

				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("Index", "Home", new { area = "Teacher" });
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
			catch (NullReferenceException e)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}

		}
	}
}

