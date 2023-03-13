using Classware.Areas.Teacher.Models.Class;
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
			teacherService= _teacherService;	
        }
		/// <summary>
		/// Gets all the classes
		/// </summary>
		/// <returns></returns>
        [HttpGet]
		public async Task<IActionResult> All()
		{
			try
			{
				ICollection<AllClassesViewModel> models = new List<AllClassesViewModel>();

				var classes = await classService.GetAllClassesAsync();

				foreach (var _class in classes)
				{
					models.Add(new AllClassesViewModel()
					{
						Id = _class.Id,
						Name = _class.Name,
						StudentsCount = _class.Students.Count
					});
				}

				return View(models);
			}
			catch (Exception)
			{

				throw;
			}
		}
		/// <summary>
		/// Gets all of the given class' students
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpGet]
		public async Task<IActionResult> ClassStudents(int id)
		{
			try
			{
				var teacher = await teacherService.GetTeacherByUserIdAsync(User.Id());

				var students = await studentService.GetStudentsByClassIdAndSubjectName(id, teacher.Subject?.Name ?? "");

				ICollection<StudentViewModel>? studentViewModels = new List<StudentViewModel>();


				foreach (var student in students)
				{
					ICollection<StudentSubjectGradesViewModel>? studentSubjectGradesViewModels = new List<StudentSubjectGradesViewModel>();

					var grades = student.Grades
						.Where(s => s.IsActive)
						.ToList();

					foreach (var grade in grades)
					{
						studentSubjectGradesViewModels.Add(new StudentSubjectGradesViewModel()
						{
							Id = grade.Id,
							Type = grade.Type
						});
					}
					studentViewModels.Add(new StudentViewModel()
					{
						Id = student.Id,
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
			catch (Exception)
			{

				throw;
			}
		
		}

	}
}
