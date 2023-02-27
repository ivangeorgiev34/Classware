using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(
			UserManager<ApplicationUser> _userManager,
			RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
			roleManager = _roleManager;
        }

        public IActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// creates a power user and administrator,student and teacher roles if needed
		/// </summary>
		/// <returns></returns>
		private async Task CreateRoles()
		{
			bool isAdministratorRoleCreated = await roleManager.RoleExistsAsync("Administrator");
			if (!isAdministratorRoleCreated)
			{
				//here we create the administrator role   
				var role = new IdentityRole();
				role.Name = "Administrator";
				await roleManager.CreateAsync(role);
                
				//creating a user who is administrator
				var user = new ApplicationUser();
				user.UserName = "admin";
				user.Email = "admin@abv.bg";
				string administratorPassword = "admin123";

				IdentityResult checkUser = await userManager.CreateAsync(user, administratorPassword);

				//adding administrator role to user   
				if (checkUser.Succeeded)
				{
					var admin = await userManager.AddToRoleAsync(user, "Administrator");
				}
			}

			//checking if the student role is already created   
			bool isStudentRoleCreated = await roleManager.RoleExistsAsync("Student");
			if (!isStudentRoleCreated)
			{
				var role = new IdentityRole();
				role.Name = "Student";
				await roleManager.CreateAsync(role);
			}

			//checking if the teacher role is already created    
			bool isTeacherRoleCreated = await roleManager.RoleExistsAsync("Teacher");
			if (!isTeacherRoleCreated)
			{
				var role = new IdentityRole();
				role.Name = "Teacher";
				await roleManager.CreateAsync(role);
			}
		}
	}
}
