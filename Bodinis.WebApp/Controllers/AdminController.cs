using Bodinis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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

            var model = await BuildProductosModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearProducto(ProductoFormViewModel form)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                TempData["ProductsError"] = "Revisa los datos del producto nuevo.";
                return RedirectToAction(nameof(Productos));
            }

            var response = await SendApiRequest(client => client.PostAsJsonAsync("api/productos", new ProductoApiWriteRequest
            {
                NombreProducto = form.NombreProducto,
                Precio = form.Precio,
                Disponible = form.Disponible,
                Stock = form.Stock,
                CategoriaId = form.CategoriaId
            }));

            SetProductOperationMessage(response, "Producto creado correctamente.", "No se pudo crear el producto.");
            return RedirectToAction(nameof(Productos));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProducto(ProductoFormViewModel form)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid || form.Id <= 0)
            {
                TempData["ProductsError"] = "Revisa los datos del producto.";
                return RedirectToAction(nameof(Productos));
            }

            var response = await SendApiRequest(client => client.PutAsJsonAsync($"api/productos/{form.Id}", new ProductoApiUpdateRequest
            {
                Id = form.Id,
                NombreProducto = form.NombreProducto,
                Precio = form.Precio,
                Disponible = form.Disponible,
                Stock = form.Stock,
                CategoriaId = form.CategoriaId
            }));

            SetProductOperationMessage(response, "Producto actualizado correctamente.", "No se pudo actualizar el producto.");
            return RedirectToAction(nameof(Productos));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DesactivarProducto(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id <= 0)
            {
                TempData["ProductsError"] = "No se encontro el producto a desactivar.";
                return RedirectToAction(nameof(Productos));
            }

            var response = await SendApiRequest(client => client.PatchAsync($"api/productos/{id}/desactivar", content: null));
            SetProductOperationMessage(response, "Producto desactivado correctamente.", "No se pudo desactivar el producto.");
            return RedirectToAction(nameof(Productos));
        }

        private async Task<AdminProductosViewModel> BuildProductosModel()
        {
            var model = new AdminProductosViewModel
            {
                Title = "Productos",
                Subtitle = "Gestiona el catalogo, la disponibilidad y el stock.",
                ActiveSection = "Productos",
                UserName = GetUserName(),
                Navigation = BuildNavigation("Productos"),
                SuccessMessage = TempData["ProductsSuccess"] as string,
                ErrorMessage = TempData["ProductsError"] as string
            };

            try
            {
                var client = CreateApiClient();

                var categorias = await GetCategorias(client);
                model.Categorias = categorias;

                var response = await client.GetAsync("api/productos");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    model.ErrorMessage = "Tu sesion vencio. Vuelve a iniciar sesion.";
                    return model;
                }

                if (!response.IsSuccessStatusCode)
                {
                    model.ErrorMessage ??= "No se pudieron cargar los productos desde la API.";
                    return model;
                }

                var productos = await response.Content.ReadFromJsonAsync<List<ProductoApiResponse>>(JsonOptions) ?? new List<ProductoApiResponse>();
                model.Productos = productos.Select(producto => new ProductoAdminViewModel
                {
                    Id = producto.Id,
                    Nombre = producto.NombreProducto,
                    Categoria = producto.Categoria,
                    CategoriaId = categorias.FirstOrDefault(categoria => categoria.Nombre == producto.Categoria)?.Id ?? 0,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    Disponible = producto.Disponible
                }).ToList();
            }
            catch (HttpRequestException)
            {
                model.ErrorMessage = "No se pudo conectar con la API. Verifica que Bodinis.WebApi este corriendo.";
            }

            return model;
        }

        private async Task<List<CategoriaOptionViewModel>> GetCategorias(HttpClient client)
        {
            var response = await client.GetAsync("api/categorias");

            if (!response.IsSuccessStatusCode)
            {
                return new List<CategoriaOptionViewModel>();
            }

            var categorias = await response.Content.ReadFromJsonAsync<List<CategoriaApiResponse>>(JsonOptions) ?? new List<CategoriaApiResponse>();

            return categorias.Select(categoria => new CategoriaOptionViewModel
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            }).ToList();
        }

        private async Task<HttpResponseMessage> SendApiRequest(Func<HttpClient, Task<HttpResponseMessage>> request)
        {
            try
            {
                return await request(CreateApiClient());
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }

        private void SetProductOperationMessage(HttpResponseMessage response, string successMessage, string fallbackErrorMessage)
        {
            if (response.IsSuccessStatusCode)
            {
                TempData["ProductsSuccess"] = successMessage;
                return;
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["ProductsError"] = "Tu usuario no tiene permisos para realizar esta accion.";
                return;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                TempData["ProductsError"] = "Tu sesion vencio. Vuelve a iniciar sesion.";
                return;
            }

            TempData["ProductsError"] = fallbackErrorMessage;
        }

        private HttpClient CreateApiClient()
        {
            var client = _httpClientFactory.CreateClient("BodinisApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            return client;
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

        private class CategoriaApiResponse
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = string.Empty;
        }

        private class ProductoApiWriteRequest
        {
            public string NombreProducto { get; set; } = string.Empty;
            public int Precio { get; set; }
            public bool Disponible { get; set; }
            public int Stock { get; set; }
            public int CategoriaId { get; set; }
        }

        private class ProductoApiUpdateRequest : ProductoApiWriteRequest
        {
            public int Id { get; set; }
        }
    }
}
