using Classware.Core.Contracts;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Core.Services
{
	public class TeacherService : ITeacherService
	{
		private readonly IRepository repo;
		private readonly UserManager<ApplicationUser> userManager;

		public TeacherService(IRepository _repo,
			UserManager<ApplicationUser> _userManager)
		{
			repo = _repo;
			userManager= _userManager;
		}

		public async Task AddTeacher(Teacher teacher)
		{
			await repo.AddAsync(teacher);
			await repo.SaveChangesAsync();
		}

		public async Task AssignSubjectToTeacherAsync(int teacherId,int subjectId)
		{
			var teacher = await repo.All<Teacher>()
				.FirstOrDefaultAsync(t => t.IsActive == true && t.Id == teacherId) ?? null;

			if (teacher == null)
			{
				throw new InvalidOperationException("Teacher with such id doesn't exist");
			}

			teacher.SubjectId = subjectId;

			await repo.SaveChangesAsync();
		}

		public async Task DeleteTeacherByIdAsync(int id)
		{
			var teacher = await GetTeacherByIdAsync(id);

			teacher.IsActive = false;

			await repo.SaveChangesAsync();
		}

		public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
		{
			return await repo.All<Teacher>()
				.Include(t => t.User)
				.Include(t=>t.Subject)
				.Where(t => t.IsActive == true)
				.ToListAsync();
		}

		public async Task<Teacher> GetTeacherByIdAsync(int teacherId)
		{
			var teacher = await repo.All<Teacher>()
				.Include(t=>t.User)
				.Include(t=>t.Subject)
				.FirstOrDefaultAsync(t => t.IsActive == true && t.Id == teacherId) ?? null;

			if (teacher == null)
			{
				throw new InvalidOperationException("Teacher with such id doesn't exist");
			}

			return teacher;
		}

		public async Task<Teacher> GetTeacherByUserIdAsync(string id)
		{
			var teacher = await repo.All<Teacher>()
				.Include(t=>t.Subject)
				.Where(t => t.IsActive && t.UserId == id)
				.FirstOrDefaultAsync();

			if (teacher == null)
			{
				throw new NullReferenceException("Teacher with such id doesn't exist");
			}

			return teacher;
		}

		public async Task<bool> TeacherHasASubjectAsync(string id)
		{
			var user =await userManager.FindByIdAsync(id);

			if (await repo.All<Teacher>().AnyAsync(t=>t.UserId == id && t.Subject != null))
			{
				return true;
			}

			return false;
		}
	}
}
