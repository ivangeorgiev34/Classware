using Classware.Infrastructure.Models;

namespace Classware.Areas.Administrator.Models.Teacher
{
	public class TeacherViewModel
	{
        public string Id { get; set; } = null!;

		public string UserId { get; set; } = null!;

		public ApplicationUser User { get; set; } = null!;

		public bool IsActive { get; set; }

		public string? SubjectId { get; set; }
	}
}
