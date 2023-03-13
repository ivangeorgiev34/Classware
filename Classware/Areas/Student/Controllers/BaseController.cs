using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Classware.Areas.Student.Controllers
{
	[Area(nameof(Student))]
	[Route("student/[controller]/[action]/{id?}")]
	[Authorize(Roles = nameof(Student))]
	public class BaseController : Controller
	{
	}
}
