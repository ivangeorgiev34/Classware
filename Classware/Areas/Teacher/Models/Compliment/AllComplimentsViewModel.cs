using Classware.Areas.Teacher.Models.Remark;

namespace Classware.Areas.Teacher.Models.Compliment
{
	public class AllComplimentsViewModel
	{
		public string? FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string? LastName { get; set; }

		public string? ClassName { get; set; }

		public int StudentId { get; set; }

		public ICollection<ComplimentViewModel>? Compliments { get; set; }
	}
}
