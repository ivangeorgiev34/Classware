using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}
