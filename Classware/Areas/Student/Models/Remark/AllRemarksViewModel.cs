namespace Classware.Areas.Student.Models.Remark
{
	public class AllRemarksViewModel
	{
        public int TotalRemarks { get; set; }

        public int Page { get; set; }

        public ICollection<RemarkViewModel>? Remarks { get; set; }
    }
}
