using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Infrastructure.Models
{
    public class Teacher
    {
        /// <summary>
        /// Teacher's primary key
        /// </summary>
		[Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Reference to asp.net users
        /// </summary>
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Is teacher active
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Teacher's subject
        /// </summary>
        [ForeignKey(nameof(Subject))]
        public Guid? SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}
