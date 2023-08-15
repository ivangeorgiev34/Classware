﻿using Classware.Areas.Teacher.Models.Class;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Teacher.Controllers
{
	public class ClassController : BaseController
	{
		private readonly ILogger<ClassController> logger;
		private readonly IClassService classService;
		private readonly IStudentService studentService;
		private readonly ITeacherService teacherService;

		public ClassController(
			ILogger<ClassController> _logger,
			IClassService _classService,
			IStudentService _studentService,
			ITeacherService _teacherService)
		{
			logger = _logger;
			classService = _classService;
			studentService = _studentService;
			teacherService = _teacherService;
		}
		/// <summary>
		/// Gets all the classes
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> All([FromQuery] int page = 1)
		{

			var teacher = await teacherService.GetTeacherByUserIdAsync(User.Id());

			if (teacher.Subject == null)
			{
				return BadRequest();
			}

			var model = new AllClassesViewModel();

			var classes = await classService.GetAllClassesAsync();

			foreach (var _class in classes.Skip((page - 1) * 4).Take(4).ToList())
			{
				model.Classes?.Add(new ClassViewModel()
				{
					Id = _class.Id.ToString(),
					Name = _class.Name,
					StudentsCount = _class.Students.Where(s => s.IsActive == true).Count(s => s.StudentSubjects
					.Select(ss => ss.Subject?.Name).Contains(teacher.Subject.Name))
				});
			}

			model.Page = page;
			model.TotalClasses = classes.Count();

			return View(model);
		}
		/// <summary>
		/// Gets all of the given class' students
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpGet]
		public async Task<IActionResult> ClassStudents(string id)
		{
			try
			{
				if (await classService.ClassExistsByIdAsync(id) == false)
				{
					return StatusCode(StatusCodes.Status400BadRequest);
				}

				var teacher = await teacherService.GetTeacherByUserIdAsync(User.Id());

				var students = await studentService.GetStudentsByClassIdAndSubjectName(id, teacher.Subject?.Name ?? "");

				ICollection<StudentViewModel>? studentViewModels = new List<StudentViewModel>();


				foreach (var student in students)
				{
					ICollection<StudentSubjectGradesViewModel>? studentSubjectGradesViewModels = new List<StudentSubjectGradesViewModel>();

					var grades = student.Grades
						.Where(s => s.IsActive && s.Subject?.Name == teacher.Subject?.Name)
						.ToList();

					foreach (var grade in grades)
					{
						studentSubjectGradesViewModels.Add(new StudentSubjectGradesViewModel()
						{
							Id = grade.Id.ToString(),
							Type = grade.Type
						});
					}
					studentViewModels.Add(new StudentViewModel()
					{
						Id = student.Id.ToString(),
						ProfilePicture = student.User.ProfilePicture == null ? null : Convert.ToBase64String(student.User.ProfilePicture),
						FirstName = student.User.FirstName,
						MiddleName = student.User.MiddleName,
						LastName = student.User.LastName,
						Age = student.User.Age,
						Gender = student.User.Gender,
						Grades = studentSubjectGradesViewModels
					});
				}

				var model = new ClassStudentsViewModel()
				{
					Students = studentViewModels
				};

				return View(model);

			}
			catch (NullReferenceException e)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("All");
			}

		}

	}
}
