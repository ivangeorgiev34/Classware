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
	public class SubjectService : ISubjectService
	{
		private readonly IRepository repo;

		public SubjectService(IRepository _repo)
		{
			repo = _repo;
		}
		public async Task AddSubjectAsync(Subject subject)
		{
			await repo.AddAsync(subject);

			await repo.SaveChangesAsync();
		}

		public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
		{
			return await repo.All<Subject>()
				.Include(s => s.StudentSubjects)
				.ThenInclude(s => s.Student)
				.Where(s => s.IsActive)
				.ToListAsync();
		}

		public async Task<ICollection<Subject>> GetAllSubjectsByIdsAsync(ICollection<int> subjectIds)
		{
			var allSubjects = await repo.All<Subject>()
				.Where(s => s.IsActive)
				.ToListAsync();

			var subjects = new List<Subject>();

			foreach (var id in subjectIds)
			{
				foreach (var subject in allSubjects)
				{
					if (subject.Id == id)
					{
						subjects.Add(subject);

						break;
					}
				}
			}

			return subjects;
		}

		public async Task<Subject> GetSubjectByNameAsync(string subjectName)
		{
			var subject = await repo.All<Subject>()
				.Where(s => s.IsActive == true && s.Name == subjectName)
				.FirstOrDefaultAsync();

			if (subject == null)
			{
				throw new NullReferenceException("Subject with such name doesn't exist");
			}

			return subject;
		}

		public async Task<bool> SubjectExistsByNameAsync(string name)
		{
			if (await repo.AllReadonly<Subject>().AnyAsync(s => s.Name == name && s.IsActive == true))
			{
				return true;
			}
			return false;
		}


	}
}
