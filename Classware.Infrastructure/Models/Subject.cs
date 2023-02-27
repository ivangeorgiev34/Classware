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
	public class Subject
	{
        public Subject()
        {
            this.Grades = new List<Grade>();
        }
        /// <summary>
        /// Subject's primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

		/// <summary>
		/// Subject's name
		/// </summary>
		[Required]
		[StringLength(InfrastructureConstants.Subject.SUBJECT_NAME_MAX_LENGTH)]
		public string Name { get; set; } = null!;

		/// <summary>
		/// Is subject active
		/// </summary>
		[Required]
		public bool IsActive { get; set; } = true;

		/// <summary>
		/// Grades of the specific subject
		/// </summary>
        public ICollection<Grade> Grades { get; set; }

		/// <summary>
		/// Remarks of the specific subject
		/// </summary>
		public ICollection<Remark> Remarks { get; set; }


		/// <summary>
		/// Compliments of the specific subject
		/// </summary>
		public ICollection<Compliment> Compliments { get; set; }

		/// <summary>
		/// Subject's teacher
		/// </summary>
		[Required]
		[ForeignKey(nameof(Teacher))]
		public int TeacherId { get; set; }
		public Teacher Teacher { get; set; } = null!;
	}
}
