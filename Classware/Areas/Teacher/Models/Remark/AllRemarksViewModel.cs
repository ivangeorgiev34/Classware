namespace Classware.Areas.Teacher.Models.Remark
{
	public class AllRemarksViewModel
	{
        public string? FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string? LastName { get; set; }

		public string? ClassName { get; set; }

		public int StudentId { get; set; }

        public ICollection<RemarkViewModel>? Remarks { get; set; }
    }
}
