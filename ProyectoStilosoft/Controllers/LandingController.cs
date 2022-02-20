using Microsoft.AspNetCore.Mvc;
using Stilosoft.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stilosoft.Controllers
{
    public class LandingController : Controller
    {
        //private readonly IProductoService _productoService;

       /* public LandingController(IProductoService productoService)
        {
            _productoService = productoService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _productoService.ObtenerListaProductosEstado());
        }

        public IActionResult Insumos()
        {
            return View();
        }*/

        public IActionResult Index()
        {
            return View();
        }
    }
}
