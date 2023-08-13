using Classware.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Core.Contracts
{
    public interface IStudentService
    {
		Task AddStudentAsync(Student student);

		Task<IEnumerable<Student>> GetAllStudentsAsync();

		Task DeleteStudentByIdAsync(string id);

		Task<Student> GetStudentByIdAsync(string id);

		Task AssignSubjectsAsync(ICollection<string> subjectIds,string studentId);

		Task<IEnumerable<Student>> GetStudentsByClassIdAndSubjectName(string classId,string subjectName);

		Task<bool> StudentHasASubjectAsync(Student student,string subjectName);

		Task<Student> GetStudentByUserIdAsync(string id);
	}
}
