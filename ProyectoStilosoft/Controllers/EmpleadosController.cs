using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoStilosoft.ViewModels.Empleados;
using Stilosoft.Business.Abstract;
using Stilosoft.Model.DAL;
using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmpleadoService _empleado;
        private readonly IServicioService _servicio;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmpleadosController(AppDbContext context, IEmpleadoService empleado, IServicioService servicio, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _empleado = empleado;
            _servicio = servicio;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _empleado.ObtenerListaEmpleados());
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            EmpleadoViewModel empleado = new();
            empleado.Servicios = await _servicio.ObtenerListaServiciosEstado();
            return View(empleado);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(EmpleadoViewModel serviciosEmpleado)
        {
            if (ModelState.IsValid)
            {
                using(var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        IdentityUser identityUser = new()
                        {
                            UserName = serviciosEmpleado.Email,
                            Email = serviciosEmpleado.Email
                        };

                        var resultado = await _userManager.CreateAsync(identityUser, serviciosEmpleado.Password);
                        if (resultado.Succeeded)
                        {

                            var usuario = await _userManager.FindByEmailAsync(serviciosEmpleado.Email);
                            await _userManager.AddToRoleAsync(usuario, "Empleado");
                            Empleado empleado = new()
                            {
                                Nombre = serviciosEmpleado.Nombre,
                                Apellidos = serviciosEmpleado.Apellidos,
                                Documento = serviciosEmpleado.Documento,
                                FechaNacimiento = serviciosEmpleado.FechaNacimiento.Date,
                                Estado = true
                            };
                            _context.Add(empleado);
                            _context.SaveChanges();

                            if (serviciosEmpleado.EmpleadoServicios.Count() > 0)
                            {
                                foreach (var servicios in serviciosEmpleado.EmpleadoServicios)
                                {
                                    DetalleEmpleadoServicios detalleEmpleado = new()
                                    {
                                        EmpleadoId = empleado.EmpleadoId,
                                        ServicioId = servicios.ServicioId
                                    };
                                    _context.Add(detalleEmpleado);
                                }
                                _context.SaveChanges();
                            }
                            transaction.Commit();
                        }

                        
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        TempData["Accion"] = "Error";
                        TempData["Mensaje"] = "No se pudo completar la operación";
                        return RedirectToAction("index");
                    }
                }
                TempData["Accion"] = "Crear";
                TempData["Mensaje"] = "Empleado creada correctamente";
                return RedirectToAction("index");
            }
            else
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Ingresaste un valor inválido";
                return RedirectToAction("index");
            }
        }
    }
}
