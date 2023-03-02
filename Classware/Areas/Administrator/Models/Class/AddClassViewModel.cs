using Classware.Areas.Administrator.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Classware.Areas.Administrator.Models.Class
{
    public class AddClassViewModel
    {
        [Required(ErrorMessage ="Field name is required!")]
        [StringLength(AdminAreaConstants.AddClassViewModel.CLASS_NAME_MAX_LENGTH, MinimumLength = AdminAreaConstants.AddClassViewModel.CLASS_NAME_MIN_LENGTH,ErrorMessage ="Name should be between 2 and 3 symbols!")]
        [RegularExpression(AdminAreaConstants.AddClassViewModel.CLASS_NAME_REGULAR_EXPRESSION,ErrorMessage = "Name should consist of 2 digits and 1 capital letter!")]
        public string Name { get; set; } = null!;
    }
}
