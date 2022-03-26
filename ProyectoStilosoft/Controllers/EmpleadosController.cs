using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoStilosoft.ViewModels.EmpleadoAgenda;
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
        private readonly IUsuarioService _usuarioService;
        private readonly IEmpleadoService _empleado;
        private readonly IServicioService _servicio;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmpleadosController(AppDbContext context, IEmpleadoService empleado, IServicioService servicio, IUsuarioService usuarioService, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _empleado = empleado;
            _usuarioService = usuarioService;
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
                if (DocumentoExists(serviciosEmpleado.Documento))
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "El documento ya se encuentra registrado";
                    return RedirectToAction("index");
                }
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
                                EmpleadoId = usuario.Id,
                                Nombre = serviciosEmpleado.Nombre,
                                Apellidos = serviciosEmpleado.Apellidos,
                                Documento = serviciosEmpleado.Documento,
                                FechaNacimiento = serviciosEmpleado.FechaNacimiento.Date,
                                Estado = true
                            };

                            Usuario usuario1 = new()
                            {
                                UsuarioId = usuario.Id,
                                Nombre = serviciosEmpleado.Nombre,
                                Apellido = serviciosEmpleado.Apellidos,
                                Numero = "0",
                                Documento = serviciosEmpleado.Documento,
                                Rol = "Empleado",
                                Estado = true
                            };

                            await _usuarioService.GuardarUsuario(usuario1);
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
                        else
                        {
                            TempData["Accion"] = "Error";
                            TempData["Mensaje"] = "El correo ingresado ya se encuentra registrado";
                            return RedirectToAction("index");
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
                TempData["Mensaje"] = "Empleado creado correctamente";
                return RedirectToAction("index");
            }
            else
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Ingresaste un valor inválido";
                return RedirectToAction("index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Editar(string id)
        {
            if (id == null)
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Error";
                return RedirectToAction("index");
            }
            Empleado empleado = await _empleado.ObtenerEmpleadoPorId(id);
            EmpleadoEditarViewModel empleadoEditarViewModel = new()
            {
                EmpleadoId = empleado.EmpleadoId,
                Nombre = empleado.Nombre,
                Apellidos = empleado.Apellidos,
                FechaNacimiento = empleado.FechaNacimiento,
                Documento = empleado.Documento,
                Estado = empleado.Estado
            };
            empleadoEditarViewModel.Servicios = await _servicio.ObtenerListaServiciosEstado();
            empleadoEditarViewModel.detalleEmpleadoServicios = await _empleado.ListaEmpleadoServicios(id);

            return View(empleadoEditarViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(EmpleadoEditarViewModel empleadoEditarViewModel)
        {
            if (ModelState.IsValid)
            {
                if (DocumentoExists(empleadoEditarViewModel.Documento))
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "El documento ya se encuentra registrado";
                    return RedirectToAction("index");
                }
                foreach (var item in empleadoEditarViewModel.EmpleadoServicios)
                {
                    if (ServicioExists(item.ServicioId, empleadoEditarViewModel.EmpleadoId))
                    {
                        TempData["Accion"] = "Error";
                        TempData["Mensaje"] = "No es posible repetir el servicio";
                        return RedirectToAction("Index");
                    }
                }
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Empleado empleado = new()
                        {
                            EmpleadoId = empleadoEditarViewModel.EmpleadoId,
                            Nombre = empleadoEditarViewModel.Nombre,
                            Apellidos = empleadoEditarViewModel.Apellidos,
                            Documento = empleadoEditarViewModel.Documento,
                            FechaNacimiento = empleadoEditarViewModel.FechaNacimiento,
                            Estado = empleadoEditarViewModel.Estado
                        };
                        await _empleado.EditarEmpleado(empleado);

                        if (empleadoEditarViewModel.EmpleadoServicios.Count() > 0)
                        {
                            foreach (var servicios in empleadoEditarViewModel.EmpleadoServicios)
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
                    catch (Exception)
                    {
                        transaction.Rollback();
                        TempData["Accion"] = "Error";
                        TempData["Mensaje"] = "No se pudo completar la operación";
                        return RedirectToAction("index");
                    }
                }
                TempData["Accion"] = "Crear";
                TempData["Mensaje"] = "Empleado editado correctamente";
                return RedirectToAction("index");
            }
            else
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Ingresaste un valor inválido";
                return RedirectToAction("index");
            }
        }
        public async Task<IActionResult> DetalleEmpleado(string id)
        {
            return View(await _empleado.ObtenerListaServiciosEmpleado(id));
        }
        [HttpGet]
        public async Task<IActionResult> ListaServiciosEmpleado(string id)
        {
            return View(await _empleado.ObtenerListaServiciosEmpleado(id));
        }

        [HttpPost]
        public async Task<IActionResult> EliminarEmpleadoServicio(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _empleado.EliminarEmpleadoServicio(id);
                    TempData["Accion"] = "Eliminar";
                    TempData["Mensaje"] = "Compra eliminada correctamente";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "Error realizando la operación";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Error realizando la operación";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarEstado(string id)
        {
            Empleado empleado = await _empleado.ObtenerEmpleadoPorId(id);
            if (empleado.Estado == true)
                empleado.Estado = false;
            else if (empleado.Estado == false)
                empleado.Estado = true;

            try
            {
                await _empleado.EditarEmpleado(empleado);
                TempData["Accion"] = "EditarEstado";
                TempData["Mensaje"] = "Estado editado correctamente";
                return RedirectToAction("index");
            }
            catch (Exception)
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Ingresaste un valor inválido";
                return RedirectToAction("index");
            }
        }
        //Validaciones con AJAX
        [HttpPost]
        public IActionResult obtenerEmpleados(int servicioId)
        {
            return Json(_context.detalleEmpleados.Include(em => em.Empleado).Include(se => se.Servicio)
                .Where(s => s.ServicioId == servicioId).Where(es => es.Empleado.Estado == true).Select(e => new
            {
                EmpleadoId = e.EmpleadoId,
                EmpleadoNombre = e.Empleado.Nombre
            }).ToList());
        }
        [HttpPost]
        public IActionResult ValidarAgenda(string empleadoId, string fecha)
        {
            var fechaExiste = _context.empleadoAgendas.Where(e => e.EmpleadoId == empleadoId).Where(f => f.Fecha == fecha).Count();

            if (fechaExiste == 0)
            {
                return null;
            }
            else
            {
                var agendaId = _context.empleadoAgendas.Where(e => e.EmpleadoId == empleadoId).Where(f => f.Fecha == fecha).Select(em => em.EmpleadoAgendaId).ToList();                
                return Json(_context.agendaOcupadas.Include(em => em.EmpleadoAgenda).Where(a => a.EmpleadoAgendaId == agendaId[0]).Select(a => new 
                {
                    HoraInicio = a.HoraInicio,
                    HoraFin = a.HoraFin
                }).ToList());
            }            
        }
        [HttpPost]
        public int ObtenerAgendaId(string empleadoId, string fecha)
        {            
            var agendaId = _context.empleadoAgendas.Where(e => e.EmpleadoId == empleadoId).Where(f => f.Fecha == fecha).Select(em => em.EmpleadoAgendaId).ToList();
            if (agendaId.Count() != 0)
            {
                return agendaId[0];
            }
            return 0;
        }

        [HttpPost]
        public bool HorarioDisponible(int empleadoAgendaId, string horaInicio, int duracion)
        {
            bool horaDisponibleInicio = false;
            bool horaDisponibleTreinta = false;
            bool horaDisponibleSesenta = false;

            if (duracion > 0 && duracion <= 30)
            {
                horaDisponibleInicio = _context.agendaOcupadas.Where(e => e.EmpleadoAgendaId == empleadoAgendaId).Any(h => h.HoraInicio == horaInicio);
            }
            else if (duracion > 30 && duracion <= 60)
            {
                horaDisponibleInicio = _context.agendaOcupadas.Where(e => e.EmpleadoAgendaId == empleadoAgendaId).Any(h => h.HoraInicio == horaInicio);
                DateTime convertirHoraInicio = DateTime.Parse(horaInicio).AddMinutes(30);
                string horaTreintaMin = convertirHoraInicio.ToString("HH:mm");
                horaDisponibleTreinta = _context.agendaOcupadas.Where(e => e.EmpleadoAgendaId == empleadoAgendaId).Any(h => h.HoraInicio == horaTreintaMin);
            }
            else if (duracion > 60 && duracion <= 90)
            {
                horaDisponibleInicio = _context.agendaOcupadas.Where(e => e.EmpleadoAgendaId == empleadoAgendaId).Any(h => h.HoraInicio == horaInicio);
                DateTime convertirHoraInicio = DateTime.Parse(horaInicio).AddMinutes(30);
                string horaTreintaMin = convertirHoraInicio.ToString("HH:mm");
                horaDisponibleTreinta = _context.agendaOcupadas.Where(e => e.EmpleadoAgendaId == empleadoAgendaId).Any(h => h.HoraInicio == horaTreintaMin);
                DateTime convertirHoraTreinta = DateTime.Parse(horaTreintaMin).AddMinutes(30);
                string horaSesentaMin = convertirHoraTreinta.ToString("HH:mm");
            }

            if (horaDisponibleInicio || horaDisponibleTreinta || horaDisponibleSesenta)
            {
                return false;
            }
            return true;
        }


        //Empleado agenda

        [HttpGet]
        public async Task<IActionResult> ListarAgenda()
        {
            return View( await _empleado.ObtenerListaAgendaEmpleado());
        }
        [HttpGet]
        public async Task<IActionResult> CrearAgenda()
        {
            var empleado = await _context.empleados.Where(e => e.Estado == true).Select(s => new
            {
                EmpleadoId = s.EmpleadoId,
                DatosEmpleado = string.Format("{0} - {1}", s.Nombre , s.Documento)
            }).ToListAsync();

            ViewBag.Empleados = new SelectList(empleado, "EmpleadoId", "DatosEmpleado");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CrearAgenda(EmpleadoAgendaViewModel empleadoAgenda)
        {
            if (ModelState.IsValid)
            {

                if (AgendaExists(empleadoAgenda.EmpleadoId, empleadoAgenda.Fecha))
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "El empleado ya tiene una agenda para esa fecha";
                    return RedirectToAction("ListarAgenda");
                }
                try
                {
                    EmpleadoAgenda agenda = new()
                    {
                        EmpleadoId = empleadoAgenda.EmpleadoId,
                        Fecha = empleadoAgenda.Fecha,
                        HoraInicio = empleadoAgenda.HoraInicio,
                        HoraFin = empleadoAgenda.HoraFin
                    };

                    await _empleado.GuardarEmpleadoAgenda(agenda);
                    TempData["Accion"] = "Crear";
                    TempData["Mensaje"] = "Empleado editado correctamente";
                    return RedirectToAction("ListarAgenda");
                }
                catch (Exception)
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "Error realizando la operación";
                    return RedirectToAction("ListarAgenda");
                }
            }
            else
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Error realizando la operación";
                return RedirectToAction("ListarAgenda");
            }
        }

        private bool AgendaExists(string empleadoId, string fecha)
        {
            return _context.empleadoAgendas.Where(e => e.EmpleadoId == empleadoId).Any(d => d.Fecha == fecha);
        }
        private bool DocumentoExists(string documento)
        {
            return _context.usuarios.Any(d => d.Documento == documento);
        }
        private bool ServicioExists(int servicioId, string empleadoId)
        {
            return _context.detalleEmpleados.Where(e => e.EmpleadoId == empleadoId).Any(s => s.ServicioId == servicioId);
        }
    }
}
