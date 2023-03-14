namespace Classware.Areas.Student.Models.Compliment
{
	public class ComplimentInformationViewModel
	{
		public string Title { get; set; } = null!;

		public string? Description { get; set; }

		public string? FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string? LastName { get; set; }

		public string? SubjectName { get; set; }

		public string TeacherName { get; set; } = null!;
	}
}
