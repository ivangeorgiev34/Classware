using Classware.Areas.Student.Models.Remark;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Student.Controllers
{
	public class RemarkController : BaseController
	{
		private readonly IStudentService studentService;
		private readonly IRemarkService remarkService;

		public RemarkController(IStudentService _studentService,
			IRemarkService _remarkService)
		{
			studentService = _studentService;
			remarkService = _remarkService;
		}
		[HttpGet]
		public async Task<IActionResult> All([FromQuery] int page = 1)
		{
			try
			{
				var student = await studentService.GetStudentByUserIdAsync(User.Id());

				var remarks = student.Remarks
					.Where(r => r.IsActive == true)
					.ToList();

				ICollection<RemarkViewModel> remarkViewModels = new List<RemarkViewModel>();

				var paginatedRemarks = remarks
					.Skip((page - 1) * 4)
					.Take(4)
					.ToList();

				foreach (var remark in paginatedRemarks)
				{
					remarkViewModels.Add(new RemarkViewModel
					{
						Id = remark.Id,
						Title = remark.Title,
						Description = remark.Description
					});
				}

				var model = new AllRemarksViewModel()
				{
					TotalRemarks = remarks.Count,
					Page = page,
					Remarks = remarkViewModels
				};

				return View(model);
			}
			catch (NullReferenceException e)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("All", "Remark", new { area = "Student" });
			}
		}

		[HttpGet]
		public async Task<IActionResult> RemarkInformation(string id)
		{
			try
			{
				var remark = await remarkService.GetRemarkByIdAsync(id);

				var student = await studentService.GetStudentByUserIdAsync(User.Id());

				if (remark.StudentId != student.Id)
				{
					return BadRequest();
				}

				var model = new RemarkInformationViewModel()
				{
					FirstName = student.User.FirstName,
					MiddleName = student.User.MiddleName,
					LastName = student.User.LastName,
					Title = remark.Title,
					Description = remark.Description,
					SubjectName = remark.Subject?.Name ?? null,
					TeacherName = $"{remark.Teacher.User.FirstName} {remark.Teacher.User.MiddleName} {remark.Teacher.User.LastName}"
				};

				return View(model);
			}
			catch (NullReferenceException e)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("All", "Remark", new { area = "Student" });
			}
		}
	}
}
