using Bodinis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bodinis.WebApp.Controllers
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
            return RedirectToEntryPoint();
        }

        public IActionResult Privacy()
        {
            return RedirectToEntryPoint();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IActionResult RedirectToEntryPoint()
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("AuthToken")))
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Login", "Auth");
        }
    }
}
