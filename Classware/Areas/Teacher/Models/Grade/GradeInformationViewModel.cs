using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Classware.Areas.Teacher.Models.Grade
{
	public class GradeInformationViewModel
	{
		public string Id { get; set; } = null!;

        public int Grade { get; set; }

		public string ClassName { get; set; } = null!;

        public string SubjectName { get; set; } = null!;

		public string TeacherName { get; set; } = null!;

        public string? FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string? LastName { get; set; }

        public string TeacherId { get; set; }= null!;

		public string GradeTeacherId { get; set; } = null!;
	}
}
