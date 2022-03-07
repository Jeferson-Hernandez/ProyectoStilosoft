using Microsoft.AspNetCore.Mvc;
using Stilosoft.Business.Abstract;
using System.Threading.Tasks;

namespace ProyectoStilosoft.Controllers
{
    public class CitasController : Controller
    {
        private readonly ICitaService _citaService;

        public CitasController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _citaService.ObtenerListaCitas());
        }
    }
}
