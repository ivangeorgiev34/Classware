using System.ComponentModel.DataAnnotations;

namespace Classware.Areas.Administrator.Models.Teacher
{
	public class AddTeacherViewModel
	{
		[Required(ErrorMessage ="First name is required")]
        public string FirstName { get; set; } = null!;

		[Required(ErrorMessage = "Middle name is required")]
		public string MiddleName { get; set; } = null!;

		[Required(ErrorMessage = "Last name is required")]
		public string LastName { get; set; } = null!;

		[Required(ErrorMessage = "Age is required")]
		public int Age { get; set; }

		[Required(ErrorMessage = "Gender is required")]
		public string Gender { get; set; } = null!;

		[Required(ErrorMessage = "Username is required")]
		public string Username { get; set; } = null!;

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = null!;

		[Required(ErrorMessage = "Password is required")]
		[Compare(nameof(ConfirmPassword),ErrorMessage ="Passwords don't match")]
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;

		[Required(ErrorMessage = "Repeat password is required")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; } = null!;
	}
}
