using Classware.Areas.Teacher.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Classware.Areas.Teacher.Models.Grade
{
	public class AddGradeViewModel
	{
        public int StudentId { get; set; }

        public ICollection<StudentViewModel>? Students { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage ="Grade is required")]
        [Range(TeacherConstants.AddGradeViewModel.GRADE_TYPE_MIN_RANGE,TeacherConstants.AddGradeViewModel.GRADE_TYPE_MAX_RANGE,ErrorMessage ="Grade must be between 2 and 6")]
        public int Type { get; set; }
    }
}
