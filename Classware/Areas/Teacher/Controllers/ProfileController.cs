using Classware.Areas.Teacher.Models.Profile;
using Classware.Core.Constants;
using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Extensions;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace Classware.Areas.Teacher.Controllers
{
	public class ProfileController : BaseController
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IProfileService profileService;

        public ProfileController(UserManager<ApplicationUser> _userManager,
			IProfileService _profileService)
        {
            userManager = _userManager;
			profileService = _profileService;
        }

		/// <summary>
		/// Gets all the information about the profile of the user
		/// </summary>
		/// <returns></returns>

        [HttpGet]
		public async Task<IActionResult> ProfileInformation()
		{
				var currentUser = await userManager.GetUserAsync(User);

				var model = new ProfileInformationViewModel()
				{
					FirstName = currentUser.FirstName,
					MiddleName = currentUser.MiddleName,
					LastName = currentUser.LastName,
					Age = currentUser.Age,
					Gender = currentUser.Gender,
					UploadedProfilePicture = currentUser.ProfilePicture != null ? Convert.ToBase64String(currentUser.ProfilePicture) : null,
					Username = currentUser.UserName,
					Id = currentUser.Id
				};

				return View(model);		
		}

		/// <summary>
		/// Uploads a profile picture to the user
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Upload(ProfileInformationViewModel model)
		{

				
				if (model.ProfilePicture == null)
				{
					TempData[UserMessagesConstants.ERROR_MESSAGE] = "Please upload a file before applying it to your profile";

					return RedirectToAction("ProfileInformation", "Profile", new { area = "Teacher" });
				}

				byte[] data = null;
				using (var ms = new MemoryStream())
				{
					await model.ProfilePicture?.CopyToAsync(ms);
					data = ms.ToArray();
				}

				await profileService.UploadPictureAsync(data, User.Id());

				return RedirectToAction("ProfileInformation", "Profile", new { area = "Teacher" });

			
		}

		/// <summary>
		/// Edits and updates the user's information
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{

				var user = await userManager.FindByIdAsync(id);

				var model = new ProfileInformationViewModel()
				{
					FirstName = user.FirstName,
					MiddleName = user.MiddleName,
					LastName = user.LastName,
					Age = user.Age,
					Gender = user.Gender,
					UploadedProfilePicture = user.ProfilePicture != null ? Convert.ToBase64String(user.ProfilePicture) : null,
					Username = user.UserName,
					Id = user.Id
				};

				return View(model);



		}

		[HttpPost]
		public async Task<IActionResult> Edit(ProfileInformationViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}
				byte[] data = null;
				using (var ms = new MemoryStream())
				{
					await model.ProfilePicture?.CopyToAsync(ms);
					data = ms.ToArray();
				}

				await profileService.EditProfileInformationAsync(User.Id(), data, model.FirstName, model.MiddleName, model.LastName, model.Age, model.Gender);

				return RedirectToAction("ProfileInformation", "Profile", new { area = "Teacher" });
			}
			catch (NullReferenceException e)
			{

				TempData[UserMessagesConstants.ERROR_MESSAGE] = e.Message;
				return RedirectToAction("Index", "Home", new { area = "Teacher" });
			}
		
		}
	}
}
