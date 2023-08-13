using Classware.Core.Contracts;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Core.Services
{
	public class ClassService : IClassService
	{
		private readonly IRepository repo;

        public ClassService(IRepository _repo)
        {
            repo= _repo;
        }
        /// <summary>
        /// Adds a class to the database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AddClassAsync(Class _class)
		{
            await repo.AddAsync<Class>(_class);

            await repo.SaveChangesAsync();
		}

		public async Task<bool> ClassExistsByIdAsync(string id)
		{
            var classExists = await repo.AllReadonly<Class>()
                .AnyAsync(c => c.IsActive == true && c.Id == new Guid(id));

			return classExists;
		}

		/// <summary>
		/// Checks if there is already a class with the same name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<bool> ClassExistsByNameAsync(string name)
		{
            if (await repo.AllReadonly<Class>().Where(c=>c.IsActive).AnyAsync(c=>c.Name == name))
            {
                return true;
            }

            return false;
		}

        /// <summary>
        /// Sets the given class to not active
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

		public async Task DeleteClassByIdAsync(string id)
		{
            var _class = await GetClassByIdAsync(id);

            _class.IsActive = false;

            await repo.SaveChangesAsync();
		}


		/// <summary>
		/// Gets all active classes
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<IEnumerable<Class>> GetAllClassesAsync()
		{
            IEnumerable<Class> classes =await repo.All<Class>()
                .Include(c=>c.Students)
                .ThenInclude(s=>s.StudentSubjects)
                .ThenInclude(ss=>ss.Subject)
                .Where(c => c.IsActive)
                .ToListAsync();

            return classes;
		}

        /// <summary>
        /// Gets all classes' names
        /// </summary>
        /// <returns></returns>
		public async Task<IEnumerable<string>> GetAllClassNamesAsync()
		{
            IEnumerable<string> classNames = await repo.AllReadonly<Class>()
                .Where(c => c.IsActive)
                .Select(c => c.Name)
                .ToListAsync();

            return classNames;
		}

        /// <summary>
        /// Gets the class with the corresponding id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public async Task<Class> GetClassByIdAsync(string id)
		{
            var _class =await repo.All<Class>()
                .FirstOrDefaultAsync(c => c.IsActive && c.Id == new Guid(id));

            if (_class == null)
            {
                return null;
            }

            return _class;
		}
	}
}
