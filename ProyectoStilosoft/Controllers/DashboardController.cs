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
            //ViewBag.ConteoServiciosOne = _context.detalleCitas.Where(e => e.ServicioId == 1).Count();
            //ViewBag.ConteoServiciosTwo = _context.detalleCitas.Where(e => e.ServicioId == 2).Count();
            //ViewBag.ConteoServiciosThree = _context.detalleCitas.Where(e => e.ServicioId == 3).Count();
            //ViewBag.ConteoServiciosDos = _context.citas.Where(i => i.EstadoCitaId == 3).Select(e => e.DetalleCitaServicios.Servicio.Nombre).Distinct().ToList();



            return View();
        }

        public IActionResult barras()
        {
            return View();
        }

        [HttpGet]
        public JsonResult reporteServicio()
        {
            datosReportes obj_datosReportes = new datosReportes();

            List<reporteServicios> objLista = obj_datosReportes.retornaServicios();

            return Json(objLista);


        }


    }        
}



        //Conexion con la BD

        //public class datosReportes
        //{

        //[HttpGet]
        //    public List<reporteServicios> retornaServicios()
        //    {

        //        List<reporteServicios> objLista = new List<reporteServicios>();

        //        using (SqlConnection conexionSQL = new SqlConnection("Data Source = DESKTOP-STNJGIJ; Initial catalog = Stilosoft; Integrated Security = True"))
        //        {
        //            string query = "sp_prueba";
        //            SqlCommand cmd = new SqlCommand(query, conexionSQL);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            conexionSQL.Open();

        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    objLista.Add(new reporteServicios()
        //                    {
        //                        nombre = dr["Nombre"].ToString(),
        //                        cantidad = int.Parse(dr["Cantidad"].ToString()),
        //                    });
        //                }
        //            }
        //        }
        //        return objLista;
        //    }
        //}
//    }
//}

        //[HttpGet]
    //    protected string obtenerDatos()
    //    {
    //        SqlConnection conexionSQL = new SqlConnection("Data Source = DESKTOP-STNJGIJ; Initial catalog = Stilosoft; Integrated Security = True");

    //        SqlCommand cmd = new SqlCommand();
    //        cmd.CommandText = "Select Nombre, ProveedorId from proveedors";
    //        cmd.CommandType = CommandType.Text;
    //        cmd.Connection = conexionSQL;
    //        conexionSQL.Open();

    //        DataTable Datos = new DataTable();
    //        Datos.Load(cmd.ExecuteReader());
    //        conexionSQL.Close();

    //        string strDatos;

    //        strDatos = "[['Task', 'Hours per Day'],";

    //        foreach (DataRow dr in Datos.Rows)
    //        {
    //            strDatos = strDatos + "[";
    //            strDatos = strDatos + "'" + dr[0] + "'" + "," + dr[1];
    //            strDatos = strDatos + "],";
    //        }
    //        strDatos = strDatos + "]";
    //        return strDatos;
    //    }
    //}
//}
