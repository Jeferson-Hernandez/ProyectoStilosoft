using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;

namespace ProyectoStilosoft.ViewModels.Citas
{
    public class CitasCrearViewModel
    {
        public string ClienteId { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public long Total { get; set; }
        public string EmpleadoId { get; set; }
        public int ServicioId { get; set; }
        public List<Servicio> Servicios { get; set; }
    }
}
