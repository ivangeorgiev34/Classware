using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Infrastructure.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// User's first name
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// User's middle name
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// User's Last name
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// User's age
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// User's gender
        /// </summary>
        public string? Gender { get; set; }

        /// <summary>
        /// User's profile picture 
        /// </summary>
        public byte[]? ProfilePicture { get; set; }

        /// <summary>
        /// Is user active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
