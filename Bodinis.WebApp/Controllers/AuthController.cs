using Bodinis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;

namespace Bodinis.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory; // Inyectamos el IHttpClientFactory para crear clientes HTTP
        private static readonly JsonSerializerOptions JsonOptions = new() // Configuramos las opciones de JSON para que ignore mayúsculas/minúsculas en los nombres de las propiedades
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
        [ValidateAntiForgeryToken] // Protegemos el método POST contra ataques CSRF
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) // Si el modelo no es válido, volvemos a mostrar la vista con los errores de validación
            {
                return View(model);
            }

            try
            {
                var client = _httpClientFactory.CreateClient("BodinisApi"); // Creamos un cliente HTTP utilizando el IHttpClientFactory y el nombre de la configuración del cliente
                var response = await client.PostAsJsonAsync("api/auth/login", new LoginApiRequest // Enviamos una solicitud POST a la API con el modelo de inicio de sesión convertido a un objeto de solicitud para la API
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
                HttpContext.Session.SetInt32("UserId", login.UsuarioId);
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
