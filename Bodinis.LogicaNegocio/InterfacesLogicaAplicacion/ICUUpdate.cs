using System;

namespace Bodinis.LogicaNegocio.InterfacesLogicaAplicacion
{
    public interface ICUUpdate<T>
    {
        void Execute(int id,T obj);
    }
}
