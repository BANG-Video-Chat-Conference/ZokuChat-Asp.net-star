using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ZokuChat.Models;

namespace ZokuChat.Controllers
{
    [Authorize]
    [Route("Contacts")]
    public class ContactsController : Controller
    {
        [Route("")]
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
