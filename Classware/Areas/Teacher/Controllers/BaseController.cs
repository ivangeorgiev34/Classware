using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Classware.Areas.Teacher.Controllers
{
	[Area(nameof(Teacher))]
	[Route("teacher/[controller]/[action]/{id?}")]
	[Authorize(Roles = nameof(Teacher))]
	public class BaseController : Controller
	{
	}
}
