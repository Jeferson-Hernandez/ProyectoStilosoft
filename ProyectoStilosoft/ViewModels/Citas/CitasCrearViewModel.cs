using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;

namespace ProyectoStilosoft.ViewModels.Citas
{
    public class CitasCrearViewModel
    {
        public string ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public long Total { get; set; }
        public List<DetalleEmpleadoServicios> EmpleadoServicios { get; set; }
    }
}
