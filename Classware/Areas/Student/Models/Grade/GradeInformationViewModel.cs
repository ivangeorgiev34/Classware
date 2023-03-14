namespace Classware.Areas.Student.Models.Grade
{
	public class GradeInformationViewModel
	{
		public int Id { get; set; }

		public int Grade { get; set; }

		public string Teacher { get; set; } = null!;

        public string SubjectName { get; set; } = null!;

		public string? FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string? LastName { get; set; }
	}
}
