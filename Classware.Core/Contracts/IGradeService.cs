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
		Task AddGradeAsync(int studentId,int subjectId,int grade);

		Task<Grade> GetGradeByIdAsync(int id);

		Task DeleteGradeByIdAsync(int id);

		Task EditGradeByIdAsync(int id, int gradeNumber);
	}
}
