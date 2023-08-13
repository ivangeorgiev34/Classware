using Classware.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Core.Contracts
{
	public interface IGradeService
	{
		Task AddGradeAsync(string studentId, string teacherId, string subjectId,int grade);

		Task<Grade> GetGradeByIdAsync(string id);

		Task DeleteGradeByIdAsync(string id);

		Task EditGradeByIdAsync(string id, int gradeNumber);

		Task<ICollection<Grade>> GetGradesByStudentIdAndSubjectName(string studentId, string subjectName);
	}
}
