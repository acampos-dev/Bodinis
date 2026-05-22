using Bodinis.LogicaAplicacion.DTOs.Ventas;
using Bodinis.LogicaAplicacion.Mappers;
using Bodinis.LogicaNegocio.Excepciones;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesRepositorio;

namespace Bodinis.LogicaAplicacion.CasosDeUso.Ventas
{
    public class GetVentaById : ICUGetById<VentaDtoListado>
    {
        private readonly IRepositorioVenta _repoVenta;

        public GetVentaById(IRepositorioVenta repoVenta)
        {
            _repoVenta = repoVenta;
        }

        public VentaDtoListado Execute(int id)
        {
            var venta = _repoVenta.GetById(id)
                ?? throw new DatosInvalidosException("Venta no encontrada");

            return VentaMapper.ToDto(venta);
        }
    }
}
