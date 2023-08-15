using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Infrastructure.Dtos.Query
{
	public class ClassStudentsQueryParams
	{
		public int Page { get; set; } = 1;

        public string? FirstName { get; set; }

		public string? MiddleName { get; set; }

		public string? LastName { get; set; }
	}
}
