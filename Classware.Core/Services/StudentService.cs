﻿using Classware.Core.Contracts;
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
	public class StudentService : IStudentService
	{
		private readonly IRepository repo;
		private readonly ISubjectService subjectService;

		public StudentService(IRepository _repo,
			ISubjectService _subjectService)
		{
			repo = _repo;
			subjectService = _subjectService;
		}
		public async Task AddStudentAsync(Student student)
		{
			await repo.AddAsync(student);

			await repo.SaveChangesAsync();
		}

		public async Task AssignSubjectsAsync(ICollection<string> subjectIds, string studentId)
		{
			if (subjectIds.Count == 0)
			{
				throw new NullReferenceException("No subject ids were given");
			}

			var subjects = await subjectService.GetAllSubjectsByIdsAsync(subjectIds);

			var student = await GetStudentByIdAsync(studentId);

			foreach (var subject in subjects)
			{
				if (student.StudentSubjects.Any(ss => ss.SubjectId == subject.Id))
				{
					continue;
				}

				var studentSubjectPair = new StudentSubject()
				{
					SubjectId = subject.Id,
					StudentId = new Guid(studentId)
				};

				student.StudentSubjects.Add(studentSubjectPair);
			}

			await repo.SaveChangesAsync();
		}

		public async Task DeleteStudentByIdAsync(string id)
		{
			var student = await GetStudentByIdAsync(id);

			student.IsActive = false;

			await repo.SaveChangesAsync();
		}

		public async Task<IEnumerable<Student>> GetAllStudentsAsync()
		{
			return await repo.All<Student>()
				.Include(s => s.Class)
				.Include(s => s.User)
				.Include(s => s.StudentSubjects)
				.ThenInclude(s => s.Subject)
				.Where(s => s.IsActive)
				.ToListAsync();
		}

		public async Task<Student> GetStudentByIdAsync(string id)
		{
			var student = await repo.All<Student>()
				.Include(s => s.Remarks)
				.ThenInclude(c => c.Teacher)
				.Include(s => s.Compliments)
				.ThenInclude(c => c.Teacher)
				.Include(s => s.User)
				.Include(s => s.Class)
				.Include(s => s.StudentSubjects)
				.ThenInclude(ss => ss.Subject)
				.FirstOrDefaultAsync(s => s.Id == new Guid(id) && s.IsActive == true);

			if (student == null)
			{
				throw new InvalidOperationException("Student with such id doesn't exist");
			}
			return student;
		}

		public async Task<Student> GetStudentByUserIdAsync(string id)
		{
			var student = await repo.All<Student>()
				.Include(s => s.Class)
				.Include(s => s.User)
				.Include(s => s.Compliments)
				.Include(s => s.Grades)
				.Include(s => s.Remarks)
				.Include(s => s.StudentSubjects)
				.ThenInclude(ss => ss.Subject)
				.Where(s => s.IsActive == true && s.UserId == id)
				.FirstOrDefaultAsync();

			if (student == null)
			{
				throw new NullReferenceException("Such student doesn't exits");
			}

			return student;
		}

		public async Task<IEnumerable<Student>> GetStudentsByClassIdAndSubjectName(string classId, string subjectName)
		{
			var students = await repo.All<Student>()
				.Include(s => s.Grades)
				.Include(s => s.User)
				.Include(s => s.StudentSubjects)
				.ThenInclude(ss => ss.Subject)
				.Where(s => s.ClassId == new Guid(classId) && s.StudentSubjects.Any(s => s.Subject.Name == subjectName) && s.IsActive == true)
				.ToListAsync();

			return students;
		}

		public async Task<bool> StudentExistsByUserId(string id)
		{
			return await repo.AllReadonly<Student>()
				.AnyAsync(c => c.IsActive == true && c.Id == new Guid(id));
		}

		public async Task<bool> StudentHasASubjectAsync(Student student, string subjectName)
		{
			if (student.StudentSubjects.Any(s => s.Subject.Name == subjectName))
			{
				return true;
			}
			return false;
		}
	}
}
