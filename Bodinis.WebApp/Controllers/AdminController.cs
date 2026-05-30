using Bodinis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Bodinis.WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new AdminPageViewModel
            {
                Title = "Panel administrativo",
                Subtitle = "Elegi una seccion para empezar a gestionar Bodinis.",
                ActiveSection = "Inicio",
                UserName = GetUserName(),
                Navigation = BuildNavigation("Inicio")
            };

            return View(model);
        }

        public async Task<IActionResult> Productos()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new AdminProductosViewModel
            {
                Title = "Productos",
                Subtitle = "Gestiona el catalogo, la disponibilidad y el stock.",
                ActiveSection = "Productos",
                UserName = GetUserName(),
                Navigation = BuildNavigation("Productos")
            };

            try
            {
                var client = _httpClientFactory.CreateClient("BodinisApi");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

                var response = await client.GetAsync("api/productos");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "Auth");
                }

                if (!response.IsSuccessStatusCode)
                {
                    model.ErrorMessage = "No se pudieron cargar los productos desde la API.";
                    return View(model);
                }

                var productos = await response.Content.ReadFromJsonAsync<List<ProductoApiResponse>>(JsonOptions) ?? new List<ProductoApiResponse>();
                model.Productos = productos.Select(producto => new ProductoAdminViewModel
                {
                    Id = producto.Id,
                    Nombre = producto.NombreProducto,
                    Categoria = producto.Categoria,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    Disponible = producto.Disponible
                }).ToList();
            }
            catch (HttpRequestException)
            {
                model.ErrorMessage = "No se pudo conectar con la API. Verifica que Bodinis.WebApi este corriendo.";
            }

            return View(model);
        }

        private bool IsLoggedIn()
        {
            return !string.IsNullOrWhiteSpace(GetToken());
        }

        private string GetToken()
        {
            return HttpContext.Session.GetString("AuthToken") ?? string.Empty;
        }

        private string GetUserName()
        {
            return HttpContext.Session.GetString("UserName") ?? "Usuario";
        }

        private static IReadOnlyList<AdminNavItemViewModel> BuildNavigation(string activeSection)
        {
            var items = new List<AdminNavItemViewModel>
            {
                new() { Label = "Inicio", Icon = "IN", Action = nameof(Index) },
                new() { Label = "Productos", Icon = "PR", Action = nameof(Productos) },
                new() { Label = "Categorias", Icon = "CA", Action = nameof(Index) },
                new() { Label = "Pedidos", Icon = "PE", Action = nameof(Index) },
                new() { Label = "Ventas", Icon = "$", Action = nameof(Index) },
                new() { Label = "Caja", Icon = "CJ", Action = nameof(Index) }
            };

            foreach (var item in items)
            {
                item.IsActive = item.Label == activeSection;
            }

            return items;
        }

        private class ProductoApiResponse
        {
            public int Id { get; set; }
            public string NombreProducto { get; set; } = string.Empty;
            public int Precio { get; set; }
            public bool Disponible { get; set; }
            public int Stock { get; set; }
            public string Categoria { get; set; } = string.Empty;
        }
    }
}
