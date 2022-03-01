using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Business.Abstract
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<DetalleEmpleadoServicios>> ObtenerListaServiciosEmpleado();
        Task GuardarEmpleado(Empleado empleado);
        Task<IEnumerable<Empleado>> ObtenerListaEmpleados();
        Task<Empleado> ObtenerEmpleadoPorId(string id);
        Task EditarEmpleado(Empleado empleado);
    }
}
