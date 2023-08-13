namespace Classware.Areas.Student.Models.Remark
{
	public class RemarkViewModel
	{
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }
    }
}
