using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Administrador")]
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
                        var horaOcupada = _context.agendaOcupadas.Where(e => e.EmpleadoId == citaDatos.EmpleadoId).Where(f => f.Fecha == citaDatos.Fecha).Any(h => h.HoraInicio == citaDatos.Hora);
                        if (horaOcupada)
                        {
                            TempData["Accion"] = "Error";
                            TempData["Mensaje"] = "La hora seleccionada ya se encuentra agendada";
                            return RedirectToAction("index");
                        }

                        var clienteCita = _context.citas.Where(c => c.ClienteId == citaDatos.ClienteId).Where(f => f.Fecha == citaDatos.Fecha).Where(h => h.Hora == citaDatos.Hora).Any();
                        if (clienteCita)
                        {
                            TempData["Accion"] = "Error";
                            TempData["Mensaje"] = "Ya hay una cita registrada para la fecha y hora seleccionadas";
                            return RedirectToAction("index");
                        }

                        int contador = 0;
                        if (citaDatos.Duracion > 0 && citaDatos.Duracion <= 30)
                            contador = 1;
                        else if (citaDatos.Duracion > 30 && citaDatos.Duracion <= 60)
                            contador = 2;
                        else if (citaDatos.Duracion > 60 && citaDatos.Duracion <= 90)
                            contador = 3;
                        else if (citaDatos.Duracion > 90 && citaDatos.Duracion <= 120)
                            contador = 4;
                        else if (citaDatos.Duracion > 120 && citaDatos.Duracion <= 150)
                            contador = 5;
                        else if (citaDatos.Duracion > 150 && citaDatos.Duracion <= 180)
                            contador = 6;
                        else if (citaDatos.Duracion > 180 && citaDatos.Duracion <= 210)
                            contador = 7;
                        else if (citaDatos.Duracion > 210 && citaDatos.Duracion <= 240)
                            contador = 8;
                        else if (citaDatos.Duracion > 240 && citaDatos.Duracion <= 270)
                            contador = 9;

                        var empleadoNovedad = _context.empleadoNovedades.Where(e => e.EmpleadoId == citaDatos.EmpleadoId).Where(f => f.Fecha == citaDatos.Fecha).FirstOrDefault();

                        if (empleadoNovedad != null)
                        {
                            var empleadoHoraInicio = empleadoNovedad.HoraInicio;
                            var empleadoHoraFin = empleadoNovedad.HoraFin;

                            DateTime novedadHoraInicio = DateTime.Parse(empleadoHoraInicio);
                            DateTime novedadHoraFin = DateTime.Parse(empleadoHoraFin);
                            DateTime horaSeleccionada = DateTime.Parse(citaDatos.Hora);
                            if (horaSeleccionada >= novedadHoraInicio && horaSeleccionada <= novedadHoraFin)
                            {
                                TempData["Accion"] = "Error";
                                TempData["Mensaje"] = "El empleado tiene una novedad para la hora seleccionada";
                                return RedirectToAction("index");
                            }
                            else
                            {
                                var citaHoraCiclo = citaDatos.Hora;
                                DateTime novedadHoraInicioTreinta = DateTime.Parse(empleadoHoraInicio).AddMinutes(30);
                                string novedadHoraInicioTreintaString = novedadHoraInicioTreinta.ToString("HH:mm");
                                for (int i = 0; i <= contador; i++)
                                {
                                    DateTime citaHoraNueva = DateTime.Parse(citaHoraCiclo).AddMinutes(30);
                                    string citaHoraString = citaHoraNueva.ToString("HH:mm");

                                    if (citaHoraString == novedadHoraInicioTreintaString)
                                    {
                                        TempData["Accion"] = "Error";
                                        TempData["Mensaje"] = "El empleado tiene una novedad, tenga en cuenta que si la duración pasa el horario de novedad no es posible asignarla";
                                        return RedirectToAction("index");
                                    }
                                    citaHoraCiclo = citaHoraString;
                                }
                            }
                        }

                        Cita cita = new()
                        {
                            ClienteId = citaDatos.ClienteId,
                            EmpleadoId = citaDatos.EmpleadoId,
                            ServicioId = citaDatos.ServicioId,
                            Fecha = citaDatos.Fecha,
                            Hora = citaDatos.Hora,
                            Total = citaDatos.Total,
                            EstadoCitaId = 1
                        };
                        _context.Add(cita);
                        await _context.SaveChangesAsync();
                                                
                        DateTime fechaHoraFin = DateTime.Parse(citaDatos.Hora).AddMinutes(citaDatos.Duracion);
                        string horaFin = fechaHoraFin.ToString("HH:mm");

                        string citaHora = citaDatos.Hora;

                        for (int i = 0; i < contador; i++)
                        {
                            AgendaOcupada agendaOcupada = new()
                            {
                                EmpleadoId = citaDatos.EmpleadoId,
                                Fecha = citaDatos.Fecha,
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

                        //MailMessage mensaje = new();
                        //mensaje.To.Add(usuario.Email); //destinatario
                        //mensaje.Subject = "Cita Stilosoft";

                        //mensaje.Body = "<h1> Cita reservada correctamente </h1><br>" +
                        //    "<h3> Gracias por confiar en nosotros <h3><br>";

                        //mensaje.IsBodyHtml = true;
                        //mensaje.From = new MailAddress(_configuration["Mail"], "Maria C Stilos");
                        //SmtpClient smtpClient = new("smtp.gmail.com");
                        //smtpClient.Port = 587;
                        //smtpClient.UseDefaultCredentials = false;
                        //smtpClient.EnableSsl = true;
                        //smtpClient.Credentials = new System.Net.NetworkCredential(_configuration["Mail"], _configuration["Password"]);
                        //smtpClient.Send(mensaje);

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
        [Authorize(Roles = "Cliente")]
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
                        var horaOcupada = _context.agendaOcupadas.Where(e => e.EmpleadoId == citaDatos.EmpleadoId).Where(f => f.Fecha == citaDatos.Fecha).Any(h => h.HoraInicio == citaDatos.Hora);
                        if (horaOcupada)
                        {
                            TempData["Accion"] = "Error";
                            TempData["Mensaje"] = "La hora seleccionada ya se encuentra agendada";
                            return RedirectToAction("index");
                        }

                        var clienteCita = _context.citas.Where(c => c.ClienteId == citaDatos.ClienteId).Where(f => f.Fecha == citaDatos.Fecha).Where(h => h.Hora == citaDatos.Hora).Any();
                        if (clienteCita)
                        {
                            TempData["Accion"] = "Error";
                            TempData["Mensaje"] = "Ya hay una cita registrada para la fecha y hora seleccionadas";
                            return RedirectToAction("index");
                        }
                        Cita cita = new()
                        {
                            ClienteId = citaDatos.ClienteId,
                            EmpleadoId = citaDatos.EmpleadoId,
                            ServicioId = citaDatos.ServicioId,
                            Fecha = citaDatos.Fecha,
                            Hora = citaDatos.Hora,
                            Total = citaDatos.Total,
                            EstadoCitaId = 1
                        };
                        _context.Add(cita);
                        await _context.SaveChangesAsync();

                        int contador = 0;
                        if (citaDatos.Duracion > 0 && citaDatos.Duracion <= 30)
                            contador = 1;
                        else if (citaDatos.Duracion > 30 && citaDatos.Duracion <= 60)
                            contador = 2;
                        else if (citaDatos.Duracion > 60 && citaDatos.Duracion <= 90)
                            contador = 3;
                        else if (citaDatos.Duracion > 90 && citaDatos.Duracion <= 120)
                            contador = 4;
                        else if (citaDatos.Duracion > 120 && citaDatos.Duracion <= 150)
                            contador = 5;
                        else if (citaDatos.Duracion > 150 && citaDatos.Duracion <= 180)
                            contador = 6;
                        else if (citaDatos.Duracion > 180 && citaDatos.Duracion <= 210)
                            contador = 7;
                        else if (citaDatos.Duracion > 210 && citaDatos.Duracion <= 240)
                            contador = 8;
                        else if (citaDatos.Duracion > 240 && citaDatos.Duracion <= 270)
                            contador = 9;

                        DateTime fechaHoraFin = DateTime.Parse(citaDatos.Hora).AddMinutes(citaDatos.Duracion);
                        string horaFin = fechaHoraFin.ToString("HH:mm");

                        string citaHora = citaDatos.Hora;

                        for (int i = 0; i < contador; i++)
                        {
                            AgendaOcupada agendaOcupada = new()
                            {
                                EmpleadoId = citaDatos.EmpleadoId,
                                Fecha = citaDatos.Fecha,
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
        public async Task<IActionResult> citaEstados(int citaId, int estadoId, string empleadoId, string horaInicio, int duracion, string fecha)
        {
            try
            {
                Cita cita = await _cita.ObtenerCitaPorId(citaId);
                cita.EstadoCitaId = estadoId;

                if ( cita.EstadoCitaId == 4)
                {
                    DateTime fechaHoraFin = DateTime.Parse(horaInicio).AddMinutes(duracion);
                    string horaFin = fechaHoraFin.ToString("HH:mm");

                    var agendaEliminar = await _context.agendaOcupadas.Where(e => e.EmpleadoId == empleadoId).Where(f => f.Fecha == fecha).Where(h => h.HoraFin == horaFin).ToListAsync();

                    if (agendaEliminar != null)
                    {
                        foreach (var item in agendaEliminar)
                        {
                            _context.Remove(item);
                            await _context.SaveChangesAsync();
                        }
                    }

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
    }
}
