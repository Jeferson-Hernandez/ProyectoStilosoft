using Microsoft.AspNetCore.Identity;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedUsers(builder);
            this.SeedRoles(builder);
            this.SeedUserRoles(builder);
            this.SeedEstadosCita(builder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            IdentityUser user = new IdentityUser()
            {
                Id = "b74ddd14-6340-4840-95c2-db12554843e5",
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                LockoutEnabled = false,
                PhoneNumber = "1234567890"
            };

            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "Admin123456");

            builder.Entity<IdentityUser>().HasData(user);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Administrador", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Id = "c7b013f0-5201-4317-abd8-c211f91b7330", Name = "Cliente", ConcurrencyStamp = "2", NormalizedName = "Client" }
                );
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", UserId = "b74ddd14-6340-4840-95c2-db12554843e5" }
                );
        }

        private void SeedEstadosCita(ModelBuilder builder)
        {
            builder.Entity<EstadoCita>().HasData(
                new EstadoCita
                {
                    EstadoCitaId = 1,
                    Nombre = "Confirmada"
                },
                new EstadoCita
                {
                    EstadoCitaId = 2,
                    Nombre = "En curso"
                },
                new EstadoCita
                {
                    EstadoCitaId = 3,
                    Nombre = "Finalizada"
                }
                );
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
        public object Proveedor { get; set; }
    }
}
