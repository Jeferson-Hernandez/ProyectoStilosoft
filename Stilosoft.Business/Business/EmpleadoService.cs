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
        public async Task<Empleado> ObtenerEmpleadoPorId(string id)
        {
            return await _context.empleados.FirstOrDefaultAsync(s => s.EmpleadoId == id);
        }
        public async Task EditarEmpleado(Empleado empleado)
        {
            _context.Update(empleado);
            await _context.SaveChangesAsync();
        }

        //Detalle empleado servicios
        public async Task<IEnumerable<DetalleEmpleadoServicios>> ObtenerListaServiciosEmpleado(string id)
        {
            return await _context.detalleEmpleados.Where(em => em.EmpleadoId == id).Include(e => e.Empleado).Include(s => s.Servicio).ToListAsync();
        }
        public async Task<List<DetalleEmpleadoServicios>> ListaEmpleadoServicios(string id)
        {
            return await _context.detalleEmpleados.Where(em => em.EmpleadoId == id).Include(e => e.Empleado).Include(s => s.Servicio).ToListAsync();
        }
        public async Task<DetalleEmpleadoServicios> ObtenerDetallePorId(int id)
        {
            return await _context.detalleEmpleados.FirstOrDefaultAsync(s => s.EmpleadoServicioId == id);
        }
        public async Task EliminarEmpleadoServicio(int id)
        {
            var EmpleadoServicio = await ObtenerDetallePorId(id);
            _context.Remove(EmpleadoServicio);
            await _context.SaveChangesAsync();
        }

        //Empleado agenda 
        public async Task<IEnumerable<EmpleadoAgenda>> ObtenerListaAgendaEmpleado()
        {
            return await _context.empleadoAgendas.Include(e => e.Empleado).ToListAsync();
        }
        public async Task GuardarEmpleadoAgenda(EmpleadoAgenda empleadoAgenda)
        {
            _context.Add(empleadoAgenda);
            await _context.SaveChangesAsync();
        }
    }
}
