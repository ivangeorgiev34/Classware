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
	public class GradeService : IGradeService
	{
		private readonly IRepository repo;

        public GradeService(IRepository _repo)
        {
            repo  = _repo;
        }
        public async Task AddGradeAsync(string studentId, string teacherId, string subjectId,int grade)
		{
			await repo.AddAsync(new Grade()
			{
				Type = grade,
				StudentId = new Guid(studentId),
				SubjectId = new Guid(subjectId),
				TeacherId = new Guid(teacherId)
			});

			await repo.SaveChangesAsync();
		}

		public async Task DeleteGradeByIdAsync(string id)
		{
			var grade =await repo.All<Grade>()
				.Where(g => g.Id == new Guid(id) && g.IsActive == true)
				.FirstOrDefaultAsync();

			if (grade == null)
			{
				throw new NullReferenceException("Such grade doesn't exist");
			}

			grade.IsActive = false;

			await repo.SaveChangesAsync();
		}

		public async Task EditGradeByIdAsync(string id, int gradeNumber)
		{
			var grade = await GetGradeByIdAsync(id);

			grade.Type = gradeNumber;

			await repo.SaveChangesAsync();
		}

		public async Task<Grade> GetGradeByIdAsync(string id)
		{
			var grade =await  repo.All<Grade>()
				.Include(g=>g.Teacher)
				.ThenInclude(t=>t!.User)
				.Include(g=>g.Student)
				.ThenInclude(s=>s!.Class)
				.Include(g => g.Student)
				.ThenInclude(s=>s!.User)
				.Include(g => g.Subject)
				.Where(g => g.IsActive && g.Id == new Guid(id))
				.FirstOrDefaultAsync();

			if (grade == null)
			{
				throw new NullReferenceException("Such grade doesn't exist");
			}

			return grade;
		}

		public async Task<ICollection<Grade>> GetGradesByStudentIdAndSubjectName(string studentId, string subjectName)
		{
			var grades = await repo.All<Grade>()
				.Include(g => g.Subject)
				.Include(g=>g.Student)
				.Where(g => g.StudentId == new Guid(studentId) && g.Subject!.Name == subjectName && g.IsActive == true)
				.ToListAsync();

			return grades;
		}
	}
}
