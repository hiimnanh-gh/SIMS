using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SIMS.Models;

namespace SIMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var roleId = HttpContext.Session.GetInt32("RoleID");

            if (roleId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.RoleID = roleId;
            return View();
        }
    }
}
