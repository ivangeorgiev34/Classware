

using Classware.Infrastructure.Models;

namespace Classware.Core.Contracts
{
	public interface IClassService
	{
		Task AddClassAsync(Class _class);

		Task<bool> ClassExistsByNameAsync(string name);
	}
}
