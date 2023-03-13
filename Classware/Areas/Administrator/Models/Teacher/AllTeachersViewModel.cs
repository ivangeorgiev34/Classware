using System.ComponentModel.DataAnnotations;

namespace Classware.Areas.Administrator.Models.Teacher
{
	public class AllTeachersViewModel
	{
        public int Id { get; set; }

        public string? ProfilePicture { get; set; }

        public string? FirstName { get; set; } 

		public string? MiddleName { get; set; } 

		public string? LastName { get; set; } 

		public int? Age { get; set; }

		public string? Gender { get; set; } 

		public string Username { get; set; } = null!;

		public string? SubjectName { get; set; }

	}
}
