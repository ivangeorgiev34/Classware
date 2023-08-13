using Classware.Infrastructure.Models;

namespace Classware.Areas.Teacher.Models.Compliment
{
	public class StudentViewModel
	{
		public string Id { get; set; } = null!;

		public string? UserId { get; set; }
		public ApplicationUser? User { get; set; }

		public string? ClassId { get; set; }
		public Classware.Infrastructure.Models.Class? Class { get; set; }
	}
}
