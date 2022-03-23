﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Stilosoft.Model.DAL;

namespace Stilosoft.Model.Migrations
{
    [DbContext(typeof(AppDbContext))]
<<<<<<< HEAD:Stilosoft.Model/Migrations/20220315151255_inicial.Designer.cs
    [Migration("20220315151255_inicial")]
=======
    [Migration("20220320194431_inicial")]
>>>>>>> master:Stilosoft.Model/Migrations/20220320194431_inicial.Designer.cs
    partial class inicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "fab4fac1-c546-41de-aebc-a14da6895711",
                            ConcurrencyStamp = "1",
                            Name = "Administrador",
                            NormalizedName = "Administrador"
                        },
                        new
                        {
                            Id = "c7b013f0-5201-4317-abd8-c211f91b7330",
                            ConcurrencyStamp = "2",
                            Name = "Cliente",
                            NormalizedName = "Cliente"
                        },
                        new
                        {
                            Id = "cub049f0-5302-2217-abc1-c123f55n7229",
                            ConcurrencyStamp = "3",
                            Name = "Empleado",
                            NormalizedName = "Empleado"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = "b74ddd14-6340-4840-95c2-db12554843e5",
                            AccessFailedCount = 0,
<<<<<<< HEAD:Stilosoft.Model/Migrations/20220315151255_inicial.Designer.cs
                            ConcurrencyStamp = "83b5d91c-54ee-4b61-9114-3923bba50f9d",
=======
                            ConcurrencyStamp = "4bb18c22-2da3-4626-9f56-bc5716ff2674",
>>>>>>> master:Stilosoft.Model/Migrations/20220320194431_inicial.Designer.cs
                            Email = "admin@gmail.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@GMAIL.COM",
                            NormalizedUserName = "ADMIN@GMAIL.COM",
<<<<<<< HEAD:Stilosoft.Model/Migrations/20220315151255_inicial.Designer.cs
                            PasswordHash = "AQAAAAEAACcQAAAAECiuCFYSMTJItwhWJuGB2tes7jUflaUlRLH5cwp1kkYlTsy30Qws7fAQ315JiCsRYQ==",
                            PhoneNumber = "1234567890",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "dc9fa9cd-acb0-4930-a7db-8440324056fd",
=======
                            PasswordHash = "AQAAAAEAACcQAAAAEFcrRCTCtTBLnRo4IT8wDDaSqB3EAl6Zz/szHnyOqoOah7NpCsCFGng/vMzyTo09ZA==",
                            PhoneNumber = "1234567890",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "080faa0e-6105-4ac0-9d56-c2fee538dca3",
>>>>>>> master:Stilosoft.Model/Migrations/20220320194431_inicial.Designer.cs
                            TwoFactorEnabled = false,
                            UserName = "admin@gmail.com"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");

                    b.HasData(
                        new
                        {
                            UserId = "b74ddd14-6340-4840-95c2-db12554843e5",
                            RoleId = "fab4fac1-c546-41de-aebc-a14da6895711"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.AgendaOcupada", b =>
                {
                    b.Property<int>("AgendaOcupadaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmpleadoAgendaId")
                        .HasColumnType("int");

                    b.Property<string>("HoraFin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoraInicio")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AgendaOcupadaId");

                    b.HasIndex("EmpleadoAgendaId");

                    b.ToTable("agendaOcupadas");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Cita", b =>
                {
                    b.Property<int>("CitaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClienteId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("EstadoCitaId")
                        .HasColumnType("int");

                    b.Property<string>("Fecha")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Hora")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<long>("Total")
                        .HasColumnType("bigint");

                    b.HasKey("CitaId");

                    b.HasIndex("ClienteId");

                    b.HasIndex("EstadoCitaId");

                    b.ToTable("citas");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Cliente", b =>
                {
                    b.Property<string>("ClienteId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Apellido")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Documento")
                        .HasColumnType("nvarchar(15)");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Numero")
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("ClienteId");

                    b.ToTable("clientes");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.DetalleCitaServicios", b =>
                {
                    b.Property<int>("CitaServicioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CitaId")
                        .HasColumnType("int");

                    b.Property<string>("EmpleadoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ServicioId")
                        .HasColumnType("int");

                    b.HasKey("CitaServicioId");

                    b.HasIndex("CitaId")
                        .IsUnique();

                    b.HasIndex("EmpleadoId");

                    b.HasIndex("ServicioId");

                    b.ToTable("detalleCitas");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.DetalleEmpleadoServicios", b =>
                {
                    b.Property<int>("EmpleadoServicioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmpleadoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ServicioId")
                        .HasColumnType("int");

                    b.HasKey("EmpleadoServicioId");

                    b.HasIndex("EmpleadoId");

                    b.HasIndex("ServicioId");

                    b.ToTable("detalleEmpleados");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Empleado", b =>
                {
                    b.Property<string>("EmpleadoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Apellidos")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Documento")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("Date");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("EmpleadoId");

                    b.ToTable("empleados");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.EmpleadoAgenda", b =>
                {
                    b.Property<int>("EmpleadoAgendaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmpleadoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Fecha")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoraFin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoraInicio")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmpleadoAgendaId");

                    b.HasIndex("EmpleadoId");

                    b.ToTable("empleadoAgendas");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.EstadoCita", b =>
                {
                    b.Property<int>("EstadoCitaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("EstadoCitaId");

                    b.ToTable("estadoCitas");

                    b.HasData(
                        new
                        {
                            EstadoCitaId = 1,
                            Nombre = "Confirmada"
                        },
                        new
                        {
                            EstadoCitaId = 2,
                            Nombre = "En curso"
                        },
                        new
                        {
                            EstadoCitaId = 3,
                            Nombre = "Finalizada"
                        },
                        new
                        {
                            EstadoCitaId = 4,
                            Nombre = "Cancelada"
                        });
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Proveedor", b =>
                {
                    b.Property<int>("ProveedorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Celular")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Contacto")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<string>("Nit")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ProveedorId");

                    b.ToTable("proveedors");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Servicio", b =>
                {
                    b.Property<int>("ServicioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Costo")
                        .HasColumnType("bigint");

                    b.Property<int>("Duracion")
                        .HasColumnType("int");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ServicioId");

                    b.ToTable("servicios");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Usuario", b =>
                {
                    b.Property<string>("UsuarioId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Documento")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Rol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UsuarioId");

                    b.ToTable("usuarios");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.AgendaOcupada", b =>
                {
                    b.HasOne("Stilosoft.Model.Entities.EmpleadoAgenda", "EmpleadoAgenda")
                        .WithMany()
                        .HasForeignKey("EmpleadoAgendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmpleadoAgenda");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Cita", b =>
                {
                    b.HasOne("Stilosoft.Model.Entities.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId");

                    b.HasOne("Stilosoft.Model.Entities.EstadoCita", "EstadoCita")
                        .WithMany()
                        .HasForeignKey("EstadoCitaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("EstadoCita");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Cliente", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdentityUser")
                        .WithMany()
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdentityUser");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.DetalleCitaServicios", b =>
                {
                    b.HasOne("Stilosoft.Model.Entities.Cita", "Cita")
                        .WithOne("DetalleCitaServicios")
                        .HasForeignKey("Stilosoft.Model.Entities.DetalleCitaServicios", "CitaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Stilosoft.Model.Entities.Empleado", "Empleado")
                        .WithMany("DetalleCitaServicios")
                        .HasForeignKey("EmpleadoId");

                    b.HasOne("Stilosoft.Model.Entities.Servicio", "Servicio")
                        .WithMany("DetalleCitaServicios")
                        .HasForeignKey("ServicioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cita");

                    b.Navigation("Empleado");

                    b.Navigation("Servicio");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.DetalleEmpleadoServicios", b =>
                {
                    b.HasOne("Stilosoft.Model.Entities.Empleado", "Empleado")
                        .WithMany("DetalleEmpleadoServicios")
                        .HasForeignKey("EmpleadoId");

                    b.HasOne("Stilosoft.Model.Entities.Servicio", "Servicio")
                        .WithMany("DetalleEmpleadoServicios")
                        .HasForeignKey("ServicioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Empleado");

                    b.Navigation("Servicio");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Empleado", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdentityUser")
                        .WithMany()
                        .HasForeignKey("EmpleadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdentityUser");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.EmpleadoAgenda", b =>
                {
                    b.HasOne("Stilosoft.Model.Entities.Empleado", "Empleado")
                        .WithMany()
                        .HasForeignKey("EmpleadoId");

                    b.Navigation("Empleado");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Usuario", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdentityUser")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdentityUser");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Cita", b =>
                {
                    b.Navigation("DetalleCitaServicios");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Empleado", b =>
                {
                    b.Navigation("DetalleCitaServicios");

                    b.Navigation("DetalleEmpleadoServicios");
                });

            modelBuilder.Entity("Stilosoft.Model.Entities.Servicio", b =>
                {
                    b.Navigation("DetalleCitaServicios");

                    b.Navigation("DetalleEmpleadoServicios");
                });
#pragma warning restore 612, 618
        }
    }
}
