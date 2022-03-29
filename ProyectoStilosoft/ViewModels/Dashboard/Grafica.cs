using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.ViewModels.Dashboard
{
    public class Grafica
    {
        public string barras()
        {
            SqlConnection conexionSQL = new SqlConnection("Data Source = DESKTOP-88LP2EU; Initial catalog = Stilosoft; Integrated Security = True");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select Nombre from proveedors";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conexionSQL;
            conexionSQL.Open();

            DataTable Datos = new DataTable();
            Datos.Load(cmd.ExecuteReader());
            conexionSQL.Close();

            string strDatos;

            strDatos = "[['ProveedorId', 'Nombre'],";

            foreach (DataRow dr in Datos.Rows)
            {
                strDatos = strDatos + "[";
                strDatos = strDatos + "'" + dr[0] + "," + dr[1];
                strDatos = strDatos + "],";
            }
            strDatos = strDatos + "]";


            return strDatos;
        }
    }
}
