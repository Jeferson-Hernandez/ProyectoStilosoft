using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoStilosoft.ViewModels.Proveedores;
using Stilosoft.Business.Abstract;
using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.Controllers
{    
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _proveedorService;

        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _proveedorService.ObtenerListaProveedor());
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(ProveedorViewModels proveedorViewModels)
        {
            if (ModelState.IsValid)
            {
                Proveedor proveedor = new()
                {
                    Nit = proveedorViewModels.Nit,
                    Nombre = proveedorViewModels.Nombre,
                    Direccion = proveedorViewModels.Direccion,
                    Celular = proveedorViewModels.Celular,
                    Estado = true
                };
                try
                {
                    var NitExiste = await _proveedorService.NitProveedorExiste(proveedor.Nit);
                    if (NitExiste != null)
                    {
                        TempData["Accion"] = "Error";
                        TempData["Mensaje"] = "El NIT ya se encuentra registrado";
                        return RedirectToAction("Index");
                    }
                    await _proveedorService.GuardarProveedor(proveedor);
                    TempData["Accion"] = "Crear";
                    TempData["Mensaje"] = "Proveedor registrado";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["Accion"] = "Error";
                    TempData["Mensaje"] = "Ingresaste un valor inválido";
                    return RedirectToAction("Index");
                }
            }
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "Ingresaste un valor inválido";
            return View(proveedorViewModels);
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Error";
                return RedirectToAction("Index");
            }
            Proveedor proveedor = await _proveedorService.ObtenerProveedorPorId(id.Value);
            ProveedorViewModels proveedorViewModels = new()
            {
                ProveedorId = proveedor.ProveedorId,
                Nit = proveedor.Nit,
                Nombre = proveedor.Nombre,
                Direccion = proveedor.Direccion,
                Celular = proveedor.Celular,
                Estado = proveedor.Estado,
            };
            return View(proveedorViewModels);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(ProveedorViewModels proveedorViewModels)
        {
            if (ModelState.IsValid)
            {
                Proveedor proveedor = new()
                {
                    ProveedorId = proveedorViewModels.ProveedorId,
                    Nit = proveedorViewModels.Nit,
                    Nombre = proveedorViewModels.Nombre,
                    Direccion = proveedorViewModels.Direccion,
                    Celular = proveedorViewModels.Celular,
                    Estado = proveedorViewModels.Estado
                };
                await _proveedorService.EditarProveedor(proveedor);
                TempData["Accion"] = "Editar";
                TempData["Mensaje"] = "Proveedor editado correctamente";
                return RedirectToAction("Index");
            }
            TempData["Accion"] = "Error";
            TempData["Mensaje"] = "Ingresaste un valor inválido";
            return View("index");
        }


        public async Task<IActionResult> EditarEstado(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Error";
                return RedirectToAction("Index");
            }
            Proveedor proveedor = await _proveedorService.ObtenerProveedorPorId(id.Value);
            try
            {
                if (proveedor.Estado == true)
                    proveedor.Estado = false;
                else if (proveedor.Estado == false)
                    proveedor.Estado = true;

                await _proveedorService.EditarProveedor(proveedor);
                TempData["Accion"] = "EditarEstado";
                TempData["Mensaje"] = "Estado editado correctamente";
                return RedirectToAction("index");
            }
            catch (Exception)
            {
                TempData["Accion"] = "Error";
                TempData["Mensaje"] = "Ingresaste un valor inválido";
                return RedirectToAction("Index");
            }
        }
    }
}
