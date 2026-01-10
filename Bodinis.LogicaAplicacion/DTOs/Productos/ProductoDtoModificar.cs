

namespace Bodinis.LogicaAplicacion.DTOs.Productos
{
    public record ProductoDtoModificar(
                                      int Id,
                                      string NombreProducto,
                                      int Precio,
                                      bool Disponible,
                                      int Stock,
                                      int CategoriaId
                                    )   
    {
    }
}
