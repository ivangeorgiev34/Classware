using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace Classware.Areas.Student.Models.Subject
{
	public class SubjectViewModel
	{
        public int Id { get; set; }

		public string Name { get; set; } = null!;

        public ICollection<GradeViewModel>? Grades { get; set; }
    }
}
