using Classware.Infrastructure.Models;

namespace Classware.Areas.Administrator.Models.Teacher
{
	public class TeacherViewModel
	{
        public int Id { get; set; }

		public string UserId { get; set; } = null!;

		public ApplicationUser User { get; set; } = null!;

		public bool IsActive { get; set; }

		public int? SubjectId { get; set; }
	}
}
