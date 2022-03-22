using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
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
        public bool Login()
        {
            using (SqlConnection con = new SqlConnection("Data Source = DESKTOP-88LP2EU; Initial catalog = Stilosoft; Integrated Security = True"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("CitasFinalizadass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CitasFinalizadas","Finalizada");              


                //Si hay datos retorna true
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }
}
