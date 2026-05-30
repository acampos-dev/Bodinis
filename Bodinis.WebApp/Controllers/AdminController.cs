using Bodinis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bodinis.WebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var model = new AdminPageViewModel
            {
                Title = "Panel administrativo",
                Subtitle = "Elegí una sección para empezar a gestionar Bodinis.",
                ActiveSection = "Inicio",
                Navigation = BuildNavigation("Inicio")
            };

            return View(model);
        }

        public IActionResult Productos()
        {
            var model = new AdminProductosViewModel
            {
                Title = "Productos",
                Subtitle = "Gestioná el catálogo, la disponibilidad y el stock.",
                ActiveSection = "Productos",
                Navigation = BuildNavigation("Productos"),
                Productos = new List<ProductoAdminViewModel>
                {
                    new() { Id = 1, Nombre = "Milanesa napolitana", Categoria = "Minutas", Precio = 360, Stock = 18, Disponible = true },
                    new() { Id = 2, Nombre = "Tarta de jamón y queso", Categoria = "Tartas", Precio = 180, Stock = 9, Disponible = true },
                    new() { Id = 3, Nombre = "Ensalada rusa", Categoria = "Guarniciones", Precio = 140, Stock = 0, Disponible = false },
                    new() { Id = 4, Nombre = "Pollo al horno", Categoria = "Platos calientes", Precio = 420, Stock = 7, Disponible = true }
                }
            };

            return View(model);
        }

        private static IReadOnlyList<AdminNavItemViewModel> BuildNavigation(string activeSection)
        {
            var items = new List<AdminNavItemViewModel>
            {
                new() { Label = "Inicio", Icon = "⌂", Action = nameof(Index) },
                new() { Label = "Productos", Icon = "▦", Action = nameof(Productos) },
                new() { Label = "Categorías", Icon = "◇", Action = nameof(Index) },
                new() { Label = "Pedidos", Icon = "☰", Action = nameof(Index) },
                new() { Label = "Ventas", Icon = "$", Action = nameof(Index) },
                new() { Label = "Caja", Icon = "□", Action = nameof(Index) }
            };

            foreach (var item in items)
            {
                item.IsActive = item.Label == activeSection;
            }

            return items;
        }
    }
}
