using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stilosoft.Business.Abstract;
using Stilosoft.Business.Dtos.Clientes;
using Stilosoft.Model.Entities;
using ProyectoStilosoft.ViewModels.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stilosoft.Model.DAL;

namespace Stilosoft.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClientesController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly IUsuarioService _usuarioService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;



        public ClientesController(AppDbContext context, IClienteService clienteService, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IUsuarioService usuarioService)
        {
            _clienteService = clienteService;
            _usuarioService = usuarioService;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;

        }
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteService.ObtenerListaClientes();
            return View(clientes);
        }
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(UsuarioViewModel usuarioViewModel)
        {

            if (ModelState.IsValid)
            {
                IdentityUser identityUser = new()
                {
                    UserName = usuarioViewModel.Email,
                    Email = usuarioViewModel.Email
                };
                if (DocumentoExists(usuarioViewModel.Documento))
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "El documento ya se encuentra registrado";
                    return RedirectToAction("index");
                }

                try
                {
                    var resultado = await _userManager.CreateAsync(identityUser, usuarioViewModel.Password);
                    if (resultado.Succeeded)
                    {
                        var usuario = await _userManager.FindByEmailAsync(usuarioViewModel.Email);
                        await _userManager.AddToRoleAsync(usuario, "Cliente");
                        Cliente cliente = new()
                        {
                            ClienteId = usuario.Id,
                            Nombre = usuarioViewModel.Nombre,
                            Apellido = usuarioViewModel.Apellido,
                            Numero = usuarioViewModel.Numero,
                            Documento = usuarioViewModel.Documento,
                            Estado = true
                        };
                        Usuario usuario1 = new()
                        {
                            UsuarioId = usuario.Id,
                            Nombre = usuarioViewModel.Nombre,
                            Apellido = usuarioViewModel.Apellido,
                            Numero = usuarioViewModel.Numero,
                            Documento = usuarioViewModel.Documento,
                            Rol = "Cliente",
                            Estado = true
                        };
                        await _usuarioService.GuardarUsuario(usuario1);
                        await _clienteService.GuardarCliente(cliente);
                        TempData["Accion"] = "Registrar";
                        TempData["Mensaje"] = "Cliente registrado correctamente";
                        return RedirectToAction("index");
                    }
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "El correo ya existe";
                    return RedirectToAction("index");
                }
                catch (Exception)
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "Ingresaste un valor inválido";
                    return RedirectToAction("index");
                }
            }
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "Ingresaste un valor inválido";
            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> EditarEstado(string id)
        {
            if (id == null)
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Error";
                return RedirectToAction("index");
            }
            Cliente cliente = await _clienteService.ObtenerClientePorId(id);
            try
            {
                if (cliente.Estado == true)
                    cliente.Estado = false;
                else if (cliente.Estado == false)
                    cliente.Estado = true;

                await _clienteService.EditarCliente(cliente);
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
        [HttpPost]
        public async Task<IActionResult> Eliminar(string id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "Error";
                    return RedirectToAction("index");
                }
                await _clienteService.EliminarCliente(id);
                TempData["Accion"] = "Eliminar";
                TempData["Mensaje"] = "Cliente eliminado correctamente";
                return RedirectToAction("index");
            }
            catch (Exception)
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Ingresaste un valor inválido";
                return RedirectToAction("index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Editar(string id)
        {
            if (id != null)
            {
                Cliente cliente = await _clienteService.ObtenerClientePorId(id);
                ClienteDto clienteDto = new()
                {
                    ClienteId = cliente.ClienteId,
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    Documento = cliente.Documento,
                    Celular = cliente.Numero,
                    Estado = cliente.Estado
                };
                return View(clienteDto);
            }
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "Ingresaste un valor inválido";
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> Editar(ClienteDto clienteDto)
        {
            if (ModelState.IsValid)
            {
                Cliente cliente = new()
                {
                    ClienteId = clienteDto.ClienteId,
                    Nombre = clienteDto.Nombre,
                    Apellido = clienteDto.Apellido,
                    Documento = clienteDto.Documento,
                    Numero = clienteDto.Celular,
                    Estado = clienteDto.Estado
                };
                try
                {
                    await _clienteService.EditarCliente(cliente);
                    TempData["Accion"] = "Editar";
                    TempData["Mensaje"] = "Cliente editado correctamente";
                    return RedirectToAction("index");
                }
                catch (Exception)
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "Ingresaste un valor inválido";
                    return RedirectToAction("index");
                }
            }
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "Ingresaste un valor inválido";
            return RedirectToAction("index");
        }
        private bool DocumentoExists(string documento)
        {
            return _context.usuarios.Any(d => d.Documento == documento);
        }
    }

}
