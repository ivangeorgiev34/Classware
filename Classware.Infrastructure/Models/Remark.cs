using Classware.Infrastructure.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classware.Infrastructure.Models
{
    public class Remark
    {
        /// <summary>
        /// Remark's primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Remark's title
        /// </summary>
        [Required]
        [StringLength(InfrastructureConstants.Remark.REMARK_TITLE_MAX_LENGTH, MinimumLength = InfrastructureConstants.Remark.REMARK_TITLE_MIN_LENGTH)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Remark's description
        /// </summary>
        [StringLength(InfrastructureConstants.Remark.REMARK_DESCRIPTION_MAX_LENGTH)]
        public string? Description { get; set; }

        /// <summary>
        /// Is remark active
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;


        /// <summary>
        /// Remark's subject
        /// </summary>
		[ForeignKey(nameof(Subject))]
		public int SubjectId { get; set; }
		public Subject? Subject { get; set; }

        /// <summary>
        /// Remark to the given student
        /// </summary>
		[ForeignKey(nameof(Student))]
		public int StudentId { get; set; }
		public Student? Student { get; set; }


	}
}
