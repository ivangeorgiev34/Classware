using Classware.Areas.Teacher.Constants;
using System.ComponentModel.DataAnnotations;

namespace Classware.Areas.Teacher.Models.Compliment
{
	public class AddComplimentViewModel
	{
		public int StudentId { get; set; }

		public ICollection<StudentViewModel>? Students { get; set; }

		[Required(ErrorMessage = "Title is required")]
		[StringLength(TeacherConstants.AddComplimentViewModel.TITLE_MAX_LENGTH, MinimumLength = TeacherConstants.AddComplimentViewModel.TITLE_MIN_LENGTH, ErrorMessage = "Title must be between 10 and 30 symbols")]
		public string Title { get; set; } = null!;

		[StringLength(TeacherConstants.AddComplimentViewModel.DESCRIPTION_MAX_LENGTH, MinimumLength = TeacherConstants.AddComplimentViewModel.DESCRIPTION_MIN_LENGTH, ErrorMessage = "Description length must be no more than 100 symbols")]
		public string? Description { get; set; }
	}
}
