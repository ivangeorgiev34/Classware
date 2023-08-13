using Classware.Areas.Teacher.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Classware.Areas.Teacher.Models.Remark
{
	public class AddRemarkViewModel
	{
        public string StudentId { get; set; } = null!;

        public ICollection<StudentViewModel>? Students { get; set; }

		[Required(ErrorMessage = "Title is required")]
		[StringLength(TeacherConstants.AddRemarkViewModel.TITLE_MAX_LENGTH,MinimumLength = TeacherConstants.AddRemarkViewModel.TITLE_MIN_LENGTH, ErrorMessage ="Title must be between 10 and 30 symbols")]
		public string Title { get; set; } = null!;

		[StringLength(TeacherConstants.AddRemarkViewModel.DESCRIPTION_MAX_LENGTH,MinimumLength =TeacherConstants.AddRemarkViewModel.DESCRIPTION_MIN_LENGTH,ErrorMessage ="Description length must be no more than 100 symbols")]
		public string? Description { get; set; }
	}
}
