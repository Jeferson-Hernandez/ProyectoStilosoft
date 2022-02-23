using Microsoft.EntityFrameworkCore;
using Stilosoft.Business.Abstract;
using Stilosoft.Model.DAL;
using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Business.Business
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly AppDbContext _context;
        public EmpleadoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empleado>> ObtenerListaEmpleados()
        {
            return await _context.empleados.ToListAsync();
        }

        public async Task GuardarEmpleado(Empleado empleado)
        {
            _context.Add(empleado);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Empleado>> ObtenerListaEmpleadosEstado()
        {
            return await _context.empleados.Where(s => s.Estado == true).ToListAsync();
        }

        //Detalle empleado servicios
        public async Task<IEnumerable<DetalleEmpleadoServicios>> ObtenerListaServiciosEmpleado()
        {
            return await _context.detalleEmpleados.Include(e => e.Empleado).Include(s => s.Servicio).ToListAsync();
        }
    }
}
