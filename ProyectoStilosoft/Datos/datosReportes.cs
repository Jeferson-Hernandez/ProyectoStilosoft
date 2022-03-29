﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoStilosoft.ViewModels.Graficas;
using Stilosoft.Model.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace ProyectoStilosoft.Datos
{
    public class datosReportes
    {
        public List<reporteServicios> retornaServicios(){

            List<reporteServicios> objLista = new List<reporteServicios>();

            using (SqlConnection conexionSQL = new SqlConnection("Data Source = DESKTOP-STNJGIJ; Initial catalog = Stilosoft; Integrated Security = True"))
            {
                string query = "sp_pruebauno";

                SqlCommand cmd = new SqlCommand(query, conexionSQL);
                cmd.CommandType = CommandType.StoredProcedure;

                conexionSQL.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        objLista.Add(new reporteServicios()
                        {
                            nombre = dr["nombre"].ToString(),
                            cantidad = int.Parse(dr["cantidad"].ToString()),
                        });
                    }
                }
            }
            return objLista;
        }
    }
}
