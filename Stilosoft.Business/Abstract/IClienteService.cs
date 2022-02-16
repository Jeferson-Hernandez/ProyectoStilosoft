using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Business.Abstract
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ObtenerListaClientes();
        Task GuardarCliente(Cliente cliente);
        Task EditarCliente(Cliente cliente);
        Task<Cliente> ObtenerClientePorId(string id);
        Task EliminarCliente(string id);
    }
}
