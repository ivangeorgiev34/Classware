using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Infrastructure.Models
{
	public class StudentSubject
	{
		[ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public Student? Student { get; set; }

		[ForeignKey(nameof(Subject))]
		public int SubjectId { get; set; }
		public Subject? Subject { get; set; }

	}
}
