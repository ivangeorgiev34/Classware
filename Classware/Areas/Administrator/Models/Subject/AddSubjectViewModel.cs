using Classware.Areas.Administrator.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Classware.Areas.Administrator.Models.Subject
{
	public class AddSubjectViewModel
	{
		[Required(ErrorMessage = "Name is required!")]
		[StringLength(AdminAreaConstants.AddClassViewModel.SUBJECT_NAME_MAX_LENGTH)]
		public string Name { get; set; } = null!;
    }
}
