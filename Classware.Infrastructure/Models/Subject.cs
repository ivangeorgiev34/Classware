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
			StudentSubjects = new List<StudentSubject>();
		}
		/// <summary>
		/// Subject's primary key
		/// </summary>
		[Key]
		public Guid Id { get; set; }

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
		/// Subject's students
		/// </summary>
		public ICollection<StudentSubject> StudentSubjects { get; set; }

	}
}
