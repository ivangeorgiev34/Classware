using Classware.Areas.Administrator.Models.Teacher;

namespace Classware.Areas.Administrator.Models.Student
{
	public class AssignSubjectsToStudentViewModel
	{
		public string StudentId { get; set; } = null!;

		public ICollection<StudentViewModel>? Students { get; set; }

		public ICollection<string>? SubjectsId { get; set; }

		public ICollection<SubjectViewModel>? Subjects { get; set; }
	}
}
