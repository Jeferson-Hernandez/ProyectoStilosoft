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
        //Empleado
        Task<IEnumerable<DetalleEmpleadoServicios>> ObtenerListaServiciosEmpleado(string id);
        Task GuardarEmpleado(Empleado empleado);
        Task<IEnumerable<Empleado>> ObtenerListaEmpleados();
        Task<IEnumerable<Empleado>> ObtenerListaEmpleadosEstado();
        Task<Empleado> ObtenerEmpleadoPorId(string id);
        //Empleado detalle
        Task EditarEmpleado(Empleado empleado);
        Task<List<DetalleEmpleadoServicios>> ListaEmpleadoServicios(string id);
        Task EliminarEmpleadoServicio(int id);
        //Empleado agenda
        Task<IEnumerable<EmpleadoNovedad>> ObtenerListaNovedades();
        Task GuardarEmpleadoNovedad(EmpleadoNovedad empleadoNovedad);
    }
}
