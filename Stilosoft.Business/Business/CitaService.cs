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
    public class CitaService : ICitaService
    {
        private readonly AppDbContext _context;

        public CitaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cita>> ObtenerListaCitas()
        {
            return await _context.citas.Include(c => c.Cliente).Include(e => e.EstadoCita).Include(em => em.Empleado).Include(s => s.Servicio).ToListAsync();
        }
        public async Task<IEnumerable<Cita>> ObtenerListaCitasCliente(string clienteId)
        {
            return await _context.citas.Where(c => c.ClienteId == clienteId).Where(es => es.EstadoCitaId == 1).Include(c => c.Cliente).Include(e => e.EstadoCita).Include(em => em.Empleado).Include(s => s.Servicio).ToListAsync();
        }
        public async Task<Cita> ObtenerCitaPorId(int id)
        {
            return await _context.citas.Include(c => c.Cliente).Include(e => e.EstadoCita).Include(em => em.Empleado).Include(s => s.Servicio).FirstOrDefaultAsync(c => c.CitaId == id);
        }
        public async Task GuardarCita(Cita cita)
        {
            _context.Add(cita);
            await _context.SaveChangesAsync();
        }
        public async Task EditarCita(Cita cita)
        {
            _context.Update(cita);
            await _context.SaveChangesAsync();
        }
    }
}
