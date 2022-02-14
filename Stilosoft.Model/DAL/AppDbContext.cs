using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Model.DAL
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {

        }

        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Cita> citas { get; set; }
        public DbSet<Empleado> empleados { get; set; }
        public DbSet<Proveedor> proveedors { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<EstadoCita> estadoCitas { get; set; }
        public DbSet<Servicio> servicios { get; set; }
        public DbSet<DetalleCitaServicios> detalleCitas { get; set; }
        public DbSet<DetalleEmpleadoServicios> detalleEmpleados { get; set;}
}
}
