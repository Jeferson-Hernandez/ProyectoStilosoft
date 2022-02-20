using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Business.Abstract
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> ObtenerListaUsuarios();
        Task GuardarUsuario(Usuario usuario1);
        Task EditarUsuario(Usuario usuario1);
        Task<Usuario> ObtenerUsuarioPorId(string id);
    }
}
