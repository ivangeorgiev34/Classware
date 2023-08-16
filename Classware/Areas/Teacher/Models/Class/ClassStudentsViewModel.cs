namespace Classware.Areas.Teacher.Models.Class
{
	public class ClassStudentsViewModel
	{
		public string Id { get; set; } = null!;

		public int TotalProperties { get; set; }

		public int Page { get; set; }

		public string? SearchOption { get; set; }

		public string? SearchValue { get; set; }

		public ICollection<StudentViewModel>? Students { get; set; }
	}
}
