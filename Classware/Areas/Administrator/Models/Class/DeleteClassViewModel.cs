using Classware.Infrastructure.Models;

namespace Classware.Areas.Administrator.Models.Class
{
	public class DeleteClassViewModel
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
