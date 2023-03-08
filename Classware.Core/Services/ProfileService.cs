using Classware.Core.Contracts;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Core.Services
{
	public class ProfileService : IProfileService
	{
		private readonly IRepository repo;
		private readonly UserManager<ApplicationUser> userManager;

        public ProfileService(UserManager<ApplicationUser> _userManager,
			IRepository _repo)
        {
            userManager = _userManager;
            repo = _repo;
        }

		/// <summary>
		/// Edits user's infomation
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="data"></param>
		/// <param name="firstName"></param>
		/// <param name="middleName"></param>
		/// <param name="lastName"></param>
		/// <param name="age"></param>
		/// <param name="gender"></param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		public async Task EditProfileInformationAsync(string userId, byte[] data, string? firstName, string? middleName, string? lastName, int? age, string? gender)
		{
			var user = await userManager.FindByIdAsync(userId);

			if (user == null)
			{
				throw new NullReferenceException("User doesn't exist");
			}

			user.ProfilePicture = data;
			user.FirstName = firstName;
			user.MiddleName = middleName;
			user.LastName = lastName;
			user.Age = age;
			user.Gender = gender;

			await repo.SaveChangesAsync();
		}

		public async Task UploadPictureAsync(byte[] data,string userId)
		{
            var user = await userManager.FindByIdAsync(userId);

            user.ProfilePicture = data;

            await repo.SaveChangesAsync();
		}
	}
}
