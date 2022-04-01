using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Business.Abstract
{
    public interface IServicioService
    {
        Task<IEnumerable<Servicio>> ObtenerListaServicios();
        Task<IEnumerable<Servicio>> ObtenerServiciosLanding();
        Task<List<Servicio>> ObtenerListaServiciosEstado();
        Task<Servicio> NombreServicioExiste(string nombre);
        Task<Servicio> ObtenerServicioPorId(int id);
        Task GuardarServicio(Servicio servicio);
        Task EditarServicio(Servicio servicio);
    }
}
