using Classware.Areas.Administrator.Models.Student;
using Classware.Areas.Student.Models.Subject;
using Classware.Core.Contracts;
using Classware.Extensions;
using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Student.Controllers
{
	public class SubjectController : BaseController
	{
		private readonly IStudentService studentService;
		private readonly IGradeService gradeService;

		public SubjectController(IStudentService _studentService,
			IGradeService _gradeService)
        {
            studentService = _studentService;
            gradeService = _gradeService;
        }
        /// <summary>
        /// Gets all of the given students subjects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> All()
		{
            
            var student = await studentService.GetStudentByUserIdAsync(User.Id());

            var subjects = student.StudentSubjects
                .Where(ss => ss.Subject?.IsActive == true)
                .Select(ss=>ss.Subject)
                .ToList();

            ICollection<Models.Subject.SubjectViewModel> subjectViewModels = new List<Models.Subject.SubjectViewModel>();

            foreach (var subject in subjects)
            {
                var grades = await gradeService.GetGradesByStudentIdAndSubjectName(student.Id, subject.Name);

                ICollection<GradeViewModel> gradeViewModels = new List<GradeViewModel>();

                foreach (var grade in grades)
                {
                    gradeViewModels.Add(new GradeViewModel()
                    {
                        Id = grade.Id,
                        Grade = grade.Type
                    });
				}

                subjectViewModels.Add(new Models.Subject.SubjectViewModel()
                {
                    Id = subject!.Id,
                    Name = subject!.Name,
                    Grades = gradeViewModels
				});
			}

            var model = new AllSubjectsViewModel()
            {
                Subjects = subjectViewModels
            };

			return View(model);
		}
        
	}
}
