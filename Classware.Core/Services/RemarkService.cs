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
		public async Task AddRemarkAsync(int studentId, int teacherId, int subjectId, string title, string? description)
		{
			await repo.AddAsync(new Remark()
			{
				Title = title,
				Description = description,
				StudentId = studentId,
				SubjectId = subjectId,
				TeacherId = teacherId
			});

			await repo.SaveChangesAsync();
		}

		public async Task DeleteRemarkByIdAsync(int id)
		{
			var remark = await repo.All<Remark>()
				.Where(r => r.Id == id)
				.FirstOrDefaultAsync();

			if (remark == null)
			{
				throw new NullReferenceException("Such remark doesn't exist");
			}

			remark.IsActive = false;

			await repo.SaveChangesAsync();
		}

		public async Task EditRemarkByIdAsync(int id, string title, string? description)
		{
			var remark = await GetRemarkByIdAsync(id);

			remark.Title = title;
			remark.Description = description;

			await repo.SaveChangesAsync();
		}

		public async Task<Remark> GetRemarkByIdAsync(int id)
		{
			var remark = await repo.All<Remark>()
				.Include(r => r.Subject)
				.Include(r => r.Student)
				.Include(r=>r.Teacher)
				.ThenInclude(t=>t!.User)
				.Where(r => r.IsActive == true && r.Id == id)
				.FirstOrDefaultAsync();

			if (remark == null)
			{
				throw new NullReferenceException("Such remark doesn't exist");
			}

			return remark;
		}
	}
}
