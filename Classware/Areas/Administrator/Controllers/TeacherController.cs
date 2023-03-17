using Classware.Areas.Administrator.Models.Teacher;
using Classware.Controllers;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Classware.Areas.Administrator.Controllers
{
    public class TeacherController : BaseController
    {
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly ILogger<AccountController> logger;
		private readonly ITeacherService teacherService;
		private readonly IClassService classService;
		private readonly ISubjectService subjectService;

		public TeacherController(
			 UserManager<ApplicationUser> _userManager,
			 RoleManager<IdentityRole> _roleManager,
			 SignInManager<ApplicationUser> _signInManager,
			 ILogger<AccountController> _logger,
			 ITeacherService _teacherService,
			 IClassService _classService,
			 ISubjectService _subjectService)
		{
			userManager = _userManager;
			roleManager = _roleManager;
			signInManager = _signInManager;
			logger = _logger;
			teacherService = _teacherService;
			classService = _classService;
			subjectService= _subjectService;
		}
		
		/// <summary>
		/// Adds a teacher
		/// </summary>
		/// <returns></returns>

		[HttpGet]
        public IActionResult Add()
        {
			
				var model = new AddTeacherViewModel();

				return View(model);
		

        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTeacherViewModel model)
        {
				if (!ModelState.IsValid)
				{
					return View(model);
				}

				if (await userManager.FindByNameAsync(model.Username) != null)
				{
					ModelState.AddModelError("", "Teacher with such username already exists");

					return View(model);
				}

				if (await userManager.FindByEmailAsync(model.Email) != null)
				{
					ModelState.AddModelError("", "Teacher with such email already exists");

					return View(model);
				}

				var user = new ApplicationUser()
				{
					FirstName = model.FirstName,
					MiddleName = model.MiddleName,
					LastName = model.LastName,
					Gender = model.Gender,
					Age = model.Age,
					Email = model.Email,
					UserName = model.Username
				};

				var result = await userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					var userRole = await userManager.FindByEmailAsync(user.Email);

					await userManager.AddToRoleAsync(userRole, "Teacher");

					var teacher = new Classware.Infrastructure.Models.Teacher()
					{
						UserId = userRole.Id
					};

					await teacherService.AddTeacher(teacher);

					logger.LogInformation("Teacher with email:{0} name:{1} {2} {3},who is {4} years of age and a {5} has been added", model.Email, model.FirstName, model.MiddleName, model.LastName, model.Age, model.Gender);

					TempData[UserMessagesConstants.SUCCESS_MESSAGE] = $"Teacher {model.FirstName} {model.LastName} added successfully";

					return RedirectToAction("Index", "Home", new { area = "Administrator" });
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

				return View(model);
			
           
        }

		/// <summary>
		/// Assigns a given teacher a given subject
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		public async Task<IActionResult> AssignSubject()
		{
				var teachers = await teacherService.GetAllTeachersAsync();

				var teacherViewModels = new List<TeacherViewModel>();

				foreach (var teacher in teachers)
				{
					if (teacher.Subject == null)
					{
						teacherViewModels.Add(new TeacherViewModel()
						{
							Id = teacher.Id,
							UserId = teacher.UserId,
							User = teacher.User,
							SubjectId = teacher.SubjectId,
							IsActive = teacher.IsActive
						});
					}
				}

				var subjects = await subjectService.GetAllSubjectsAsync();

				var subjectViewModels = new List<SubjectViewModel>();

				foreach (var subject in subjects)
				{
					subjectViewModels.Add(new SubjectViewModel()
					{
						Id = subject.Id,
						Name = subject.Name,
						IsActive = subject.IsActive
					});
				}

				var model = new AssignTeachersViewModel()
				{
					Teachers = teacherViewModels,
					Subjects = subjectViewModels
				};

				return View(model);
			
		}

		[HttpPost]
		public async Task<IActionResult> AssignSubject(AssignTeachersViewModel model)
		{
			try
			{
				await teacherService.AssignSubjectToTeacherAsync(model.TeacherId, model.SubjectId);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = $"Subject has been assigned successfully";

				return RedirectToAction("Index", "Home", new { area = "Administrator" });
			}
			catch (InvalidOperationException e)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE]  = e.Message;
				return View(model);
			}
			
		}

		/// <summary>
		/// Gets all the teachers
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> All()
		{

				var teachers = await teacherService.GetAllTeachersAsync();

				ICollection<AllTeachersViewModel> models = new List<AllTeachersViewModel>();

				foreach (var teacher in teachers)
				{
					string? image = null;
					if (teacher.User.ProfilePicture != null)
					{
						image = Convert.ToBase64String(teacher.User.ProfilePicture);
					}

					models.Add(new AllTeachersViewModel()
					{
						Id = teacher.Id,
						ProfilePicture = image,
						FirstName = teacher.User.FirstName,
						MiddleName = teacher.User.MiddleName,
						LastName = teacher.User.LastName,
						Age = teacher.User.Age,
						Gender = teacher.User.Gender,
						Username = teacher.User.UserName,
						SubjectName = teacher.Subject?.Name

					});
				}

				return View(models);
		
		}

		/// <summary>
		/// Sets a teacher with the corresponding id to not active
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var teacher = await teacherService.GetTeacherByIdAsync(id);

				await teacherService.DeleteTeacherByIdAsync(id);

				logger.LogInformation("Teacher with email:{0} name:{1} {2} {3},who is {4} years of age and a {5} has been added", teacher.User.Email, teacher.User.FirstName, teacher.User.MiddleName, teacher.User.LastName, teacher.User.Age, teacher.User.Gender);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = $"Teacher {teacher.User.FirstName} {teacher.User.LastName} has been added successfully";

				return RedirectToAction("All", "Teacher", new { area = "Administrator" });
			}
			catch (InvalidOperationException e)
			{

				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("All", "Teacher", new { area = "Administrator" });
			}
			
		}

	}
}
