using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stilosoft.Business.Abstract;
using Stilosoft.Model.DAL;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.Controllers
{
    public class CitasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICitaService _citaService;
        private readonly IServicioService _servicio;
        private readonly IEmpleadoService _empleado;

        public CitasController(AppDbContext context, ICitaService citaService, IServicioService servicio, IEmpleadoService empleado)
        {
            _context = context;
            _citaService = citaService;
            _servicio = servicio;
            _empleado = empleado;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _citaService.ObtenerListaCitas());
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
            ViewBag.Empleados = new SelectList(await _empleado.ObtenerListaEmpleadosEstado(), "EmpleadoId", "Nombre");
        }
    }
}
