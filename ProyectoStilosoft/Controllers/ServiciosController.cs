using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoStilosoft.ViewModels.Servicios;
using Stilosoft.Business.Abstract;
using Stilosoft.Model.DAL;
using Stilosoft.Model.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ServiciosController : Controller
    {
        private readonly IServicioService _servicioService;
        private readonly AppDbContext _context;

        public ServiciosController(IServicioService servicioService, AppDbContext context)
        {
            _servicioService = servicioService;
            _context = context;
        }
    
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _servicioService.ObtenerListaServicios());
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
      
        [HttpPost]
        public async Task<IActionResult> Crear(ServicioViewModel servicioViewModel)
        {
            if (ModelState.IsValid)
            {
                Servicio servicio = new()
                {
                    Nombre = servicioViewModel.Nombre,
                    Duracion = servicioViewModel.Duracion,
                    Costo = servicioViewModel.Costo,
                    Descripcion = servicioViewModel.Descripcion,
                    EstadoLanding = false,
                    Estado = true
                };

                try
                {
                    var ServicioExiste = await _servicioService.NombreServicioExiste(servicio.Nombre);

                    if (ServicioExiste != null)
                    {
                        TempData["Accion"] = "Error";
                        TempData["Mensaje"] = "El servicio ya se encuentra registrado";
                        return RedirectToAction("index");
                    }

                    await _servicioService.GuardarServicio(servicio);
                    TempData["Accion"] = "Crear";
                    TempData["Mensaje"] = "Servicio guardado correctamente";
                    return RedirectToAction("index");
                }
                catch (Exception)
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "Ingresaste un valor inválido";
                    return RedirectToAction("index");
                }
            }
            else
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Ingresaste un valor inválido";
                return RedirectToAction("index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Servicio servicio = await _servicioService.ObtenerServicioPorId(id);
            ServicioViewModel servicioViewModel = new()
            {
                ServicioId = servicio.ServicioId,
                Nombre = servicio.Nombre,
                Duracion = servicio.Duracion,
                Costo = servicio.Costo,
                Descripcion = servicio.Descripcion,
                EstadoLanding = servicio.EstadoLanding,
                Estado = servicio.Estado
            };

            return View(servicioViewModel);
        }
      
        [HttpPost]
        public async Task<IActionResult> Editar(ServicioViewModel servicioViewModel, int id)
        {
            if (ModelState.IsValid)
            {
                Servicio servicio = new()
                {
                    ServicioId = servicioViewModel.ServicioId,
                    Nombre = servicioViewModel.Nombre,
                    Duracion = servicioViewModel.Duracion,
                    Costo = servicioViewModel.Costo,
                    Descripcion = servicioViewModel.Descripcion,
                    EstadoLanding = servicioViewModel.EstadoLanding,
                    Estado = servicioViewModel.Estado
                };

                try
                {              
                    if (ServicioExists(servicioViewModel.Nombre, servicioViewModel.ServicioId))
                    {
                        TempData["Accion"] = "Error";
                        TempData["Mensaje"] = "El servicio ya se encuentra registrado";
                        return RedirectToAction("Index");
                    }                                         
                        await _servicioService.EditarServicio(servicio);
                        TempData["Accion"] = "Editar";
                        TempData["Mensaje"] = "Servicio editado correctamente";
                        return RedirectToAction("index");                    
                }
                catch (Exception)
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "Ingresaste un valor inválido";
                    return RedirectToAction("index");
                }
            }
            else
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Ingresaste un valor inválido";
                return RedirectToAction("index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarEstado(int id)
        {
            Servicio servicio = await _servicioService.ObtenerServicioPorId(id);

            if (servicio.Estado == true)
                servicio.Estado = false;
            else if (servicio.Estado == false)
                servicio.Estado = true;

            try
            {
                await _servicioService.EditarServicio(servicio);
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
        public async Task<IActionResult> EditarEstadoLanding(int id)
        {
            Servicio servicio = await _servicioService.ObtenerServicioPorId(id);

            if (servicio.EstadoLanding == true)
                servicio.EstadoLanding = false;
            else if (servicio.EstadoLanding == false)
                servicio.EstadoLanding = true;

            try
            {
                await _servicioService.EditarServicio(servicio);
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
        private bool ServicioExists(string nombre,int id)
        {
            return _context.servicios.Where(i => i.ServicioId != id).Any(d => d.Nombre == nombre);
        }
    }
}
