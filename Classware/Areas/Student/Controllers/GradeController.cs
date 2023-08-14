using Classware.Areas.Student.Models.Grade;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Student.Controllers
{
	public class GradeController : BaseController
	{
		private readonly IStudentService studentService;
		private readonly IGradeService gradeService;
		private readonly ITeacherService teacherService;

		public GradeController(IStudentService _studentService,
			 IGradeService _gradeService,
			 ITeacherService _teacherService)
        {
            studentService = _studentService;
			gradeService = _gradeService;
			teacherService = _teacherService;
        }

        [HttpGet]
		public async Task<IActionResult> GradeInformation(string id)
		{
			try
			{
				var grade = await gradeService.GetGradeByIdAsync(id);

				var student = await studentService.GetStudentByUserIdAsync(User.Id());

				if (grade.StudentId != student.Id)
				{
					return BadRequest();
				}

				var model = new GradeInformationViewModel()
				{
					Grade = grade.Type,
					FirstName = student.User.FirstName,
					MiddleName = student.User.MiddleName,
					LastName = student.User.LastName,
					SubjectName = grade.Subject.Name,
					Teacher = $"{grade.Teacher.User.FirstName} {grade.Teacher.User.MiddleName} {grade.Teacher.User.LastName}"
				};

				return View(model);
			}
			catch (NullReferenceException e)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("All", "Subject", new { area = "Student" });
			}
		}
	}
}
