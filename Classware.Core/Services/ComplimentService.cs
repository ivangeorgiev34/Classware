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
	public class ComplimentService : IComplimentService
	{
		private readonly IRepository repo;

        public ComplimentService(IRepository _repo)
        {
			repo = _repo;
        }
        public async Task AddComplimentAsync(string studentId, string teacherId, string subjectId, string title, string? description)
		{
			await repo.AddAsync(new Compliment()
			{
				Title = title,
				Description = description,
				StudentId = new Guid(studentId),
				SubjectId = new Guid(subjectId),
				TeacherId = new Guid(teacherId)
			});

			await repo.SaveChangesAsync();
		}

		public async Task DeleteComplimentByIdAsync(string id)
		{
			var compliment =await repo.All<Compliment>()
				.Where(c=>c.Id == new Guid(id))
				.FirstOrDefaultAsync();

			if (compliment == null)
			{
				throw new NullReferenceException("Such compliment doesn't exist");
			}

			compliment.IsActive = false;

			await repo.SaveChangesAsync();
		}

		public async Task EditComplimentByIdAsync(string id, string title, string? description)
		{
			var compliment = await repo.All<Compliment>()
				.Where(c => c.IsActive == true && c.Id == new Guid(id))
				.FirstOrDefaultAsync();

			if (compliment == null)
			{
				throw new NullReferenceException("Such compliment doesn't exist");
			}

			compliment.Title = title;
			compliment.Description = description;

			await repo.SaveChangesAsync();
		}

		public async Task<Compliment> GetComplimentByIdAsync(string id)
		{
			var compliment = await repo.All<Compliment>()
				.Include(c => c.Teacher)
				.ThenInclude(t => t!.User)
				.Include(c => c.Subject)
				.Include(c => c.Student)
				.ThenInclude(s => s!.User)
				.Where(c => c.IsActive == true && c.Id == new Guid(id))
				.FirstOrDefaultAsync();

			if (compliment == null)
			{
				throw new NullReferenceException("Such compliment doesn't exist");
			}

			return compliment;
		}
	}
}
