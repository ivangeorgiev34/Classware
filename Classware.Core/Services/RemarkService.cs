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
	public class RemarkService : IRemarkService
	{
		private readonly IRepository repo;

		public RemarkService(IRepository _repo)
		{
			repo = _repo;
		}
		public async Task AddRemarkAsync(string studentId, string teacherId, string subjectId, string title, string? description)
		{
			await repo.AddAsync(new Remark()
			{
				Title = title,
				Description = description,
				StudentId = new Guid(studentId),
				SubjectId = new Guid(subjectId),
				TeacherId = new Guid(teacherId)
			});

			await repo.SaveChangesAsync();
		}

		public async Task DeleteRemarkByIdAsync(string id)
		{
			var remark = await repo.All<Remark>()
				.Where(r => r.Id == new Guid(id))
				.FirstOrDefaultAsync();

			if (remark == null)
			{
				throw new NullReferenceException("Such remark doesn't exist");
			}

			remark.IsActive = false;

			await repo.SaveChangesAsync();
		}

		public async Task EditRemarkByIdAsync(string id, string title, string? description)
		{
			var remark = await GetRemarkByIdAsync(id);

			remark.Title = title;
			remark.Description = description;

			await repo.SaveChangesAsync();
		}

		public async Task<Remark> GetRemarkByIdAsync(string id)
		{
			var remark = await repo.All<Remark>()
				.Include(r => r.Subject)
				.Include(r => r.Student)
				.Include(r=>r.Teacher)
				.ThenInclude(t=>t!.User)
				.Where(r => r.IsActive == true && r.Id == new Guid(id))
				.FirstOrDefaultAsync();

			if (remark == null)
			{
				throw new NullReferenceException("Such remark doesn't exist");
			}

			return remark;
		}
	}
}
