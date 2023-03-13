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

		Task DeleteStudentByIdAsync(int id);

		Task<Student> GetStudentByIdAsync(int id);

		Task AssignSubjectsAsync(ICollection<int> subjectIds,int studentId);

		Task<IEnumerable<Student>> GetStudentsByClassIdAndSubjectName(int classId,string subjectName);

		Task<bool> StudentHasASubjectAsync(Student student,string subjectName);
	}
}
