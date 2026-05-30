using Bodinis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApp.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData["LoginInfo"] = "El login visual está listo. Luego lo conectamos con la autenticación real.";
            return RedirectToAction(nameof(Login));
        }
    }
}
