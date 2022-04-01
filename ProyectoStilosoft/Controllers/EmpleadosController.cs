using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoStilosoft.ViewModels.EmpleadoAgenda;
using ProyectoStilosoft.ViewModels.Empleados;
using ProyectoStilosoft.ViewModels.Usuarios;
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
        private readonly ICitaService _cita;
        private readonly IServicioService _servicio;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmpleadosController(AppDbContext context, ICitaService citaService, IEmpleadoService empleado, IServicioService servicio, IUsuarioService usuarioService, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _empleado = empleado;
            _usuarioService = usuarioService;
            _servicio = servicio;
            _cita = citaService;
            _userManager = userManager;
            _roleManager = roleManager;
        }
     
        public async Task<IActionResult> Index()
        {
            return View(await _empleado.ObtenerListaEmpleados());
        }
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> AgendaCitasEmpleado(string id)
        {
            return View(await _context.citas.Where(i => i.EmpleadoId == id).Include(c => c.Cliente).Include(s => s.Servicio).Include(e => e.EstadoCita).ToListAsync());
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            EmpleadoViewModel empleado = new();
            empleado.Servicios = await _servicio.ObtenerListaServiciosEstado();
            return View(empleado);
        }
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Editar(EmpleadoEditarViewModel empleadoEditarViewModel)
        {
            if (ModelState.IsValid)
            {
                
                if (EmpleadoExists(empleadoEditarViewModel.Documento, empleadoEditarViewModel.EmpleadoId))
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "El documento ya se encuentra registrado";
                    return RedirectToAction("index", "Empleados");
                }
                if (empleadoEditarViewModel.EmpleadoServicios != null)
                {
                    foreach (var item in empleadoEditarViewModel.EmpleadoServicios)
                    {
                        if (ServicioExists(item.ServicioId, empleadoEditarViewModel.EmpleadoId))
                        {
                            TempData["Accion"] = "Error";
                            TempData["Mensaje"] = "No es posible repetir el servicio";
                            return RedirectToAction("Index");
                        }
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
                        Usuario usuario = new()
                        {
                            UsuarioId = empleadoEditarViewModel.EmpleadoId,
                            Nombre = empleadoEditarViewModel.Nombre,
                            Apellido = empleadoEditarViewModel.Apellidos,
                            Numero = "0",
                            Documento = empleadoEditarViewModel.Documento,
                            Rol = "Empleado",
                            Estado = empleadoEditarViewModel.Estado
                        };
                        await _usuarioService.EditarUsuario(usuario);
                        await _empleado.EditarEmpleado(empleado);
                        if(empleadoEditarViewModel.EmpleadoServicios != null)
                        {
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
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DetalleEmpleado(string id)
        {
            return View(await _empleado.ObtenerListaServiciosEmpleado(id));
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ListaServiciosEmpleado(string id)
        {
            return View(await _empleado.ObtenerListaServiciosEmpleado(id));
        }
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> EditarEstado(string id)
        {           
            try
            {
                Empleado empleado = await _empleado.ObtenerEmpleadoPorId(id);
                Usuario usuario = await _usuarioService.ObtenerUsuarioPorId(id);
                empleado.EmpleadoId = usuario.UsuarioId;

                if (empleado.Estado == true)
                {
                    empleado.Estado = false;
                    usuario.Estado = false;
                }
                else if (empleado.Estado == false)
                {
                    empleado.Estado = true;
                    usuario.Estado = true;
                }

                await _usuarioService.EditarUsuario(usuario);
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
        [Authorize]
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
        [Authorize]
        [HttpPost]
        public IActionResult ValidarAgenda(string empleadoId, string fecha)
        {
            var novedadExiste = _context.empleadoNovedades.Where(e => e.EmpleadoId == empleadoId).Where(f => f.Fecha == fecha).Any();

            if (novedadExiste)
            {
                return Json(_context.empleadoNovedades.Where(a => a.EmpleadoId == empleadoId).Where(f => f.Fecha == fecha).Select(a => new
                {
                    EmpleadoId = "novedad",
                    HoraInicio = a.HoraInicio,
                    HoraFin = a.HoraFin
                }).FirstOrDefault());
            }
            return Json(_context.empleados.Select(a => new
            {
                EmpleadoId = "normal",
                HoraInicio = "8:00",
                HoraFin = "20:00"
            }).FirstOrDefault());
        }
        [Authorize]
        [HttpPost]
        public bool HorarioDisponible(string empleadoId, string horaInicio, int duracion, string fecha)
        {
            int contador = 0;
            bool horaOcupada = false;

            if (horaInicio == null)
            {
                horaInicio = "08:00";
            }
            if (duracion > 30 && duracion <= 60)
                contador = 1;
            else if (duracion > 60 && duracion <= 90)
                contador = 2;
            else if (duracion > 90 && duracion <= 120)
                contador = 3;
            else if (duracion > 120 && duracion <= 150)
                contador = 4;
            else if (duracion > 150 && duracion <= 180)
                contador = 5;
            else if (duracion > 180 && duracion <= 210)
                contador = 6;
            else if (duracion > 210 && duracion <= 240)
                contador = 7;
            else if (duracion > 240 && duracion <= 270)
                contador = 8;

            var empleadoNovedad = _context.empleadoNovedades.Where(e => e.EmpleadoId == empleadoId).Where(f => f.Fecha == fecha).FirstOrDefault();
            if (empleadoNovedad != null)
            {
                var empleadoHoraInicio = empleadoNovedad.HoraInicio;
                var empleadoHoraFin = empleadoNovedad.HoraFin;

                DateTime novedadHoraInicio = DateTime.Parse(empleadoHoraInicio);
                DateTime novedadHoraFin = DateTime.Parse(empleadoHoraFin);
                DateTime horaSeleccionada = DateTime.Parse(horaInicio);
                if (horaSeleccionada >= novedadHoraInicio && horaSeleccionada <= novedadHoraFin)
                {
                    return false;
                }
                else
                {
                    var citaHoraCiclo = horaInicio;
                    DateTime novedadHoraInicioTreinta = DateTime.Parse(empleadoHoraInicio).AddMinutes(30);
                    string novedadHoraInicioTreintaString = novedadHoraInicioTreinta.ToString("HH:mm");
                    for (int i = 0; i <= contador; i++)
                    {
                        DateTime citaHoraNueva = DateTime.Parse(citaHoraCiclo).AddMinutes(30);
                        string citaHoraString = citaHoraNueva.ToString("HH:mm");

                        if (citaHoraString == novedadHoraInicioTreintaString)
                        {
                            return false;
                        }
                        citaHoraCiclo = citaHoraString;
                    }
                }
            }

            string citaHora = horaInicio;
            var horaDisponibleInicial = _context.agendaOcupadas.Where(e => e.EmpleadoId == empleadoId).Where(f => f.Fecha == fecha).Any(h => h.HoraInicio == citaHora);
            if (horaDisponibleInicial || citaHora == "20:00")
            {
                return false;
            }

            for (int i = 0; i <= contador; i++)
            {                
                DateTime CitaHoraNueva = DateTime.Parse(citaHora).AddMinutes(30);
                string CitaHoraString = CitaHoraNueva.ToString("HH:mm");
                var horaDisponible = _context.agendaOcupadas.Where(e => e.EmpleadoId == empleadoId).Where(f => f.Fecha == fecha).Any(h => h.HoraInicio == CitaHoraString);
                if (horaDisponible || CitaHoraString == "20:30")
                {
                    horaOcupada = true;
                    break;
                }
                citaHora = CitaHoraString;
            }

            if (horaOcupada)
            {
                return false;
            }
            return true;

        }

        //Empleado novedades
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ListarAgenda()
        {
            return View(await _empleado.ObtenerListaNovedades());
        }
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> CrearAgenda(EmpleadoAgendaViewModel empleadoNovedad)
        {
            if (ModelState.IsValid)
            {

                if (NovedadExists(empleadoNovedad.EmpleadoId, empleadoNovedad.Fecha))
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "El empleado ya tiene una novedad para esa fecha";
                    return RedirectToAction("ListarAgenda");
                }
                try
                {
                    EmpleadoNovedad novedad = new()
                    {
                        EmpleadoId = empleadoNovedad.EmpleadoId,
                        Fecha = empleadoNovedad.Fecha,
                        HoraInicio = empleadoNovedad.HoraInicio,
                        HoraFin = empleadoNovedad.HoraFin
                    };

                    await _empleado.GuardarEmpleadoNovedad(novedad);
                    TempData["Accion"] = "Crear";
                    TempData["Mensaje"] = "Empleado creado correctamente";
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
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> EditarNovedad(int id)
        {
            var novedad = await _empleado.ObtenerNovedadPorId(id);

            EmpleadoAgendaEditarViewModel editar = new()
            {
                EmpleadoNovedadId = novedad.EmpleadoNovedadId,
                EmpleadoId = novedad.EmpleadoId,
                Fecha = novedad.Fecha,
                HoraFin = novedad.HoraFin,
                HoraInicio = novedad.HoraInicio
            };

            var empleado = await _context.empleados.Where(e => e.Estado == true).Select(s => new
            {
                EmpleadoId = s.EmpleadoId,
                DatosEmpleado = string.Format("{0} - {1}", s.Nombre, s.Documento)
            }).ToListAsync();

            ViewBag.Empleados = new SelectList(empleado, "EmpleadoId", "DatosEmpleado");

            return View(editar);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> EditarNovedad(EmpleadoAgendaEditarViewModel empleadoNovedad)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (NovedadEditarExists(empleadoNovedad.EmpleadoId, empleadoNovedad.Fecha, empleadoNovedad.EmpleadoNovedadId))
                    {
                        TempData["Accion"] = "Error";
                        TempData["Mensaje"] = "Ya se encuentra un empleado con esa novedad";
                        return RedirectToAction("ListarAgenda");
                    }
                    EmpleadoNovedad novedad = new()
                    {
                        EmpleadoNovedadId = empleadoNovedad.EmpleadoNovedadId,
                        EmpleadoId = empleadoNovedad.EmpleadoId,
                        Fecha = empleadoNovedad.Fecha,
                        HoraFin = empleadoNovedad.HoraFin,
                        HoraInicio = empleadoNovedad.HoraInicio
                    };

                    await _empleado.EditarEmpleadoNovedad(novedad);
                    TempData["Accion"] = "Editar";
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

        private bool NovedadExists(string empleadoId, string fecha)
        {
            return _context.empleadoNovedades.Where(e => e.EmpleadoId == empleadoId).Any(d => d.Fecha == fecha);
        }
        private bool NovedadEditarExists(string empleadoId, string fecha, int novedadId)
        {
            return _context.empleadoNovedades.Where(n => n.EmpleadoNovedadId != novedadId).Where(e => e.EmpleadoId == empleadoId).Any(d => d.Fecha == fecha);
        }
        private bool DocumentoExists(string documento)
        {
            return _context.usuarios.Any(d => d.Documento == documento);
        }
        private bool ServicioExists(int servicioId, string empleadoId)
        {
            return _context.detalleEmpleados.Where(e => e.EmpleadoId == empleadoId).Any(s => s.ServicioId == servicioId);
        }
        private bool EmpleadoExists(string documento, string id)
        {
            return _context.empleados.Where(i => i.EmpleadoId != id).Any(d => d.Documento == documento);
        }
    }
}
