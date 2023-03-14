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
    public class Grade
    {
        /// <summary>
        /// Grade's primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Grade's type
        /// </summary>
        [Required]
        [Range(InfrastructureConstants.Grade.GRADE_TYPE_MIN_VALUE, InfrastructureConstants.Grade.GRADE_TYPE_MAX_VALUE)]
        public int Type { get; set; }

        /// <summary>
        /// Is grade active
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Grade's subject
        /// </summary>
        [ForeignKey(nameof(Subject))]
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        /// <summary>
        /// Grade to the given student
        /// </summary>
		[ForeignKey(nameof(Student))]
		public int StudentId { get; set; }
		public Student? Student { get; set; }

		/// <summary>
		/// Teacher who wrote the grade
		/// </summary>
		[ForeignKey(nameof(Teacher))]
		public int TeacherId { get; set; }
		public Teacher? Teacher { get; set; }

	}
}
