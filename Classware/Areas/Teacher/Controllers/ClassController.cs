using Classware.Areas.Teacher.Models.Class;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Extensions;
using Classware.Infrastructure.Dtos.Query;
using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
		public async Task<IActionResult> All([FromQuery] string sortBy, string sortStyle, int page = 1)
		{

			var teacher = await teacherService.GetTeacherByUserIdAsync(User.Id());

			if (teacher.Subject == null)
			{
				return BadRequest();
			}

			var model = new AllClassesViewModel();

			var classes = await classService.GetAllClassesAsync();

			var paginatedClasses = new List<Class>();

			if (sortBy == "studentsCount")
			{
				if (sortStyle == "ascending")
				{
					paginatedClasses = classes
						.OrderBy(c => c.Students.Count)
						.Skip((page - 1) * 4)
						.Take(4)
						.ToList();
				}
				else if (sortStyle == "descending")
				{
					paginatedClasses = classes
						.OrderByDescending(c => c.Students.Count)
						.Skip((page - 1) * 4)
						.Take(4)
						.ToList();
				}
			}
			else
			{
				paginatedClasses = classes
						.Skip((page - 1) * 4)
						.Take(4)
						.ToList();
			}

			foreach (var _class in paginatedClasses)
			{
				model.Classes?.Add(new ClassViewModel()
				{
					Id = _class.Id.ToString(),
					Name = _class.Name,
					StudentsCount = _class.Students.Where(s => s.IsActive == true).Count(s => s.StudentSubjects
					.Select(ss => ss.Subject?.Name).Contains(teacher.Subject.Name))
				});
			}

			model.SortBy = sortBy;
			model.SortingStyle = sortStyle;
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
		public async Task<IActionResult> ClassStudents(string id, [FromQuery] ClassStudentsQueryParams studentsQueryParams)
		{
			try
			{
				if (await classService.ClassExistsByIdAsync(id) == false)
				{
					return StatusCode(StatusCodes.Status400BadRequest);
				}

				var teacher = await teacherService.GetTeacherByUserIdAsync(User.Id());

				var students = await studentService
						.GetStudentsByClassIdAndSubjectName(id, teacher.Subject?.Name ?? "");

				var filteredStudents = new List<Infrastructure.Models.Student>();

				if (studentsQueryParams.SearchOption == "firstName")
				{
					filteredStudents = students
						.Where(s => s.User.FirstName!.ToLower()
						.Contains(studentsQueryParams.SearchValue == null ? "" : studentsQueryParams.SearchValue.ToLower()))
						.ToList();
				}
				else if (studentsQueryParams.SearchOption == "middleName")
				{
					filteredStudents = students
						.Where(s => s.User.MiddleName!.ToLower()
						.Contains(studentsQueryParams.SearchValue == null ? "" : studentsQueryParams.SearchValue.ToLower()))
						.ToList();
				}
				else if (studentsQueryParams.SearchOption == "lastName")
				{
					filteredStudents = students
						.Where(s => s.User.LastName!.ToLower()
						.Contains(studentsQueryParams.SearchValue == null ? "" : studentsQueryParams.SearchValue.ToLower()))
						.ToList();
				}

				ICollection<StudentViewModel>? studentViewModels = new List<StudentViewModel>();

				var paginatedStudents = new List<Infrastructure.Models.Student>();

				if (filteredStudents.Count == 0)
				{
					paginatedStudents = students
				   .Skip((studentsQueryParams.Page - 1) * 4)
				   .Take(4)
				   .ToList();
				}
				else
				{
					paginatedStudents = filteredStudents
				   .Skip((studentsQueryParams.Page - 1) * 4)
				   .Take(4)
				   .ToList();
				}

				foreach (var student in paginatedStudents)
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
					Id = id,
					TotalProperties = filteredStudents.Count == 0 ? students.Count() : filteredStudents.Count,
					Page = studentsQueryParams.Page,
					SearchOption = filteredStudents.Count > 0 ? studentsQueryParams.SearchOption : null,
					SearchValue = filteredStudents.Count > 0 ? studentsQueryParams.SearchValue : null,
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
