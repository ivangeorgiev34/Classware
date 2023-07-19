using Classware.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Infrastructure.Models
{
	public class Message
	{
		[Key]
        public int Id { get; set; }

        [Required(ErrorMessage = InfrastructureConstants.Message.MESSAGE_TITLE_REQUIRED_ERROR_MESSAGE)]
		public string Title { get; set; } = null!;

		[Required(ErrorMessage = InfrastructureConstants.Message.MESSAGE_DESCRIPTION_REQUIRED_ERROR_MESSAGE)]
		public string Description { get; set; } = null!;

		[Required(ErrorMessage = InfrastructureConstants.Message.MESSAGE_FULL_NAME_REQUIRED_ERROR_MESSAGE)]
		public string FullName { get; set; } = null!;

		[Required(ErrorMessage = InfrastructureConstants.Message.MESSAGE_EMAIL_REQUIRED_ERROR_MESSAGE)]
		[EmailAddress(ErrorMessage = InfrastructureConstants.Message.MESSAGE_EMAIL_INVALID_ERROR_MESSAGE)]
		public string Email { get; set; } = null!;

		[Required(ErrorMessage = InfrastructureConstants.Message.MESSAGE_IS_ANSWERED_REQUIRED_ERROR_MESSAGE)]
		public bool IsAnswered { get; set; }

        [ForeignKey(nameof(User))]
		public string? UserId { get; set; }
		public ApplicationUser? User { get; set; }
	}
}
