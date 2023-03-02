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

        /// <summary>
        /// Checks if there is already a class with the same name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
		public async Task<bool> ClassExistsByNameAsync(string name)
		{
            if (await repo.AllReadonly<Class>().AnyAsync(c=>c.Name == name))
            {
                return true;
            }

            return false;
		}
	}
}
