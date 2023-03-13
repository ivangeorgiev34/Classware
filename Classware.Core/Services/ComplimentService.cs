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
        public async Task AddComplimentAsync(int studentId, int subjectId, string title, string? description)
		{
			await repo.AddAsync(new Compliment()
			{
				Title = title,
				Description = description,
				StudentId = studentId,
				SubjectId = subjectId
			});

			await repo.SaveChangesAsync();
		}

		public async Task DeleteComplimentByIdAsync(int id)
		{
			var compliment =await repo.All<Compliment>()
				.Where(c=>c.Id == id)
				.FirstOrDefaultAsync();

			if (compliment == null)
			{
				throw new NullReferenceException("Such compliment doesn't exist");
			}

			compliment.IsActive = false;

			await repo.SaveChangesAsync();
		}

		public async Task EditComplimentByIdAsync(int id, string title, string? description)
		{
			var compliment = await repo.All<Compliment>()
				.Where(c => c.IsActive == true && c.Id == id)
				.FirstOrDefaultAsync();

			if (compliment == null)
			{
				throw new NullReferenceException("Such compliment doesn't exist");
			}

			compliment.Title = title;
			compliment.Description = description;

			await repo.SaveChangesAsync();
		}

		public async Task<Compliment> GetComplimentByIdAsync(int id)
		{
			var compliment = await repo.All<Compliment>()
				.Where(c => c.IsActive == true && c.Id == id)
				.FirstOrDefaultAsync();

			if (compliment == null)
			{
				throw new NullReferenceException("Such compliment doesn't exist");
			}

			return compliment;
		}
	}
}
