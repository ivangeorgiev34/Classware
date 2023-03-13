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
    public class Compliment
    {
        /// <summary>
        /// Compliment's primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Compliment's title
        /// </summary>
        [Required]
        [StringLength(InfrastructureConstants.Compliment.COMPLIMENT_TITLE_MAX_LENGTH, MinimumLength = InfrastructureConstants.Compliment.COMPLIMENT_TITLE_MIN_LENGTH)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Compliment's description
        /// </summary>
        [StringLength(InfrastructureConstants.Compliment.COMPLIMENT_DESCRIPTION_MAX_LENGTH)]
        public string? Description { get; set; }

        /// <summary>
        /// Is compliment active
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

		/// <summary>
		/// Compliment's subject
		/// </summary>
		[ForeignKey(nameof(Subject))]
		public int SubjectId { get; set; }
		public Subject? Subject { get; set; }

		/// <summary>
		/// Compliment to the given student
		/// </summary>
		[ForeignKey(nameof(Student))]
		public int StudentId { get; set; }
		public Student? Student { get; set; }
	}
}
