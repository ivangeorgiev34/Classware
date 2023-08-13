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
		Task AddComplimentAsync(string studentId, string teacherId, string subjectId, string title, string? description);

		Task DeleteComplimentByIdAsync(string id);

		Task<Compliment> GetComplimentByIdAsync(string id);

		Task EditComplimentByIdAsync(string id, string title, string? description);
	}
}
