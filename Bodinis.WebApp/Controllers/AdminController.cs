using Bodinis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApp.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View(new AdminLoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData["LoginInfo"] = "El diseño del login ya está listo. El próximo paso es conectarlo con la API de autenticación.";
            return RedirectToAction(nameof(Login));
        }
    }
}
