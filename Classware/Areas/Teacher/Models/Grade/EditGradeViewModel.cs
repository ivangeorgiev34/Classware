using Classware.Areas.Teacher.Constants;
using System.ComponentModel.DataAnnotations;

namespace Classware.Areas.Teacher.Models.Grade
{
	public class EditGradeViewModel
	{
        public string Id { get; set; } = null!;

		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Grade is required")]
		[Range(TeacherConstants.AddGradeViewModel.GRADE_TYPE_MIN_RANGE, TeacherConstants.AddGradeViewModel.GRADE_TYPE_MAX_RANGE, ErrorMessage = "Grade must be between 2 and 6")]
		public int Grade { get; set; }
    }
}
