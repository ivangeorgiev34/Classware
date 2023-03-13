using Classware.Infrastructure.Constants;
using System.ComponentModel.DataAnnotations;

namespace Classware.Areas.Administrator.Models.Student
{
	public class ClassViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public bool IsActive { get; set; }

	}
}
