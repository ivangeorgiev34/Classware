using Classware.Areas.Teacher.Models.Remark;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Claims;
using System.Security.Policy;

namespace Classware.Areas.Teacher.Controllers
{
	public class RemarkController : BaseController
	{
		private readonly ITeacherService teacherService;
		private readonly IStudentService studentService;
		private readonly IRemarkService remarkService;
		public RemarkController(ITeacherService _teacherService,
			IStudentService _studentService,
			IRemarkService _remarkService)
		{
			teacherService = _teacherService;
			studentService = _studentService;
			remarkService = _remarkService;
		}
		/// <summary>
		/// Adds a remark to a given student
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		public async Task<IActionResult> Add()
		{

			if (!await teacherService.TeacherHasASubjectAsync(User.Id()))
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = "You don't have a assigned subject";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}

			var students = await studentService.GetAllStudentsAsync();

			ICollection<StudentViewModel> studentViewModels = new List<StudentViewModel>();

			var teacherUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			var teacher = await teacherService.GetTeacherByUserIdAsync(teacherUserId!);

			foreach (var student in students)
			{
				if (student.StudentSubjects.Any(ss => ss.Subject?.Name == teacher.Subject?.Name))
				{
					studentViewModels.Add(new StudentViewModel()
					{
						Id = student.Id.ToString(),
						ClassId = student.ClassId.ToString(),
						Class = student.Class,
						User = student.User,
						UserId = student.UserId
					});
				}
			}

			var model = new AddRemarkViewModel()
			{
				Students = studentViewModels
			};

			return View(model);


		}

		[HttpPost]
		public async Task<IActionResult> Add(AddRemarkViewModel model)
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

				await remarkService.AddRemarkAsync(student.Id.ToString(), teacher.Id.ToString(), teacher.Subject.Id.ToString(), model.Title, model.Description ?? null);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Remark added successfully";

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
		/// Gets all remarks for the given student
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> All(string id)
		{
			try
			{
				var teacherUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				var teacher = await teacherService
					.GetTeacherByUserIdAsync(teacherUserId!);

				if (await studentService.StudentExistsByUserId(id) == false)
				{
					return BadRequest();
				}

				var student = await studentService.GetStudentByIdAsync(id);

				if (await studentService.StudentHasASubjectAsync(student, teacher.Subject.Name) == false)
				{
					return BadRequest();
				}

				ICollection<RemarkViewModel> remarkViewModels = new List<RemarkViewModel>();

				var remarks = student.Remarks
					.Where(r => r.IsActive == true && r.Teacher.UserId == teacherUserId)
					.ToList();

				foreach (var remark in remarks)
				{
					remarkViewModels.Add(new RemarkViewModel
					{
						Id = remark.Id.ToString(),
						Description = remark.Description,
						Title = remark.Title
					});
				}

				var model = new AllRemarksViewModel()
				{
					FirstName = student.User.FirstName,
					MiddleName = student.User.MiddleName,
					LastName = student.User.LastName,
					ClassName = student.Class.Name,
					StudentId = id,
					Remarks = remarkViewModels
				};

				return View(model);
			}
			catch (InvalidOperationException e)
			{

				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}

		}
		/// <summary>
		/// Sets a given remark to not active
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			try
			{
				await remarkService.DeleteRemarkByIdAsync(id);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Remark removed successfully";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (NullReferenceException e)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}

		}
		/// <summary>
		/// Edits a given remark
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			try
			{
				var remark = await remarkService.GetRemarkByIdAsync(id);

				var model = new EditRemarkViewModel()
				{
					Id = remark.Id.ToString(),
					Title = remark.Title,
					Description = remark.Description
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
		public async Task<IActionResult> Edit(EditRemarkViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			await remarkService.EditRemarkByIdAsync(model.Id, model.Title, model.Description ?? null);

			TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Remark edited successfully";

			return RedirectToAction("Index", "Home", new { area = "Teacher" });
		}
	}
}
