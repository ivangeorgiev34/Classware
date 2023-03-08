using Classware.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Core.Contracts
{
	public interface ITeacherService
	{
		Task AddTeacher(Teacher teacher);

		Task<IEnumerable<Teacher>> GetAllTeachersAsync();

		Task<Teacher> GetTeacherByIdAsync(int teacherId);

		Task AssignSubjectToTeacherAsync(int teacherId,int subjectId);

		Task DeleteTeacherByIdAsync(int id);

		Task<bool> TeacherHasASubjectAsync(string id);

		Task<Teacher> GetTeacherByUserIdAsync(string id);

	}
}
