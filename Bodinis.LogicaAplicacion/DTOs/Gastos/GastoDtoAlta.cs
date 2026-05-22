namespace Bodinis.LogicaAplicacion.DTOs.Gastos
{
    public record GastoDtoAlta(string Descripcion,
                               int Monto,
                               string? Categoria)
    {
    }
}
