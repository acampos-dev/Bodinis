using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bodinis.LogicaAplicacion.DTOs.Usuarios
{
    public record LoginDtoResponse(int UsuarioId,
                                  string NombreCompleto,
                                  string Email,
                                  string UserName,
                                  string RolUsuario,
                                  string Token)
    {
    }
}
