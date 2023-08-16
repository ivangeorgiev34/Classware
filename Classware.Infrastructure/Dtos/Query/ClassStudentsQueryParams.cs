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

        public string? SearchOption { get; set; }

		public string? SearchValue { get; set; }
	}
}
