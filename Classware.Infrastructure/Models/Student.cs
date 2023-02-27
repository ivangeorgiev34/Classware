using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Infrastructure.Models
{
	public class Student
	{
        public Student()
        {
			this.Subjects = new List<Subject>();
        }
        /// <summary>
        /// Student's primary key
        /// </summary>
        [Key]
		public int Id { get; set; }

        /// <summary>
        /// Is student active
        /// </summary>
        public bool IsActive { get; set; } = true;

		/// <summary>
		/// Reference to asp.net users
		/// </summary>
		[Required]
		[ForeignKey(nameof(User))]
		public int Userid { get; set; }
		public ApplicationUser User { get; set; } = null!;

		/// <summary>
		/// Reference to the class of the student
		/// </summary>
		[Required]
		[ForeignKey(nameof(Class))]
		public int ClassId { get; set; }
		public Class Class { get; set; } = null!;

		/// <summary>
		/// Student's subjects
		/// </summary>
        public ICollection<Subject> Subjects { get; set; }
    }
}
