using Microsoft.AspNetCore.Mvc;
using Stilosoft.Business.Abstract;
using Stilosoft.Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stilosoft.Controllers
{
    public class LandingController : Controller
    {
        private readonly IServicioService _servicioService;
        private readonly AppDbContext _context;
        public LandingController(IServicioService servicioService, AppDbContext context)
        {
            _servicioService = servicioService;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _servicioService.ObtenerServiciosLanding());
        }
    }
}
