using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.ViewModels.Empleados
{
    public class EmpleadoEditarViewModel
    {
        public string EmpleadoId { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Documento { get; set; }
        public bool Estado { get; set; }
        public List<Servicio> Servicios { get; set; }
        public List<DetalleEmpleadoServicios> detalleEmpleadoServicios { get; set; }
        public List<EmpleadoServicios> EmpleadoServicios { get; set; }
    }
}
