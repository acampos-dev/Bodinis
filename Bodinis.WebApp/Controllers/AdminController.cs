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

        public async Task<IActionResult> Pedidos()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await BuildPedidosModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPedido(PedidoFormViewModel form)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (form.ProductoIds.Count == 0 || form.ProductoIds.Count != form.Cantidades.Count)
            {
                TempData["PedidosError"] = "Agrega al menos un producto al pedido.";
                return RedirectToAction(nameof(Pedidos));
            }

            var usuarioId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (usuarioId <= 0)
            {
                TempData["PedidosError"] = "No se encontro el usuario de la sesion.";
                return RedirectToAction(nameof(Pedidos));
            }

            var detalles = form.ProductoIds
                .Select((productoId, index) => new PedidoDetalleApiRequest
                {
                    ProductoId = productoId,
                    Cantidad = form.Cantidades[index]
                })
                .Where(detalle => detalle.ProductoId > 0 && detalle.Cantidad > 0)
                .ToList();

            if (detalles.Count == 0)
            {
                TempData["PedidosError"] = "Agrega al menos un producto con cantidad valida.";
                return RedirectToAction(nameof(Pedidos));
            }

            var response = await SendApiRequest(client => client.PostAsJsonAsync("api/pedidos", new PedidoApiWriteRequest
            {
                UsuarioId = usuarioId,
                TipoPedido = form.TipoPedido,
                NombreCliente = form.NombreCliente,
                TelefonoCliente = form.TelefonoCliente,
                DireccionCliente = form.TipoPedido == 2 ? form.DireccionCliente : null,
                Detalles = detalles
            }));

            if (response.IsSuccessStatusCode)
            {
                TempData["PedidosSuccess"] = "Pedido creado correctamente.";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                TempData["PedidosError"] = "Tu sesion vencio. Vuelve a iniciar sesion.";
            }
            else
            {
                TempData["PedidosError"] = "No se pudo crear el pedido.";
            }

            return RedirectToAction(nameof(Pedidos));
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

        private async Task<AdminPedidosViewModel> BuildPedidosModel()
        {
            var model = new AdminPedidosViewModel
            {
                Title = "Pedidos",
                Subtitle = "Carga rapida de pedidos con busqueda de productos.",
                ActiveSection = "Pedidos",
                UserName = GetUserName(),
                Navigation = BuildNavigation("Pedidos"),
                SuccessMessage = TempData["PedidosSuccess"] as string,
                ErrorMessage = TempData["PedidosError"] as string
            };

            try
            {
                var client = CreateApiClient();
                var productos = await GetProductos(client);
                model.Productos = productos.Where(producto => producto.PuedeAgregarse).ToList();
                model.MetodosPago = await GetMetodosPago(client);

                var response = await client.GetAsync("api/pedidos");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    model.ErrorMessage = "Tu sesion vencio. Vuelve a iniciar sesion.";
                    return model;
                }

                if (!response.IsSuccessStatusCode)
                {
                    model.ErrorMessage ??= "No se pudieron cargar los pedidos desde la API.";
                    return model;
                }

                var pedidos = await response.Content.ReadFromJsonAsync<List<PedidoApiResponse>>(JsonOptions) ?? new List<PedidoApiResponse>();
                model.Pedidos = pedidos
                    .OrderByDescending(pedido => pedido.FechaHora)
                    .Take(12)
                    .Select(pedido => new PedidoAdminViewModel
                    {
                        Id = pedido.Id,
                        FechaHora = pedido.FechaHora,
                        TipoPedido = MapTipoPedido(pedido.TipoPedido),
                        Estado = MapEstadoPedido(pedido.Estado),
                        Total = pedido.Total
                    })
                    .ToList();
            }
            catch (HttpRequestException)
            {
                model.ErrorMessage = "No se pudo conectar con la API. Verifica que Bodinis.WebApi este corriendo.";
            }

            return model;
        }

        private async Task<List<ProductoAdminViewModel>> GetProductos(HttpClient client)
        {
            var response = await client.GetAsync("api/productos");

            if (!response.IsSuccessStatusCode)
            {
                return new List<ProductoAdminViewModel>();
            }

            var productos = await response.Content.ReadFromJsonAsync<List<ProductoApiResponse>>(JsonOptions) ?? new List<ProductoApiResponse>();

            return productos.Select(producto => new ProductoAdminViewModel
            {
                Id = producto.Id,
                Nombre = producto.NombreProducto,
                Categoria = producto.Categoria,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Disponible = producto.Disponible
            }).ToList();
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

        private async Task<List<MetodoPagoOptionViewModel>> GetMetodosPago(HttpClient client)
        {
            var response = await client.GetAsync("api/metodos-pago");

            if (!response.IsSuccessStatusCode)
            {
                return new List<MetodoPagoOptionViewModel>
                {
                    new() { Id = 1, Nombre = "Efectivo" },
                    new() { Id = 2, Nombre = "Debito" },
                    new() { Id = 3, Nombre = "Credito 1 cuota" }
                };
            }

            var metodos = await response.Content.ReadFromJsonAsync<List<MetodoPagoApiResponse>>(JsonOptions) ?? new List<MetodoPagoApiResponse>();

            return metodos
                .Where(metodo => metodo.Activo)
                .Select(metodo => new MetodoPagoOptionViewModel
                {
                    Id = metodo.Id,
                    Nombre = metodo.Nombre
                })
                .ToList();
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
                new() { Label = "Pedidos", Icon = "PE", Action = nameof(Pedidos) },
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

        private class MetodoPagoApiResponse
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public bool Activo { get; set; }
        }

        private class PedidoApiResponse
        {
            public int Id { get; set; }
            public DateTime FechaHora { get; set; }
            public int TipoPedido { get; set; }
            public int Estado { get; set; }
            public int Total { get; set; }
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

        private class PedidoApiWriteRequest
        {
            public int UsuarioId { get; set; }
            public int TipoPedido { get; set; }
            public string? NombreCliente { get; set; }
            public string? TelefonoCliente { get; set; }
            public string? DireccionCliente { get; set; }
            public List<PedidoDetalleApiRequest> Detalles { get; set; } = new();
        }

        private class PedidoDetalleApiRequest
        {
            public int ProductoId { get; set; }
            public int Cantidad { get; set; }
        }

        private static string MapTipoPedido(int tipoPedido)
        {
            return tipoPedido == 2 ? "Delivery" : "Mostrador";
        }

        private static string MapEstadoPedido(int estado)
        {
            return estado switch
            {
                1 => "Pendiente",
                2 => "Preparacion",
                3 => "Entregado",
                4 => "Cancelado",
                _ => "Sin estado"
            };
        }
    }
}
