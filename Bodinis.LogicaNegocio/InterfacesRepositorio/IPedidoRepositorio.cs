using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IPedidoRepositorio
    {
        void Add(Pedido pedido);
        IEnumerable<Pedido> GetAll();
        Pedido? GetById(int id); // ? indica que el método puede retornar null
        void Update(Pedido pedido);
    }
}
