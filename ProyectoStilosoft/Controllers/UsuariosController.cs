using Stilosoft.Business.Dtos.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stilosoft.Business.Abstract;
using Stilosoft.Model.Entities;
using ProyectoStilosoft.ViewModels.Usuarios;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Stilosoft.Model.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stilosoft.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly IEmpleadoService _EmpleadoService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUsuarioService _usuarioService;
        private readonly AppDbContext _context;
        const string SesionNombre = "_Nombre";
        const string SesionId = "_ClienteId";

        public UsuariosController(IClienteService clienteService,IEmpleadoService empleadoService ,UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUsuarioService usuarioService, AppDbContext context)
        {
            _clienteService = clienteService;
            _EmpleadoService = empleadoService;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _configuration = configuration;
            _usuarioService = usuarioService;
            _context = context;
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listaUsuarios = await _usuarioService.ObtenerListaUsuarios();
            //var listaUsuarios = await _userManager.Users.Include(c=>c.Cliente).ToListAsync();            
            return View(listaUsuarios);
        }
   
        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }
     
        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioViewModel usuarioViewModel)
        {
            var response = Request.Form["g-recaptcha-response"];
            string secretKey = "6LcQEdoeAAAAAMOdBpmlaZFoSLWdXboAc7UOiAWm";
            var client = new System.Net.WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);

            var status = (bool)obj.SelectToken("success");
            ViewBag.Message = status ? "Google reCaptcha validation success" : "Google reCaptcha validation failed";
        
            if (ModelState.IsValid && status)
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
                    return RedirectToAction("login", "Usuarios");
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
                        TempData["Mensaje"] = "Usuario registrado correctamente";
                        return RedirectToAction("login", "Usuarios");
                    }
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "El correo ya existe";
                    return RedirectToAction("login", "Usuarios");
                }               
                catch (Exception)
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "Ingresaste un valor inválido";
                    return RedirectToAction("login", "Usuarios");
                }
            }
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "Debe completar el formulario y validar no soy robot";
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string id)
        {
            if (ModelState.IsValid)
            {
               
                var resultado = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RecordarMe, false);
                if (resultado.Succeeded)
                {

                    var usuario = await _userManager.FindByEmailAsync(loginViewModel.Email);
                    var rol = await _userManager.GetRolesAsync(usuario);
                    var usuarioLogin = await _context.usuarios.FirstOrDefaultAsync(i => i.UsuarioId == usuario.Id);                 

                    if (rol.Contains("Administrador"))
                    {
                        return RedirectToAction("Admin", "Dashboard");
                    }
                    else if (rol.Contains("Cliente"))
                    {
                        if (usuarioLogin.Estado == true)
                        {
                            var cliente = await _clienteService.ObtenerClientePorId(usuario.Id);
                            _httpContextAccessor.HttpContext.Session.SetString(SesionNombre, cliente.Nombre);
                            _httpContextAccessor.HttpContext.Session.SetString(SesionId, cliente.ClienteId);
                            return RedirectToAction("index", "Landing");
                        }
                        else
                        {
                            TempData["Accion"] = "Error";
                            TempData["Mensaje"] = "Usuario inactivo";
                            return View();
                        }
                    }
                    else if (rol.Contains("Empleado"))
                    {
                        if (usuarioLogin.Estado == true)
                        {
                            var empleado = await _EmpleadoService.ObtenerEmpleadoPorId(usuario.Id);

                            return RedirectToAction("AgendaCitasEmpleado", "Empleados", new { id = empleado.EmpleadoId });
                        }
                        else
                        {
                            TempData["Accion"] = "Error";
                            TempData["Mensaje"] = "Usuario inactivo";
                            return View();
                        }                  
                    }                   
                }                                     
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Correo o contraseña incorrecto";
                return View();
            }
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "Ingresaste un valor inválido";
            return View(loginViewModel);
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> CrearUsuario()
        {
            //var listaRoles = await _roleManager.Roles.ToListAsync();
            var usuario = await _context.usuarios.Include(u => u.IdentityUser).ToListAsync();
            if (!AdminDosExists(usuario.ToString()))
            {
                var listaRoles = await _roleManager.Roles.Where(r => r.Name != "Empleado").ToListAsync();
                ViewBag.Roles = new SelectList(listaRoles, "Name", "Name");
                return View();

            };
            var listaRolesDos = await _roleManager.Roles.Where(r => r.Name != "Empleado").Where(a => a.Name != "Administrador").ToListAsync(); 
            ViewBag.Roles = new SelectList(listaRolesDos, "Name", "Name");

            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> CrearUsuario(CrearUsuarioViewModel crearUsuarioViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser identityUser = new()
                {
                    UserName = crearUsuarioViewModel.Email,
                    Email = crearUsuarioViewModel.Email
                };
                if (DocumentoExists(crearUsuarioViewModel.Documento))
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "El documento ya se encuentra registrado";
                    return RedirectToAction("index");
                }

                try
                {
                    //Corregir error 
                    //Si creamos un usuario e ingresamos todos las datos y uno de esos esta malo
                    //No pasan los datos del usuario pero si se crea el identity user
                    var resultado = await _userManager.CreateAsync(identityUser, crearUsuarioViewModel.Password);
                    if (resultado.Succeeded)
                    {
                        var usuario = await _userManager.FindByEmailAsync(crearUsuarioViewModel.Email);                       
                        await _userManager.AddToRoleAsync(usuario, crearUsuarioViewModel.Rol);
                        Usuario usuario1 = new()
                        {
                            UsuarioId = usuario.Id,
                            Nombre = crearUsuarioViewModel.Nombre,
                            Apellido = crearUsuarioViewModel.Apellido,
                            Documento = crearUsuarioViewModel.Documento,
                            Numero = crearUsuarioViewModel.Numero,
                            Rol = crearUsuarioViewModel.Rol,
                            Estado = true
                        };
                        if (crearUsuarioViewModel.Rol == "Cliente")
                        {
                            Cliente cliente = new()
                            {
                                ClienteId = usuario.Id,
                                Nombre = crearUsuarioViewModel.Nombre,
                                Apellido = crearUsuarioViewModel.Apellido,
                                Documento = crearUsuarioViewModel.Documento,
                                Numero = crearUsuarioViewModel.Numero,
                                Estado = true
                            };
                           
                            await _clienteService.GuardarCliente(cliente);
                        }
                        await _usuarioService.GuardarUsuario(usuario1);
                        TempData["Accion"] = "Crear";
                        TempData["Mensaje"] = "Usuario registrado correctamente";
                        return RedirectToAction("index");
                    }

                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "El correo ya existe";
                    return RedirectToAction("index");
                }
                catch (Exception)
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "Correo o contraseña incorrecto";
                    return RedirectToAction("index");
                }
            }
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "Ingresaste un valor inválido";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Editar(string id, IdentityUser identityUser)
        {           
            if (id != null)
            {
                var listaRoles = await _roleManager.Roles.Where(r => r.Name != "Admin").ToListAsync();
                ViewBag.Roles = new SelectList(listaRoles, "Name", "Name");
                Usuario usuario = await _usuarioService.ObtenerUsuarioPorId(id);
                UsuarioDto usuarioDto = new()
                {
                    UsuarioId = usuario.UsuarioId,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Documento = usuario.Documento,
                    Numero = usuario.Numero,
                    Estado = usuario.Estado,
                    Rol = usuario.Rol
                };
                return View(usuarioDto);
            }
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "Ingresaste un valor inválido";
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioDto usuarioDto, IdentityUser identityUser, string id)
        {
            if (ModelState.IsValid)
            {
                if (DocumentoExists(usuarioDto.Documento, usuarioDto.UsuarioId))
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "El documento ya se encuentra registrado";
                    return RedirectToAction("Index");
                }
                Usuario usuario1 = new()
                    {
                        UsuarioId = usuarioDto.UsuarioId,
                        Nombre = usuarioDto.Nombre,
                        Apellido = usuarioDto.Apellido,
                        Documento = usuarioDto.Documento,
                        Numero = usuarioDto.Numero,
                        Estado = usuarioDto.Estado,
                        Rol = usuarioDto.Rol
                    };
                
                try
                {
                    if (usuario1.Rol == "Cliente")
                    {
                        Cliente cliente = new()
                        {
                            ClienteId = usuario1.UsuarioId,
                            Nombre = usuarioDto.Nombre,
                            Apellido = usuarioDto.Apellido,
                            Documento = usuarioDto.Documento,
                            Numero = usuarioDto.Numero,
                            Estado = true
                        };                      
                        if (!ClienteExists(cliente.ClienteId))
                        {
                            await _clienteService.GuardarCliente(cliente);                                               
                        }                       
                        await _clienteService.EditarCliente(cliente);                             
                    }
                    Empleado empleado = await _EmpleadoService.ObtenerEmpleadoPorId(id);
                    if (usuario1.Rol == "Empleado")
                    {
                        Empleado empleado1 = new()
                        {
                            EmpleadoId = usuario1.UsuarioId,
                            Nombre = usuarioDto.Nombre,
                            Apellidos = usuarioDto.Apellido,
                            DetalleEmpleadoServicios = empleado.DetalleEmpleadoServicios,
                            Documento = usuarioDto.Documento,
                            FechaNacimiento = empleado.FechaNacimiento,
                            Estado = true
                        };
                        await _EmpleadoService.EditarEmpleado(empleado1);
                    }
                    if (usuario1.Rol != "Cliente")
                      {
                        Cliente cliente = new()
                        {
                            ClienteId = usuario1.UsuarioId
                        };
                        if (!ClienteExists(cliente.ClienteId))
                        {                     
                            await _usuarioService.EditarUsuario(usuario1);
                            TempData["Accion"] = "Editar";
                            TempData["Mensaje"] = "Usuario editado correctamente";
                            return RedirectToAction("index");
                        }
                        await _clienteService.EliminarCliente(id);
                      }
                        
                    var usuario = await _userManager.FindByIdAsync(id);
                    var rol = await _userManager.GetRolesAsync(usuario);

                        await _userManager.RemoveFromRolesAsync(usuario, rol);
                        await _userManager.AddToRoleAsync(usuario, usuarioDto.Rol);
                                                       
                    await _usuarioService.EditarUsuario(usuario1);
                    TempData["Accion"] = "Editar";
                    TempData["Mensaje"] = "Usuario editado correctamente";
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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditarEstado(string id)
        {
            if (id == null)
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Error";
                return RedirectToAction("index");
            }
            Usuario usuario = await _usuarioService.ObtenerUsuarioPorId(id);
            Cliente cliente = await _clienteService.ObtenerClientePorId(id);
            Empleado empleado = await _EmpleadoService.ObtenerEmpleadoPorId(id);      
            try
            {
                if (usuario.Estado == true)
                    usuario.Estado = false;
                else if (usuario.Estado == false)
                    usuario.Estado = true;

                if (usuario.Rol == "Cliente")
                {
                    if (cliente.Estado == true)
                        cliente.Estado = false;
                    else if (cliente.Estado == false)
                        cliente.Estado = true;
                    await _clienteService.EditarCliente(cliente);
                }
                else if (usuario.Rol == "Empleado")
                {
                    if (empleado.Estado == true)
                        empleado.Estado = false;
                    else if (empleado.Estado == false)
                        empleado.Estado = true;
                    await _EmpleadoService.EditarEmpleado(empleado);
                }

                await _usuarioService.EditarUsuario(usuario);
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

        [HttpGet]
        public IActionResult OlvidePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OlvidePassword(OlvidePasswordDto olvidePasswordDto)
        {
            if (ModelState.IsValid)
            {
                // buscamos el email a ver si existe
                var usuario = await _userManager.FindByEmailAsync(olvidePasswordDto.Email);

                if (usuario != null)
                {
                    //generamos un token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);

                    //creamos un link para resetear el password
                    var passwordresetLink = Url.Action("ResetearPassword", "Usuarios",
                        new { email = olvidePasswordDto.Email, token = token }, Request.Scheme);

                    //Opción 1

                    MailMessage mensaje = new();
                    mensaje.To.Add(olvidePasswordDto.Email); //destinatario
                    mensaje.Subject = "Recuperar contraseña";

                    mensaje.Body = "<h1> Gracias por contactarte con nosotros </h1><br>" +
                        "<h3> Ingresa al siguiente link para recuperar tu contraseña<h3><br>"+ passwordresetLink;
                   
                    mensaje.IsBodyHtml = true;
                    mensaje.From = new MailAddress(_configuration["Mail"], "Maria C Stilos");
                    SmtpClient smtpClient = new("smtp.gmail.com");
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential(_configuration["Mail"], _configuration["Password"]);
                    smtpClient.Send(mensaje);
                    return View("OlvidePasswordConfirmacion");

                }else
                {
                    return View(olvidePasswordDto);
                }
            }


            return View(olvidePasswordDto);
        }

        //Cuando hacemos clic en el link que llegó al correo
       
        [HttpGet]
        public IActionResult ResetearPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Error token");
            }
            return View();
        }
        //Cuando hacemos clic en el link que llegó al correo
        
        [HttpPost]
        public async Task<IActionResult> ResetearPassword(ResetearPasswordDto resetearPasswordDto)
        {
            if (ModelState.IsValid)
            {
                //buscamos el usuario
                var usuario = await _userManager.FindByEmailAsync(resetearPasswordDto.Email);              

                if (usuario != null)
                {
                    //se resetea el password
                    var result = await _userManager.ResetPasswordAsync(usuario, resetearPasswordDto.Token, resetearPasswordDto.Password);
                    if (result.Succeeded)
                        return View("ResetearPasswordConfirmacion");
                    else
                    {
                        foreach (var errores in result.Errors)
                        {
                            if (errores.Description.ToString().Equals("Invalid token."))
                                ModelState.AddModelError("", "Debe generar un nuevo enlace");
                        }
                        return View(resetearPasswordDto);
                    }
                }
                return View(resetearPasswordDto);
            }
            return View(resetearPasswordDto);
        }
        [Authorize]
        /// Debe permitir al admin cambiar las contraseñas de los usuarios
        [HttpGet]
        public IActionResult ResetPassword(string id, IdentityUser identityUser)
        {                                 
            return View();                
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string id, CambiarPasswordDto cambiarPassWord)
        {
            if (ModelState.IsValid)
            {
                //buscamos el usuario
                var usuario = await _userManager.FindByIdAsync(id);
                var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);            
             try
                {
                    if (usuario != null)
                    {
                        //se resetea el password
                        var result = await _userManager.ResetPasswordAsync(usuario, token, cambiarPassWord.Password);

                    }
                    TempData["Accion"] = "Cambiar contraseña";
                    TempData["Mensaje"] = "Usuario cambio de contraseña correctamente";                    
                    return RedirectToAction("index");
                }
                catch
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "Ingresaste algun valor no valido";
                    return RedirectToAction("index");
                }
               
            }
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "Ingresaste algun valor no valido";
            return RedirectToAction("index");
        }
      
        private bool ClienteExists(string id)
        {
            return _context.clientes.Any(e => e.ClienteId == id);
        }
        private bool AdminDosExists(string rol)
        {
            return _context.usuarios.Any(e => e.Rol == "Administrador");
        }

        [HttpGet]
        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            _httpContextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("login", "Usuarios");
        }
        private bool DocumentoExists(string documento)
        {
            return _context.usuarios.Any(d => d.Documento == documento);
        }
        private bool DocumentoExists(string documento, string id)
        {
            return _context.usuarios.Where(i => i.UsuarioId != id).Any(d => d.Documento == documento);
        }
    }
}
