using Classware.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Core.Contracts
{
	public interface IComplimentService
	{
		Task AddComplimentAsync(int studentId, int subjectId, string title, string? description);

		Task DeleteComplimentByIdAsync(int id);

		Task<Compliment> GetComplimentByIdAsync(int id);

		Task EditComplimentByIdAsync(int id, string title, string? description);
	}
}
