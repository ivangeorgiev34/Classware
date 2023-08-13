using Classware.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Core.Contracts
{
	public interface IRemarkService
	{

		Task AddRemarkAsync(string studentId, string teacherId, string subjectId, string title, string? description);

		Task DeleteRemarkByIdAsync(string id);

		Task<Remark> GetRemarkByIdAsync(string id);

		Task EditRemarkByIdAsync(string id, string title, string? description);
	}
}
