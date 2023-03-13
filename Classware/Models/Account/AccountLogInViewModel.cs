using Classware.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Classware.Models.Account
{
    public class AccountLogInViewModel
    {
        [Required(ErrorMessage = AppConstants.AccountLogInViewModel.EMAIL_REQUIRED_ERROR_MESSAGE)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = AppConstants.AccountLogInViewModel.PASSWORD_REQUIRED_ERROR_MESSAGE)]
        [StringLength(AppConstants.AccountLogInViewModel.PASSWORD_MAX_LENGTH, MinimumLength = AppConstants.AccountLogInViewModel.PASSWORD_MIN_LENGTH)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
