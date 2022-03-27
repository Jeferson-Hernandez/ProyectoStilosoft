using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProyectoStilosoft.ViewModels.Citas;
using Stilosoft.Business.Abstract;
using Stilosoft.Model.DAL;
using Stilosoft.Model.Entities;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProyectoStilosoft.Controllers
{
    public class CitasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICitaService _cita;
        private readonly IServicioService _servicio;
        private readonly IEmpleadoService _empleado;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public CitasController(AppDbContext context, ICitaService citaService, IServicioService servicio, IEmpleadoService empleado, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _cita = citaService;
            _servicio = servicio;
            _empleado = empleado;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _cita.ObtenerListaCitas());
        }
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var cliente = await _context.clientes.Include(u => u.IdentityUser).Where(e => e.Estado == true).Select(s => new
            {
                ClienteId = s.ClienteId,
                DatosCliente = string.Format("{0} {1} - {2}", s.Nombre, s.Apellido, s.Documento)
            }).ToListAsync();

            ViewBag.Clientes = new SelectList(cliente, "ClienteId", "DatosCliente");

            CitasCrearViewModel cita = new();
            cita.Servicios = await _servicio.ObtenerListaServiciosEstado();

            return View(cita);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CitasCrearViewModel citaDatos)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Cita cita = new()
                        {
                            ClienteId = citaDatos.ClienteId,
                            Fecha = citaDatos.Fecha,
                            Hora = citaDatos.Hora,
                            Total = citaDatos.Total,
                            EstadoCitaId = 1
                        };
                        _context.Add(cita);
                        await _context.SaveChangesAsync();

                        DetalleCitaServicios citaServicio = new()
                        {
                            CitaId = cita.CitaId,
                            EmpleadoId = citaDatos.EmpleadoId,
                            ServicioId = citaDatos.ServicioId
                        };
                        _context.Add(citaServicio);
                        await _context.SaveChangesAsync();


                        int contador = 0;
                        if (citaDatos.Duracion > 0 && citaDatos.Duracion <= 30)
                        {
                            contador = 1;
                        }
                        else if (citaDatos.Duracion > 30 && citaDatos.Duracion <= 60)
                        {
                            contador = 2;
                        }
                        else if (citaDatos.Duracion > 60 && citaDatos.Duracion <= 90)
                        {
                            contador = 3;
                        }

                        DateTime fechaHoraFin = DateTime.Parse(citaDatos.Hora).AddMinutes(citaDatos.Duracion);
                        string horaFin = fechaHoraFin.ToString("HH:mm");

                        string citaHora = citaDatos.Hora;

                        for (int i = 0; i < contador; i++)
                        {
                            AgendaOcupada agendaOcupada = new()
                            {
                                EmpleadoId = citaDatos.EmpleadoId,
                                HoraInicio = citaHora,
                                HoraFin = horaFin
                            };
                            _context.Add(agendaOcupada);
                            await _context.SaveChangesAsync();

                            DateTime CitaHoraNueva = DateTime.Parse(citaHora).AddMinutes(30);
                            string CitaHoraString = CitaHoraNueva.ToString("HH:mm");
                            citaHora = CitaHoraString;
                        }                            

                        var usuario = await _userManager.FindByIdAsync(citaDatos.ClienteId);

                        MailMessage mensaje = new();
                        mensaje.To.Add(usuario.Email); //destinatario
                        mensaje.Subject = "Cita Stilosoft";

                        mensaje.Body = "<h1> Cita reservada correctamente </h1><br>" +
                            "<h3> Gracias por confiar en nosotros <h3><br>";

                        mensaje.IsBodyHtml = true;
                        mensaje.From = new MailAddress(_configuration["Mail"], "Maria C Stilos");
                        SmtpClient smtpClient = new("smtp.gmail.com");
                        smtpClient.Port = 587;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.EnableSsl = true;
                        smtpClient.Credentials = new System.Net.NetworkCredential(_configuration["Mail"], _configuration["Password"]);
                        smtpClient.Send(mensaje);

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
                TempData["Mensaje"] = "Cita creada correctamente";
                return RedirectToAction("index");
            }
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "Se ingresó un valor inválido";
            return RedirectToAction("index");
        }
        [HttpGet]
        public async Task<IActionResult> clienteCita()
        {
            CitasCrearViewModel cita = new();
            cita.Servicios = await _servicio.ObtenerListaServiciosEstado();
            return View(cita);
        }
        public async Task<IActionResult> clienteCita(CitasCrearViewModel citaDatos)
        {
            if (ModelState.IsValid)
            {
                if (citaDatos.ClienteId == null)
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "No se pudo completar la operación";
                    return RedirectToAction("Index", "Landing");
                }
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Cita cita = new()
                        {
                            ClienteId = citaDatos.ClienteId,
                            Fecha = citaDatos.Fecha,
                            Hora = citaDatos.Hora,
                            Total = citaDatos.Total,
                            EstadoCitaId = 1
                        };
                        _context.Add(cita);
                        await _context.SaveChangesAsync();

                        DetalleCitaServicios citaServicio = new()
                        {
                            CitaId = cita.CitaId,
                            EmpleadoId = citaDatos.EmpleadoId,
                            ServicioId = citaDatos.ServicioId
                        };
                        _context.Add(citaServicio);
                        await _context.SaveChangesAsync();

                        var usuario = await _userManager.FindByIdAsync(citaDatos.ClienteId);

                        MailMessage mensaje = new();
                        mensaje.To.Add(usuario.Email); //destinatario
                        mensaje.Subject = "Cita Stilosoft";

                        mensaje.Body = "<h1> Cita reservada correctamente </h1><br>" +
                            "<h3> Gracias por confiar en nosotros <h3><br>";

                        mensaje.IsBodyHtml = true;
                        mensaje.From = new MailAddress(_configuration["Mail"], "Maria C Stilos");
                        SmtpClient smtpClient = new("smtp.gmail.com");
                        smtpClient.Port = 587;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.EnableSsl = true;
                        smtpClient.Credentials = new System.Net.NetworkCredential(_configuration["Mail"], _configuration["Password"]);
                        smtpClient.Send(mensaje);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        TempData["Accion"] = "Error";
                        TempData["Mensaje"] = "No se pudo completar la operación";
                        return RedirectToAction("index","Landing");
                    }
                }
                TempData["Accion"] = "Crear";
                TempData["Mensaje"] = "Cita creada correctamente";
                return RedirectToAction("index", "Landing");
            }
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "Se ingresó un valor inválido";
            return RedirectToAction("index","Landing");
        }
        public async Task<IActionResult> citaEstados(int citaId, int estadoId, string empleadoId, string horaInicio)
        {
            try
            {
                Cita cita = await _cita.ObtenerCitaPorId(citaId);
                cita.EstadoCitaId = estadoId;

                if ( cita.EstadoCitaId == 4)
                {
                    var usuario = await _userManager.FindByIdAsync(cita.ClienteId);

                    MailMessage mensaje = new();
                    mensaje.To.Add(usuario.Email); //destinatario
                    mensaje.Subject = "Cita Stilosoft";

                    mensaje.Body = "<h1> Cita cancelada correctamente </h1><br>" +
                        "<h3> Gracias por confiar en nosotros <h3><br>";

                    mensaje.IsBodyHtml = true;
                    mensaje.From = new MailAddress(_configuration["Mail"], "Maria C Stilos");
                    SmtpClient smtpClient = new("smtp.gmail.com");
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential(_configuration["Mail"], _configuration["Password"]);
                    smtpClient.Send(mensaje);
                }

                await _cita.EditarCita(cita);
                TempData["Accion"] = "EditarEstado";
                TempData["Mensaje"] = "Estado de la cita editado correctamente";
                return RedirectToAction("index");
            }
            catch (Exception)
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "No se pudo completar la operación";
                return RedirectToAction("index");
            }
        }

        public async Task<IActionResult> DetalleCita(int id)
        {
            return View(await _cita.ObtenerDetalleCita(id));
        }
    }
}
