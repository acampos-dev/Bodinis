namespace Bodinis.LogicaAplicacion.DTOs.Gastos
{
    public record GastoDtoListado(int Id,
                                  DateTime FechaHora,
                                  string Descripcion,
                                  int Monto,
                                  string? Categoria,
                                  int CajaId)
    {
    }
}
