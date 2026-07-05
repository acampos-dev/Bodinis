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
        private readonly IHttpClientFactory _httpClientFactory; // Inyección de IHttpClientFactory para crear instancias de HttpClient

        private static readonly JsonSerializerOptions JsonOptions = new() // Configuración de opciones de serialización JSON
        {
            PropertyNameCaseInsensitive = true
        };

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await BuildInicioModel();
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

        public async Task<IActionResult> Categorias()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await BuildCategoriasModel();
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

        public async Task<IActionResult> Caja()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await BuildCajaModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AbrirCaja(int montoInicial)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (montoInicial < 0)
            {
                TempData["CajaError"] = "El monto inicial no puede ser negativo.";
                return RedirectToAction(nameof(Caja));
            }

            var usuarioId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var response = await SendApiRequest(client => client.PostAsJsonAsync("api/caja/abrir", new CajaApiOpenRequest
            {
                IdUsuario = usuarioId,
                MontoInicial = montoInicial
            }));

            await SetCajaOperationMessage(response, "Caja abierta correctamente.", "No se pudo abrir la caja.");
            return RedirectToAction(nameof(Caja));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CerrarCaja(int montoFinal)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (montoFinal < 0)
            {
                TempData["CajaError"] = "El monto final no puede ser negativo.";
                return RedirectToAction(nameof(Caja));
            }

            var usuarioId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var response = await SendApiRequest(client => client.PostAsJsonAsync("api/caja/cerrar", new CajaApiCloseRequest
            {
                IdUsuario = usuarioId,
                MontoFinal = montoFinal
            }));

            await SetCajaOperationMessage(response, "Caja cerrada correctamente.", "No se pudo cerrar la caja.");
            return RedirectToAction(nameof(Caja));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarGasto(string descripcion, int monto)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (string.IsNullOrWhiteSpace(descripcion) || monto <= 0)
            {
                TempData["CajaError"] = "Ingresa una descripcion y un monto valido para el gasto.";
                return RedirectToAction(nameof(Caja));
            }

            var response = await SendApiRequest(client => client.PostAsJsonAsync("api/gastos", new GastoApiWriteRequest
            {
                Descripcion = descripcion.Trim(),
                Monto = monto,
                Categoria = null
            }));

            await SetCajaOperationMessage(response, "Gasto registrado correctamente.", "No se pudo registrar el gasto.");
            return RedirectToAction(nameof(Caja));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPedido(PedidoFormViewModel form)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!await HasCajaAbierta())
            {
                TempData["CajaError"] = "Abre la caja antes de cargar pedidos.";
                return RedirectToAction(nameof(Caja));
            }

            if (form.ProductoIds.Count == 0 || form.ProductoIds.Count != form.Cantidades.Count)
            {
                TempData["PedidosError"] = "Agrega al menos un producto al pedido.";
                return RedirectToAction(nameof(Pedidos));
            }

            if (form.TipoPedido == 1 && form.MetodoPagoId <= 0)
            {
                TempData["PedidosError"] = "Selecciona el metodo de pago para cobrar el pedido de mostrador.";
                return RedirectToAction(nameof(Pedidos));
            }

            if (form.TipoPedido == 2 && string.IsNullOrWhiteSpace(form.DireccionCliente))
            {
                TempData["PedidosError"] = "Ingresa la direccion para el delivery.";
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
                MetodoPagoId = form.TipoPedido == 1 ? form.MetodoPagoId : null,
                Detalles = detalles
            }));

            if (response.IsSuccessStatusCode)
            {
                var ticketPedidoId = await TryGetUltimoPedidoCreadoId();
                if (ticketPedidoId.HasValue)
                {
                    TempData["PedidoTicketId"] = ticketPedidoId.Value;
                }

                TempData["PedidosSuccess"] = form.TipoPedido == 1
                    ? "Pedido de mostrador cobrado correctamente."
                    : "Delivery creado como pendiente.";
            }
            else
            {
                await SetPedidoOperationError(response, "No se pudo crear el pedido.");
            }

            return RedirectToAction(nameof(Pedidos));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EntregarPedido(int id, int metodoPagoId)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id <= 0 || metodoPagoId <= 0)
            {
                TempData["PedidosError"] = "Selecciona un pedido y un metodo de pago validos.";
                return RedirectToAction(nameof(Pedidos));
            }

            if (!await HasCajaAbierta())
            {
                TempData["CajaError"] = "Abre la caja antes de cobrar un delivery.";
                return RedirectToAction(nameof(Caja));
            }

            var response = await SendApiRequest(client => client.PostAsJsonAsync("api/ventas", new VentaApiWriteRequest
            {
                PedidoId = id,
                MetodoPagoId = metodoPagoId
            }));

            if (response.IsSuccessStatusCode)
            {
                TempData["PedidosSuccess"] = $"Delivery #{id:D4} entregado y cobrado.";
            }
            else
            {
                await SetPedidoOperationError(response, "No se pudo entregar el pedido.");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCategoria(CategoriaFormViewModel form)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                TempData["CategoriesError"] = "Ingresa un nombre valido para la categoria.";
                return RedirectToAction(nameof(Categorias));
            }

            var response = await SendApiRequest(client => client.PostAsJsonAsync("api/categorias", new CategoriaApiWriteRequest
            {
                Nombre = form.Nombre.Trim()
            }));

            await SetCategoryOperationMessage(response, "Categoria creada correctamente.", "No se pudo crear la categoria.");
            return RedirectToAction(nameof(Categorias));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCategoria(CategoriaFormViewModel form)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid || form.Id <= 0)
            {
                TempData["CategoriesError"] = "Revisa los datos de la categoria.";
                return RedirectToAction(nameof(Categorias));
            }

            var response = await SendApiRequest(client => client.PutAsJsonAsync($"api/categorias/{form.Id}", new CategoriaApiWriteRequest
            {
                Nombre = form.Nombre.Trim()
            }));

            await SetCategoryOperationMessage(response, "Categoria actualizada correctamente.", "No se pudo actualizar la categoria.");
            return RedirectToAction(nameof(Categorias));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id <= 0)
            {
                TempData["CategoriesError"] = "No se encontro la categoria a eliminar.";
                return RedirectToAction(nameof(Categorias));
            }

            var response = await SendApiRequest(client => client.DeleteAsync($"api/categorias/{id}"));
            await SetCategoryOperationMessage(response, "Categoria eliminada correctamente.", "No se pudo eliminar la categoria.");
            return RedirectToAction(nameof(Categorias));
        }

        private async Task<AdminInicioViewModel> BuildInicioModel()
        {
            var model = new AdminInicioViewModel
            {
                Title = "Panel diario",
                Subtitle = "Apertura de caja, pedidos del dia y accesos rapidos.",
                ActiveSection = "Inicio",
                UserName = GetUserName(),
                SuccessMessage = TempData["CajaSuccess"] as string,
                ErrorMessage = TempData["CajaError"] as string,
                Navigation = BuildNavigation("Inicio", cajaAbierta: false)
            };

            try
            {
                var client = CreateApiClient();
                var caja = await GetCajaActual(client);
                ApplyCajaState(model, "Inicio", caja);

                if (model.CajaAbierta)
                {
                    var pedidos = await GetPedidos(client);
                    model.PedidosDelDia = pedidos
                        .Where(pedido => pedido.FechaHora.Date == DateTime.Today)
                        .OrderByDescending(pedido => pedido.FechaHora)
                        .Take(8)
                        .Select(MapPedido)
                        .ToList();

                    var productos = await GetProductos(client);
                    model.ProductosDisponibles = productos.Count(producto => producto.PuedeAgregarse);
                }
            }
            catch (HttpRequestException)
            {
                model.ErrorMessage = "No se pudo conectar con la API. Verifica que Bodinis.WebApi este corriendo.";
            }

            return model;
        }

        private async Task<AdminProductosViewModel> BuildProductosModel()
        {
            var model = new AdminProductosViewModel
            {
                Title = "Productos",
                Subtitle = "Gestiona el catalogo, categorias, disponibilidad y stock.",
                ActiveSection = "Productos",
                UserName = GetUserName(),
                Navigation = BuildNavigation("Productos", cajaAbierta: false),
                SuccessMessage = TempData["ProductsSuccess"] as string,
                ErrorMessage = TempData["ProductsError"] as string
            };

            try
            {
                var client = CreateApiClient();
                ApplyCajaState(model, "Productos", await GetCajaActual(client));

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

        private async Task<AdminCategoriasViewModel> BuildCategoriasModel()
        {
            var model = new AdminCategoriasViewModel
            {
                Title = "Categorias",
                Subtitle = "Administra las categorias del catalogo cuando el negocio necesite ajustar la oferta.",
                ActiveSection = "Categorias",
                UserName = GetUserName(),
                Navigation = BuildNavigation("Categorias", cajaAbierta: false),
                SuccessMessage = TempData["CategoriesSuccess"] as string,
                ErrorMessage = TempData["CategoriesError"] as string
            };

            try
            {
                var client = CreateApiClient();
                ApplyCajaState(model, "Categorias", await GetCajaActual(client));
                model.Categorias = await GetCategoriasAdmin(client);
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
                Subtitle = "Carga pedidos por categoria, cobra mostrador al instante y entrega delivery con control de caja.",
                ActiveSection = "Pedidos",
                UserName = GetUserName(),
                Navigation = BuildNavigation("Pedidos", cajaAbierta: false),
                SuccessMessage = TempData["PedidosSuccess"] as string,
                ErrorMessage = TempData["PedidosError"] as string
            };

            try
            {
                var client = CreateApiClient();
                ApplyCajaState(model, "Pedidos", await GetCajaActual(client));
                model.Categorias = await GetCategorias(client);
                model.MetodosPago = await GetMetodosPago(client);

                var pedidos = await GetPedidos(client);
                var ticketPedidoId = GetTempDataInt("PedidoTicketId");
                model.Pedidos = pedidos
                    .OrderByDescending(pedido => pedido.FechaHora)
                    .Take(14)
                    .Select(MapPedido)
                    .ToList();

                if (ticketPedidoId.HasValue)
                {
                    var pedidoParaTicket = pedidos.FirstOrDefault(pedido => pedido.Id == ticketPedidoId.Value);
                    if (pedidoParaTicket != null)
                    {
                        model.TicketParaImprimir = MapTicket(pedidoParaTicket);
                    }
                }

                if (model.CajaAbierta)
                {
                    var productos = await GetProductos(client);
                    model.Productos = productos.Where(producto => producto.PuedeAgregarse).ToList();
                }
            }
            catch (HttpRequestException)
            {
                model.ErrorMessage = "No se pudo conectar con la API. Verifica que Bodinis.WebApi este corriendo.";
            }

            return model;
        }

        private async Task<AdminCajaViewModel> BuildCajaModel()
        {
            var model = new AdminCajaViewModel
            {
                Title = "Caja",
                Subtitle = "Apertura, cierre y gastos de la jornada.",
                ActiveSection = "Caja",
                UserName = GetUserName(),
                Navigation = BuildNavigation("Caja", cajaAbierta: false),
                SuccessMessage = TempData["CajaSuccess"] as string,
                ErrorMessage = TempData["CajaError"] as string
            };

            try
            {
                var client = CreateApiClient();
                ApplyCajaState(model, "Caja", await GetCajaActual(client));

                if (model.CajaAbierta)
                {
                    model.Gastos = await GetGastosCajaActual(client);
                }
            }
            catch (HttpRequestException)
            {
                model.ErrorMessage = "No se pudo conectar con la API. Verifica que Bodinis.WebApi este corriendo.";
            }

            return model;
        }

        private async Task<List<PedidoApiResponse>> GetPedidos(HttpClient client)
        {
            var response = await client.GetAsync("api/pedidos");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                return new List<PedidoApiResponse>();
            }

            if (!response.IsSuccessStatusCode)
            {
                return new List<PedidoApiResponse>();
            }

            return await response.Content.ReadFromJsonAsync<List<PedidoApiResponse>>(JsonOptions) ?? new List<PedidoApiResponse>();
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

        private async Task<List<CategoriaAdminViewModel>> GetCategoriasAdmin(HttpClient client)
        {
            var response = await client.GetAsync("api/categorias");

            if (!response.IsSuccessStatusCode)
            {
                return new List<CategoriaAdminViewModel>();
            }

            var categorias = await response.Content.ReadFromJsonAsync<List<CategoriaApiResponse>>(JsonOptions) ?? new List<CategoriaApiResponse>();

            return categorias
                .OrderBy(categoria => categoria.Nombre)
                .Select(categoria => new CategoriaAdminViewModel
                {
                    Id = categoria.Id,
                    Nombre = categoria.Nombre,
                    CantidadProductos = categoria.CantidadProductos
                })
                .ToList();
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

        private async Task<List<GastoAdminViewModel>> GetGastosCajaActual(HttpClient client)
        {
            var response = await client.GetAsync("api/gastos/caja-actual");

            if (!response.IsSuccessStatusCode)
            {
                return new List<GastoAdminViewModel>();
            }

            var gastos = await response.Content.ReadFromJsonAsync<List<GastoApiResponse>>(JsonOptions) ?? new List<GastoApiResponse>();

            return gastos
                .OrderByDescending(gasto => gasto.FechaHora)
                .Select(gasto => new GastoAdminViewModel
                {
                    Id = gasto.Id,
                    FechaHora = gasto.FechaHora,
                    Descripcion = gasto.Descripcion,
                    Monto = gasto.Monto,
                    Categoria = gasto.Categoria,
                    CajaId = gasto.CajaId
                })
                .ToList();
        }

        private async Task<int?> TryGetUltimoPedidoCreadoId()
        {
            try
            {
                var pedidos = await GetPedidos(CreateApiClient());
                return pedidos.OrderByDescending(pedido => pedido.FechaHora).FirstOrDefault()?.Id;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        private async Task<CajaEstadoViewModel?> GetCajaActual(HttpClient client)
        {
            var response = await client.GetAsync("api/caja/actual");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var caja = await response.Content.ReadFromJsonAsync<CajaApiResponse>(JsonOptions);
            if (caja == null)
            {
                return null;
            }

            return new CajaEstadoViewModel
            {
                CajaId = caja.CajaId,
                FechaApertura = caja.FechaApertura,
                FechaCierre = caja.FechaCierre,
                MontoApertura = caja.MontoApertura,
                TotalVentas = caja.TotalVentas,
                TotalGastos = caja.TotalGastos,
                SaldoCalculado = caja.SaldoCalculado,
                MontoCierre = caja.MontoCierre,
                EstaAbierta = caja.EstaAbierta
            };
        }

        private async Task<bool> HasCajaAbierta()
        {
            try
            {
                var caja = await GetCajaActual(CreateApiClient());
                return caja?.EstaAbierta == true;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        private static void ApplyCajaState(AdminPageViewModel model, string activeSection, CajaEstadoViewModel? caja)
        {
            model.CajaActual = caja;
            model.CajaAbierta = caja?.EstaAbierta == true;
            model.Navigation = BuildNavigation(activeSection, model.CajaAbierta);
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

        private async Task SetCajaOperationMessage(HttpResponseMessage response, string successMessage, string fallbackErrorMessage)
        {
            if (response.IsSuccessStatusCode)
            {
                TempData["CajaSuccess"] = successMessage;
                return;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                TempData["CajaError"] = "Tu sesion vencio. Vuelve a iniciar sesion.";
                return;
            }

            TempData["CajaError"] = await ReadApiError(response, fallbackErrorMessage);
        }

        private async Task SetPedidoOperationError(HttpResponseMessage response, string fallbackErrorMessage)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                TempData["PedidosError"] = "Tu sesion vencio. Vuelve a iniciar sesion.";
                return;
            }

            TempData["PedidosError"] = await ReadApiError(response, fallbackErrorMessage);
        }

        private async Task SetCategoryOperationMessage(HttpResponseMessage response, string successMessage, string fallbackErrorMessage)
        {
            if (response.IsSuccessStatusCode)
            {
                TempData["CategoriesSuccess"] = successMessage;
                return;
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["CategoriesError"] = "Tu usuario no tiene permisos para realizar esta accion.";
                return;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                TempData["CategoriesError"] = "Tu sesion vencio. Vuelve a iniciar sesion.";
                return;
            }

            TempData["CategoriesError"] = await ReadApiError(response, fallbackErrorMessage);
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

        private static async Task<string> ReadApiError(HttpResponseMessage response, string fallback)
        {
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                return "No se pudo conectar con la API. Verifica que Bodinis.WebApi este corriendo.";
            }

            try
            {
                var body = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(body))
                {
                    return fallback;
                }

                using var json = JsonDocument.Parse(body);
                if (json.RootElement.TryGetProperty("error", out var error) && error.ValueKind == JsonValueKind.String)
                {
                    return error.GetString() ?? fallback;
                }
            }
            catch (JsonException)
            {
                return fallback;
            }

            return fallback;
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

        private int? GetTempDataInt(string key)
        {
            if (!TempData.TryGetValue(key, out var value) || value == null)
            {
                return null;
            }

            if (value is int number)
            {
                return number;
            }

            return int.TryParse(value.ToString(), out var parsed) ? parsed : null;
        }

        private static IReadOnlyList<AdminNavItemViewModel> BuildNavigation(string activeSection, bool cajaAbierta)
        {
            var items = new List<AdminNavItemViewModel>
            {
                new() { Label = "Inicio", Icon = "IN", Action = nameof(Index) },
                new()
                {
                    Label = "Pedidos",
                    Icon = "PE",
                    Action = nameof(Pedidos),
                    IsDisabled = !cajaAbierta,
                    DisabledReason = "Abre caja para cargar pedidos"
                },
                new() { Label = "Productos", Icon = "PR", Action = nameof(Productos) },
                new() { Label = "Categorias", Icon = "CA", Action = nameof(Categorias) },
                new() { Label = "Caja", Icon = "CJ", Action = nameof(Caja) }
            };

            foreach (var item in items)
            {
                item.IsActive = item.Label == activeSection;
            }

            return items;
        }

        private static PedidoAdminViewModel MapPedido(PedidoApiResponse pedido)
        {
            return new PedidoAdminViewModel
            {
                Id = pedido.Id,
                FechaHora = pedido.FechaHora,
                TipoPedido = MapTipoPedido(pedido.TipoPedido),
                Estado = MapEstadoPedido(pedido.Estado),
                NombreCliente = pedido.NombreCliente,
                TelefonoCliente = pedido.TelefonoCliente,
                DireccionCliente = pedido.DireccionCliente,
                MetodoPago = pedido.MetodoPago,
                Total = pedido.Total,
                Detalles = pedido.Detalles.Select(MapPedidoDetalle).ToList()
            };
        }

        private static PedidoTicketViewModel MapTicket(PedidoApiResponse pedido)
        {
            return new PedidoTicketViewModel
            {
                Id = pedido.Id,
                FechaHora = pedido.FechaHora,
                TipoPedido = MapTipoPedido(pedido.TipoPedido),
                Estado = MapEstadoPedido(pedido.Estado),
                NombreCliente = pedido.NombreCliente,
                TelefonoCliente = pedido.TelefonoCliente,
                DireccionCliente = pedido.DireccionCliente,
                MetodoPago = pedido.MetodoPago,
                Total = pedido.Total,
                Detalles = pedido.Detalles.Select(MapPedidoDetalle).ToList()
            };
        }

        private static PedidoDetalleAdminViewModel MapPedidoDetalle(PedidoDetalleApiResponse detalle)
        {
            return new PedidoDetalleAdminViewModel
            {
                ProductoId = detalle.ProductoId,
                NombreProducto = detalle.NombreProducto,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = detalle.PrecioUnitario,
                Subtotal = detalle.Subtotal
            };
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
                2 => "Pendiente",
                3 => "Entregado",
                4 => "Cancelado",
                _ => "Sin estado"
            };
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
            public int CantidadProductos { get; set; }
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
            public string? NombreCliente { get; set; }
            public string? TelefonoCliente { get; set; }
            public string? DireccionCliente { get; set; }
            public string? MetodoPago { get; set; }
            public int Total { get; set; }
            public List<PedidoDetalleApiResponse> Detalles { get; set; } = new();
        }

        private class PedidoDetalleApiResponse
        {
            public int ProductoId { get; set; }
            public string NombreProducto { get; set; } = string.Empty;
            public int Cantidad { get; set; }
            public int PrecioUnitario { get; set; }
            public int Subtotal { get; set; }
        }

        private class CajaApiResponse
        {
            public int CajaId { get; set; }
            public DateTime FechaApertura { get; set; }
            public DateTime? FechaCierre { get; set; }
            public int MontoApertura { get; set; }
            public int TotalVentas { get; set; }
            public int TotalGastos { get; set; }
            public int SaldoCalculado { get; set; }
            public int MontoCierre { get; set; }
            public bool EstaAbierta { get; set; }
        }

        private class GastoApiResponse
        {
            public int Id { get; set; }
            public DateTime FechaHora { get; set; }
            public string Descripcion { get; set; } = string.Empty;
            public int Monto { get; set; }
            public string? Categoria { get; set; }
            public int CajaId { get; set; }
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
            public int? MetodoPagoId { get; set; }
            public List<PedidoDetalleApiRequest> Detalles { get; set; } = new();
        }

        private class PedidoDetalleApiRequest
        {
            public int ProductoId { get; set; }
            public int Cantidad { get; set; }
        }

        private class VentaApiWriteRequest
        {
            public int PedidoId { get; set; }
            public int MetodoPagoId { get; set; }
        }

        private class CajaApiOpenRequest
        {
            public int IdUsuario { get; set; }
            public int MontoInicial { get; set; }
        }

        private class CajaApiCloseRequest
        {
            public int IdUsuario { get; set; }
            public int MontoFinal { get; set; }
        }

        private class GastoApiWriteRequest
        {
            public string Descripcion { get; set; } = string.Empty;
            public int Monto { get; set; }
            public string? Categoria { get; set; }
        }

        private class CategoriaApiWriteRequest
        {
            public string Nombre { get; set; } = string.Empty;
        }
    }
}
