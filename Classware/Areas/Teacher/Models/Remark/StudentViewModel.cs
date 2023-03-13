using Classware.Infrastructure.Models;

namespace Classware.Areas.Teacher.Models.Remark
{
	public class StudentViewModel
	{
        public int Id { get; set; }

		public string? UserId { get; set; }
		public ApplicationUser? User { get; set; }

		public int? ClassId { get; set; }
		public Classware.Infrastructure.Models.Class? Class { get; set; }
	}
}
