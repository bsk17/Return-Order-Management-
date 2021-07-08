using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReturnOrderPortal.Controllers
{
    public class ErrorController : Controller
    {

        [Route("Error")]
        public IActionResult HandleHttpError()
        {
            return View("SomethingWentWrong");
        }
    }
}
