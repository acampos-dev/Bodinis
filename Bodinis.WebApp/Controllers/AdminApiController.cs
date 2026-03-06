using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApp.Controllers
{
    [ApiController]
    [Route("admin-api")]
    public class AdminApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AdminApiController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await ForwardPostAsync("api/auth/login", request, includeAuth: false);
            return await ToActionResultAsync(response);
        }

        [HttpGet("ventas/resumen-dia")]
        public async Task<IActionResult> ResumenDia([FromQuery] string? fecha = null)
        {
            var path = string.IsNullOrWhiteSpace(fecha)
                ? "api/ventas/resumen-dia"
                : $"api/ventas/resumen-dia?fecha={Uri.EscapeDataString(fecha)}";

            var response = await ForwardGetAsync(path);
            return await ToActionResultAsync(response);
        }

        [HttpGet("ventas/resumen-mes")]
        public async Task<IActionResult> ResumenMes([FromQuery] int? anio = null, [FromQuery] int? mes = null)
        {
            var query = new List<string>();
            if (anio.HasValue) query.Add($"anio={anio.Value}");
            if (mes.HasValue) query.Add($"mes={mes.Value}");

            var path = "api/ventas/resumen-mes";
            if (query.Count > 0)
            {
                path += "?" + string.Join("&", query);
            }

            var response = await ForwardGetAsync(path);
            return await ToActionResultAsync(response);
        }

        [HttpGet("productos")]
        public async Task<IActionResult> Productos()
        {
            var response = await ForwardGetAsync("api/productos");
            return await ToActionResultAsync(response);
        }

        [HttpPost("pedidos")]
        public async Task<IActionResult> CrearPedido([FromBody] CrearPedidoRequest request)
        {
            var response = await ForwardPostAsync("api/pedidos", request, includeAuth: true);
            return await ToActionResultAsync(response);
        }

        private async Task<HttpResponseMessage> ForwardGetAsync(string path)
        {
            var client = CreateClient();
            using var req = new HttpRequestMessage(HttpMethod.Get, path);
            CopyAuthorization(req);
            return await client.SendAsync(req);
        }

        private async Task<HttpResponseMessage> ForwardPostAsync<T>(string path, T body, bool includeAuth = true)
        {
            var client = CreateClient();
            using var req = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };

            if (includeAuth)
            {
                CopyAuthorization(req);
            }

            return await client.SendAsync(req);
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient("BodinisApi");
            return client;
        }

        private void CopyAuthorization(HttpRequestMessage req)
        {
            var auth = Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(auth))
            {
                return;
            }

            if (AuthenticationHeaderValue.TryParse(auth, out var header))
            {
                req.Headers.Authorization = header;
            }
        }

        private static async Task<IActionResult> ToActionResultAsync(HttpResponseMessage response)
        {
            var raw = await response.Content.ReadAsStringAsync();
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/json";

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                ContentType = contentType,
                Content = raw
            };
        }

        public record LoginRequest(string Email, string Password);

        public record CrearPedidoRequest(
            int UsuarioId,
            string TipoPedido,
            IReadOnlyCollection<CrearPedidoItemRequest> Items);

        public record CrearPedidoItemRequest(int ProductoId, int Cantidad);
    }
}
