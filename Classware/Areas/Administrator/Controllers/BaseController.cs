﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Classware.Areas.Administrator.Controllers
{
    [Area(nameof(Administrator))]
    [Route("administrator/[controller]/[action]/{id?}")]
    [Authorize(Roles = nameof(Administrator))]
    public class BaseController : Controller
    {
    }
}
