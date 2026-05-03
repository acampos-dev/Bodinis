using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioCaja
    {
        Caja? GetCajaAbierta();
        void AbrirCaja(Caja caja);
        void CerrarCaja(Caja caja);
    }
}
