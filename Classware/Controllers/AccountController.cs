using Classware.Infrastructure.Models;
using Classware.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics;

namespace Classware.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(
            UserManager<ApplicationUser> _userManager,
            RoleManager<IdentityRole> _roleManager,
            SignInManager<ApplicationUser> _signInManager,
            ILogger<AccountController> _logger)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            signInManager = _signInManager;
            logger = _logger;
        }

        [Authorize(Roles ="Adminitrator")]
        public async Task<IActionResult> SeedRoles()
        {
            await CreateRoles();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult LogIn()
        {
            var model = new AccountLogInViewModel();

            return View(model);
        }

        /// <summary>
        /// Logs in the user and directs them to a controller and area depending on their role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogIn(AccountLogInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);


                if (result.Succeeded && await userManager.IsInRoleAsync(user,"Administrator"))
                {
                    return RedirectToAction("Index", "Home",new {area="Administrator"});
                }
                else if (result.Succeeded && await userManager.IsInRoleAsync(user, "Teacher"))
                {
					return RedirectToAction("Index", "Home", new { area = "Teacher" });
				}
                else if (result.Succeeded && await userManager.IsInRoleAsync(user, "Student"))
                {
					return RedirectToAction("Index", "Home", new { area = "Student" });
				}
                ModelState.AddModelError("", "Invalid credentials");

                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Creates a power user and administrator,student and teacher roles if needed
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
