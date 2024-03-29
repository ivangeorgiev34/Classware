﻿using Classware.Areas.Teacher.Models.Compliment;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Classware.Areas.Teacher.Controllers
{
	public class ComplimentController : BaseController
	{
		private readonly IStudentService studentService;
		private readonly ITeacherService teacherService;
		private readonly IComplimentService complimentService;

		public ComplimentController(IStudentService _studentService,
			ITeacherService _teacherService,
			IComplimentService _complimentService)
		{
			studentService = _studentService;
			teacherService = _teacherService;
			complimentService = _complimentService;
		}

		/// <summary>
		/// Adds a compliment to a given student
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
					studentViewModels.Add(new StudentViewModel
					{
						Id = student.Id.ToString(),
						User = student.User,
						UserId = student.UserId,
						Class = student.Class,
						ClassId = student.ClassId.ToString()
					});
				}
			}

			var model = new AddComplimentViewModel()
			{
				Students = studentViewModels
			};

			return View(model);

		}

		[HttpPost]
		public async Task<IActionResult> Add(AddComplimentViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}

				var student = await studentService.GetStudentByIdAsync(model.StudentId);

				var teacher = await teacherService.GetTeacherByUserIdAsync(User.Id());

				if (!await studentService.StudentHasASubjectAsync(student, teacher.Subject.Name))
				{
					TempData[UserMessagesConstants.ERROR_MESSAGE] = "Student doesn't have such subject";

					return RedirectToAction("Index", "Home", new { area = "Teacher" });
				}

				await complimentService.AddComplimentAsync(student.Id.ToString(), teacher.Id.ToString(), teacher.Subject.Id.ToString(), model.Title, model.Description ?? null);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Compliment added successfully";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (InvalidOperationException e)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return View(model);
			}
			catch (NullReferenceException e)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
		}

		/// <summary>
		/// Gets all the compliments of a given student
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> All(string id, [FromQuery] int page = 1)
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

				var compliments = student.Compliments
					.Where(c => c.IsActive)
					.ToList();

				ICollection<ComplimentViewModel> complimentViewModels = new List<ComplimentViewModel>();

				var paginatedCompliments = compliments
					.Skip((page - 1) * 4)
					.Take(4)
					.ToList();

				foreach (var compliment in paginatedCompliments)
				{
					complimentViewModels.Add(new ComplimentViewModel()
					{
						Id = compliment.Id.ToString(),
						Title = compliment.Title,
						Description = compliment.Description,
						ComplimentTeacherId = compliment.TeacherId.ToString()
					});
				}

				var model = new AllComplimentsViewModel()
				{
					FirstName = student.User.FirstName,
					MiddleName = student.User.MiddleName,
					LastName = student.User.LastName,
					ClassName = student.Class.Name,
					StudentId = id,
					TeacherId = teacher.Id.ToString(),
					TotalCompliments = compliments.Count,
					Page = page,
					Compliments = complimentViewModels
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
		/// Sets a given compliment to not active
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			try
			{
				var teacher = await teacherService
					.GetTeacherByUserIdAsync(User.Id());

				var compliment = await complimentService.GetComplimentByIdAsync(id);

				if (compliment.TeacherId != teacher.Id)
				{
					return BadRequest();
				}

				await complimentService.DeleteComplimentByIdAsync(id);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Compliment deleted successfully";

				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
			catch (NullReferenceException e)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}

		}

		/// <summary>
		/// Edits a given compliment
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			try
			{
				var teacher = await teacherService
					.GetTeacherByUserIdAsync(User.Id());

				var compliment = await complimentService.GetComplimentByIdAsync(id);

				if (compliment.TeacherId != teacher.Id)
				{
					return BadRequest();
				}

				var model = new EditComplimentViewModel()
				{
					Id = compliment.Id.ToString(),
					Title = compliment.Title,
					Description = compliment.Description
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
		public async Task<IActionResult> Edit(EditComplimentViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}

				await complimentService.EditComplimentByIdAsync(model.Id, model.Title, model.Description);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Compliment edited successfully";

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
