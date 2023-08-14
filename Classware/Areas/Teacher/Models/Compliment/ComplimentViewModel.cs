namespace Classware.Areas.Teacher.Models.Compliment
{
	public class ComplimentViewModel
	{
		public string Id { get; set; } = null!;

		public string Title { get; set; } = null!;

		public string? Description { get; set; }

		public string ComplimentTeacherId { get; set; } = null!;
    }
}
