namespace Classware.Areas.Student.Models.Compliment
{
	public class AllComplimentsViewModel
	{
        public int TotalCompliments { get; set; }

        public int Page { get; set; }

        public ICollection<ComplimentViewModel>? Compliments { get; set; }
    }
}
