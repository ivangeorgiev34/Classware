using Classware.Areas.Administrator.Models.Teacher;

namespace Classware.Areas.Administrator.Models.Student
{
	public class AssignSubjectsToStudentViewModel
	{
		public int StudentId { get; set; }

		public ICollection<StudentViewModel>? Students { get; set; }

		public ICollection<int>? SubjectsId { get; set; }

		public ICollection<SubjectViewModel>? Subjects { get; set; }
	}
}
