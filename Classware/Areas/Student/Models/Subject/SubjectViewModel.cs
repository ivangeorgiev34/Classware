using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace Classware.Areas.Student.Models.Subject
{
	public class SubjectViewModel
	{
        public string Id { get; set; } = null!;

		public string Name { get; set; } = null!;

        public ICollection<GradeViewModel>? Grades { get; set; }
    }
}
