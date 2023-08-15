namespace Classware.Areas.Teacher.Models.Class
{
	public class ClassStudentsViewModel
	{
        public string Id { get; set; }
        public int TotalProperties { get; set; }

        public int Page { get; set; }

        public ICollection<StudentViewModel>? Students { get; set; }
    }
}
