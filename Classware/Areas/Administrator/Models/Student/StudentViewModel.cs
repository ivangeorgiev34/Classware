using Classware.Infrastructure.Models;

namespace Classware.Areas.Administrator.Models.Student
{
	public class StudentViewModel
	{
		public string Id { get; set; } = null!;

		public string UserId { get; set; } = null!;

		public ApplicationUser User { get; set; } = null!;

	}

}

