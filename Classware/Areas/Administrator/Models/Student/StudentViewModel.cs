using Classware.Infrastructure.Models;

namespace Classware.Areas.Administrator.Models.Student
{
	public class StudentViewModel
	{
		public int Id { get; set; }

		public string UserId { get; set; } = null!;

		public ApplicationUser User { get; set; } = null!;

	}

}

