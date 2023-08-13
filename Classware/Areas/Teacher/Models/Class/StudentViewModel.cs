namespace Classware.Areas.Teacher.Models.Class
{
	public class StudentViewModel
	{
        public string Id { get; set; } = null!;

        public string? ProfilePicture { get; set; }

		public string? FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string? LastName { get; set; }

		public int? Age { get; set; }

		public string? Gender { get; set; }

        public ICollection<StudentSubjectGradesViewModel>? Grades { get; set; }
    }
}
