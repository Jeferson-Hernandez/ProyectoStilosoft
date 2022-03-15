﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoStilosoft.ViewModels.Citas;
using Stilosoft.Business.Abstract;
using Stilosoft.Model.DAL;
using Stilosoft.Model.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.Controllers
{
    public class CitasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICitaService _cita;
        private readonly IServicioService _servicio;
        private readonly IEmpleadoService _empleado;

        public CitasController(AppDbContext context, ICitaService citaService, IServicioService servicio, IEmpleadoService empleado)
        {
            _context = context;
            _cita = citaService;
            _servicio = servicio;
            _empleado = empleado;
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
        public async Task<IActionResult> citaEstados(int citaId, int estadoId)
        {
            try
            {
                Cita cita = await _cita.ObtenerCitaPorId(citaId);
                cita.EstadoCitaId = estadoId;

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
