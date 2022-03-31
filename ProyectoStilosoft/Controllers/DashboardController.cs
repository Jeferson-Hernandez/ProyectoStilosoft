using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Stilosoft.Model.DAL;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ProyectoStilosoft.Datos;
using ProyectoStilosoft.ViewModels.Graficas;
using System.Web;




namespace ProyectoStilosoft.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.VentasFinalizadas = _context.citas.Where(e => e.EstadoCitaId == 3).Count();
            ViewBag.VentasCanceladas = _context.citas.Where(e => e.EstadoCitaId == 4).Count();

            return View();
        }

        public IActionResult gpastel()
        {           
            return View();
        }

        public IActionResult barras()
        {
            return View();
        }

        [HttpGet]
        public JsonResult reporteServicios()
        {
            datosReportes obj_datosReportes = new datosReportes();

            List<reporteServicios> objLista = obj_datosReportes.retornaServicios();

            return Json(objLista);
        }

        [HttpGet]
        public JsonResult reporteClientes()
        {
            datosReportes obj_datosReportes = new datosReportes();

            List<reporteClientes> objLista = obj_datosReportes.retornaClientes();

            return Json(objLista);
        }



    }
}