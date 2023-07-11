using Classware.Areas.Student.Models.Profile;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Student.Controllers
{
	public class ProfileController : BaseController
	{
		private readonly IStudentService studentService;
		private readonly IProfileService profileService;

		public ProfileController(IStudentService _studentService,
			IProfileService _profileService)
        {
            studentService = _studentService;
			profileService = _profileService;	
        }
        [HttpGet]
		public async Task<IActionResult> ProfileInformation()
		{
			var student = await studentService.GetStudentByUserIdAsync(User.Id());

			var model = new ProfileInformationViewModel()
			{
				FirstName = student.User.FirstName,
				MiddleName = student.User.MiddleName,
				LastName = student.User.LastName,
				Age = student.User.Age,
				Gender = student.User.Gender,
				UploadedProfilePicture = student.User.ProfilePicture != null ? Convert.ToBase64String(student.User.ProfilePicture) : null,
				Username = student.User.UserName,
				Id = student.User.Id
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Upload(ProfileInformationViewModel model)
		{

			if (model.ProfilePicture == null)
			{
				TempData[UserMessagesConstants.ERROR_MESSAGE] = "Please upload a file before applying it to your profile";

				return RedirectToAction("ProfileInformation", "Profile", new { area = "Student" });
			}

			byte[] data = null;
			using (var ms = new MemoryStream())
			{
				await model.ProfilePicture?.CopyToAsync(ms);
				data = ms.ToArray();
			}

			await profileService.UploadPictureAsync(data, User.Id());

			TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Successfully uploaded a profile picture";

			return RedirectToAction("ProfileInformation", "Profile", new { area = "Student" });
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var student = await studentService.GetStudentByUserIdAsync(id);

			var model = new EditProfileViewModel()
			{
				FirstName = student.User.FirstName,
				MiddleName = student.User.MiddleName,
				LastName = student.User.LastName,
				Age = student.User.Age,
				Gender = student.User.Gender,
				UploadedProfilePicture = student.User.ProfilePicture != null ? Convert.ToBase64String(student.User.ProfilePicture) : null,
				Username = student.User.UserName,
				Id = student.User.Id
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditProfileViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}

				byte[]? data = null;

				if (model.ProfilePicture != null)
				{
					using (var ms = new MemoryStream())
					{
						await model.ProfilePicture!.CopyToAsync(ms);
						data = ms.ToArray();
					}
				}

				await profileService.EditProfileInformationAsync(User.Id(), data, model.FirstName, model.MiddleName, model.LastName, model.Age, model.Gender);

				TempData[UserMessagesConstants.SUCCESS_MESSAGE] = "Successfully edited your profile";

				return RedirectToAction("ProfileInformation", "Profile", new { area = "Student" });
			}
			catch (NullReferenceException e)
			{

				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("Index", "Home", new { area = "Student" });
			}
		}
	}
}
