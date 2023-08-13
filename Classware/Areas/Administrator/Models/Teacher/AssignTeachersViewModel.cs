using Classware.Infrastructure.Models;
using Classware.Areas.Administrator.Models.Teacher;
using Classware.Controllers;
using Classware.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Administrator.Models.Teacher
{
	public class AssignTeachersViewModel
	{
		public string TeacherId { get; set; }

		public ICollection<TeacherViewModel>? Teachers { get; set; }

		public string SubjectId { get; set; }

		public ICollection<SubjectViewModel>? Subjects { get; set; }
	}
}
