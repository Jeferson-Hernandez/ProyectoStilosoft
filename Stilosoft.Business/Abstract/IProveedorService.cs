using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Business.Abstract
{
    public interface IProveedorService
    {
        Task<IEnumerable<Proveedor>> ObtenerListaProveedor();
        Task<IEnumerable<Proveedor>> ObtenerListaProveedorEstado();
        Task<Proveedor> ObtenerProveedorPorId(int id);
        Task GuardarProveedor(Proveedor proveedor);
        Task EditarProveedor(Proveedor proveedor);
        Task EliminarProveedor(int id);
        Task<Proveedor> NitProveedorExiste(string Nit);
    }
}
