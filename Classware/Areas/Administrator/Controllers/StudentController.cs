using Classware.Areas.Administrator.Models.Student;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.Data;

namespace Classware.Areas.Administrator.Controllers
{
	public class StudentController : BaseController
	{

		private readonly UserManager<ApplicationUser> userManager;
		private readonly IClassService classService;
		private readonly IStudentService studentService;
		private readonly ISubjectService subjectService;
		private readonly ILogger<StudentController> logger;


		public StudentController(UserManager<ApplicationUser> _userManager,
			IClassService _classService,
			IStudentService _studentService,
			ISubjectService _subjectService,
			ILogger<StudentController> _logger)
		{
			userManager = _userManager;
			classService = _classService;
			studentService = _studentService;
			subjectService = _subjectService;
			logger = _logger;
		}

		/// <summary>
		/// Gets all students
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> All()
		{
				List<AllStudentsViewModel> models = new List<AllStudentsViewModel>();

				var students = await studentService.GetAllStudentsAsync();

				foreach (var student in students)
				{
				string? image = null;
				if (student.User.ProfilePicture != null)
				{
					image = Convert.ToBase64String(student.User.ProfilePicture);
				}
				models.Add(new AllStudentsViewModel()
					{
						Id = student.Id,
						FirstName = student.User.FirstName,
						MiddleName = student.User.MiddleName,
						LastName = student.User.LastName,
						Age = student.User.Age,
						Gender = student.User.Gender,
						ProfilePicture = image,
						Class = student.Class.Name,
						Username = student.User.UserName
					});
				}

				return View(models);
			
		}

		/// <summary>
		/// Adds a student
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Add()
		{
				var classes = await classService.GetAllClassesAsync();

				var classViewModels = new List<ClassViewModel>();

				foreach (var _class in classes)
				{
					classViewModels.Add(new ClassViewModel()
					{
						Id = _class.Id,
						Name = _class.Name,
						IsActive = _class.IsActive
					});
				}

				var model = new AddStudentViewModel()
				{
					Classes = classViewModels
				};

				return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddStudentViewModel model)
		{
				if (!ModelState.IsValid)
				{
					return View(model);
				}

				if (await userManager.FindByNameAsync(model.Username) != null)
				{
					ModelState.AddModelError("", "Student with such username already exists");

					return View(model);
				}

				if (await userManager.FindByEmailAsync(model.Email) != null)
				{
					ModelState.AddModelError("", "Student with such email already exists");

					return View(model);
				}

				var user = new ApplicationUser()
				{
					FirstName = model.FirstName,
					MiddleName = model.MiddleName,
					LastName = model.LastName,
					Age = model.Age,
					Gender = model.Gender,
					UserName = model.Username,
					Email = model.Email
				};

				var result = await userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					var userRole = await userManager.FindByEmailAsync(user.Email);

					await userManager.AddToRoleAsync(userRole, "Student");

					var student = new Classware.Infrastructure.Models.Student()
					{
						Userid = userRole.Id,
						ClassId = model.ClassId
					};

					await studentService.AddStudentAsync(student);

					logger.LogInformation("Student with email:{0} name:{1} {2} {3},who is {4} years of age and a {5} has been added", model.Email, model.FirstName, model.MiddleName, model.LastName, model.Age, model.Gender);

					TempData[UserMessagesConstants.SUCCESS_MESSAGE] = $"Student {model.FirstName} {model.LastName} added successfully";

					return RedirectToAction("Index", "Home", new { area = "Administrator" });
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

				return View(model);
		}

		/// <summary>
		/// Sets a given student to not active
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{

				var student = await studentService.GetStudentByIdAsync(id);

				await studentService.DeleteStudentByIdAsync(id);

				logger.LogInformation("Student with email:{0} name:{1} {2} {3},who is {4} years of age and a {5} has been added", student.User.Email, student.User.FirstName, student.User.MiddleName, student.User.LastName, student.User.Age, student.User.Gender);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Student deleted successfully";

				return RedirectToAction("All", "Student", new { area = "Administrator" });
			

		}

		/// <summary>
		/// Assigns subjects to a student
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		public async Task<IActionResult> AssignSubjects()
		{

				var students = await studentService.GetAllStudentsAsync();

				var subjects = await subjectService.GetAllSubjectsAsync();

				var studentViewModels = new List<StudentViewModel>();

				var subjectViewModels = new List<SubjectViewModel>();

				foreach (var student in students)
				{
					studentViewModels.Add(new StudentViewModel()
					{
						Id = student.Id,
						User = student.User,
						UserId = student.Userid
					});
				}

				foreach (var subject in subjects)
				{
					subjectViewModels.Add(new SubjectViewModel()
					{
						Id = subject.Id,
						Name = subject.Name
					});
				}

				var model = new AssignSubjectsToStudentViewModel()
				{
					Subjects = subjectViewModels,
					Students = studentViewModels
				};

				return View(model);
			
			
			
		}

		[HttpPost]
		public async Task<IActionResult> AssignSubjects(AssignSubjectsToStudentViewModel model)
		{
			try
			{
				await studentService.AssignSubjectsAsync(model.SubjectsId, model.StudentId);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = $"Subjects assigned successfully";

				return RedirectToAction("Index", "Home", new { area = "Administrator" });
			}
			catch (NullReferenceException e)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return View(model);
			}

		}
	}
}
