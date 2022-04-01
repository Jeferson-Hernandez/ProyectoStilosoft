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
    public class ServicioService : IServicioService
    {
        private readonly AppDbContext _context;

        public ServicioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Servicio>> ObtenerListaServicios()
        {
            return await _context.servicios.ToListAsync();
        }
        public async Task<IEnumerable<Servicio>> ObtenerServiciosLanding()
        {
            return await _context.servicios.Where(s => s.EstadoLanding == true).ToListAsync();
        }

        public async Task<List<Servicio>> ObtenerListaServiciosEstado()
        {
            return await _context.servicios.Where(s => s.Estado == true).ToListAsync();
        }
        public async Task<Servicio> ObtenerServicioPorId(int id)
        {
            return await _context.servicios.FirstOrDefaultAsync(s => s.ServicioId == id);
        }

        public async Task<Servicio> NombreServicioExiste(string nombre)
        {
            return await _context.servicios.FirstOrDefaultAsync(n => n.Nombre == nombre);
        }

        public async Task GuardarServicio(Servicio servicio)
        {
            _context.Add(servicio);
            await _context.SaveChangesAsync();
        }

        public async Task EditarServicio(Servicio servicio)
        {
            _context.Update(servicio);
            await _context.SaveChangesAsync();
        }
    }
}
