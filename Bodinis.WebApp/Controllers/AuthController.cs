using Bodinis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;

namespace Bodinis.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("AuthToken")))
            {
                return RedirectToAction("Index", "Admin");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var client = _httpClientFactory.CreateClient("BodinisApi");
                var response = await client.PostAsJsonAsync("api/auth/login", new LoginApiRequest
                {
                    Email = model.Email,
                    Password = model.Password
                });

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Email o contrasena incorrectos.");
                    return View(model);
                }

                var login = await response.Content.ReadFromJsonAsync<LoginApiResponse>(JsonOptions);

                if (login is null || string.IsNullOrWhiteSpace(login.Token))
                {
                    ModelState.AddModelError(string.Empty, "La API no devolvio un token valido.");
                    return View(model);
                }

                HttpContext.Session.SetString("AuthToken", login.Token);
                HttpContext.Session.SetString("UserName", string.IsNullOrWhiteSpace(login.NombreCompleto) ? login.UserName : login.NombreCompleto);
                HttpContext.Session.SetString("UserRole", login.RolUsuario);

                return RedirectToAction("Index", "Admin");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo conectar con la API. Verifica que Bodinis.WebApi este corriendo.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}
