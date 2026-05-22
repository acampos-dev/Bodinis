using Bodinis.LogicaNegocio.Excepciones;

namespace Bodinis.LogicaNegocio.Entidades
{
    public class Gasto
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Monto { get; set; }
        public string? Categoria { get; set; }
        public int CajaId { get; set; }
        public Caja? Caja { get; set; }

        public Gasto() { } // Constructor para EF

        public Gasto(DateTime fechaHora, string descripcion, int monto, string? categoria, Caja caja)
        {
            FechaHora = fechaHora;
            Descripcion = descripcion;
            Monto = monto;
            Categoria = categoria;
            Caja = caja;
            CajaId = caja.Id;
            Validar();
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Descripcion))
            {
                throw new DatosInvalidosException("La descripcion del gasto es obligatoria");
            }

            if (Monto <= 0)
            {
                throw new DatosInvalidosException("El monto del gasto debe ser mayor a cero");
            }
        }
    }
}
