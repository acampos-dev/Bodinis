
namespace Bodinis.LogicaAplicacion.DTOs.Productos
{
    public record ProductoDtoAlta(
                                      string NombreProducto,
                                      int Precio,
                                      bool Disponible,
                                      int Stock,
                                      int CategoriaId
                                    )
    {
    }
}
