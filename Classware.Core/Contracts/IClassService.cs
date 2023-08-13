

using Classware.Infrastructure.Models;

namespace Classware.Core.Contracts
{
	public interface IClassService
	{
		Task AddClassAsync(Class _class);

		Task<bool> ClassExistsByNameAsync(string name);

		Task<IEnumerable<Class>> GetAllClassesAsync();

		Task<IEnumerable<string>> GetAllClassNamesAsync();

		Task<Class> GetClassByIdAsync(string id);

		Task DeleteClassByIdAsync(string id);

		Task<bool> ClassExistsByIdAsync(string id);
	}
}
