using Classware.Areas.Teacher.Models.Remark;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
					studentViewModels.Add(new StudentViewModel()
					{
						Id = student.Id,
						ClassId = student.ClassId,
						Class = student.Class,
						User = student.User,
						UserId = student.Userid
					});
				}

				var model = new AddRemarkViewModel()
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

				await remarkService.AddRemarkAsync(student.Id,teacher.Id, teacher.Subject.Id, model.Title, model.Description ?? null);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Remark added successfully";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });

			}
			catch (Exception ex)
			{

				throw;
			}
		}

		/// <summary>
		/// Gets all remarks for the given student
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> All(int id)
		{
			try
			{
				var student = await studentService.GetStudentByIdAsync(id);

				ICollection<RemarkViewModel> remarkViewModels = new List<RemarkViewModel>();

				var remarks = student.Remarks
					.Where(r => r.IsActive == true)
					.ToList();

				foreach (var remark in remarks)
				{
					remarkViewModels.Add(new RemarkViewModel
					{
						Id = remark.Id,
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
			catch (Exception)
			{

				throw;
			}
		
		}
		/// <summary>
		/// Sets a given remark to not active
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await remarkService.DeleteRemarkByIdAsync(id);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Remark removed successfully";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (Exception)
			{

				throw;
			}
			
		}
		/// <summary>
		/// Edits a given remark
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var remark = await remarkService.GetRemarkByIdAsync(id);

				var model = new EditRemarkViewModel()
				{
					Id = remark.Id,
					Title = remark.Title,
					Description = remark.Description
				};

				return View(model);
			}
			catch (Exception)
			{

				throw;
			}
	
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditRemarkViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}
				await remarkService.EditRemarkByIdAsync(model.Id, model.Title, model.Description ?? null);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Remark edited successfully";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
