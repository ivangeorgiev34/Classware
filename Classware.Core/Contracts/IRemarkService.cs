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

		Task AddRemarkAsync(int studentId, int teacherId, int subjectId, string title, string? description);

		Task DeleteRemarkByIdAsync(int id);

		Task<Remark> GetRemarkByIdAsync(int id);

		Task EditRemarkByIdAsync(int id, string title, string? description);
	}
}
