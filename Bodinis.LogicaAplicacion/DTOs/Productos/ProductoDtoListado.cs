

namespace Bodinis.LogicaAplicacion.DTOs.Productos
{
    public record ProductoDtoListado(
                                      int Id,
                                      string NombreProducto,
                                      int Precio,
                                      bool Disponible,
                                      int Stock,
                                      string Categoria
                                    )
    {
    }
}
