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

		bool AddGradeToStudentAsync(Student student,string subjectName,int grade);
	}
}
