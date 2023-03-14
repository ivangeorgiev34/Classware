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
        public async Task AddGradeAsync(int studentId, int teacherId, int subjectId,int grade)
		{
			await repo.AddAsync(new Grade()
			{
				Type = grade,
				StudentId = studentId,
				SubjectId = subjectId,
				TeacherId = teacherId
			});

			await repo.SaveChangesAsync();
		}

		public async Task DeleteGradeByIdAsync(int id)
		{
			var grade =await repo.All<Grade>()
				.Where(g => g.Id == id)
				.FirstOrDefaultAsync();

			if (grade == null)
			{
				throw new NullReferenceException("Such grade doesn't exist");
			}

			grade.IsActive = false;

			await repo.SaveChangesAsync();
		}

		public async Task EditGradeByIdAsync(int id, int gradeNumber)
		{
			var grade = await GetGradeByIdAsync(id);

			grade.Type = gradeNumber;

			await repo.SaveChangesAsync();
		}

		public async Task<Grade> GetGradeByIdAsync(int id)
		{
			var grade =await  repo.All<Grade>()
				.Include(g=>g.Teacher)
				.ThenInclude(t=>t.User)
				.Include(g=>g.Student)
				.ThenInclude(s=>s.Class)
				.Include(g => g.Student)
				.ThenInclude(s=>s.User)
				.Include(g => g.Subject)
				.Where(g => g.IsActive && g.Id == id)
				.FirstOrDefaultAsync();

			if (grade == null)
			{
				throw new NullReferenceException("Such grade doesn't exist");
			}

			return grade;
		}

		public async Task<ICollection<Grade>> GetGradesByStudentIdAndSubjectName(int studentId, string subjectName)
		{
			var grades = await repo.All<Grade>()
				.Include(g => g.Subject)
				.Include(g=>g.Student)
				.Where(g => g.StudentId == studentId && g.Subject!.Name == subjectName)
				.ToListAsync();

			return grades;
		}
	}
}
