using Microsoft.AspNetCore.Mvc;
using Stilosoft.Business.Abstract;
using System.Threading.Tasks;

namespace ProyectoStilosoft.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly IServicioService _servicioService;

        public ServiciosController(IServicioService servicioService)
        {
            _servicioService = servicioService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View( await _servicioService.ObtenerListaEstilistas());
        }
    }
}
