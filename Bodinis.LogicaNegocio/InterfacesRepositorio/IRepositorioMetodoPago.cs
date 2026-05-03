using Bodinis.LogicaNegocio.Entidades;

namespace Bodinis.LogicaNegocio.InterfacesRepositorio
{
    public interface IRepositorioMetodoPago
    {
        void Add(MetodoPago metodoPago);
        IEnumerable<MetodoPago> GetAll();
        MetodoPago? GetById(int id); // ? indica que el método puede retornar null
         void Update(MetodoPago metodoPago);
        void Delete(int id);
    }
}
