using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.ViewModels.Empleados
{
    public class EmpleadoEditarViewModel
    {
        public string EmpleadoId { get; set; }

        [DisplayName("Nombres *")]
        public string Nombre { get; set; }

        [DisplayName("Apellidos *")]
        public string Apellidos { get; set; }

        [DisplayName("Fecha nacimiento *")]
        public DateTime FechaNacimiento { get; set; }

        [DisplayName("Documento *")]
        public string Documento { get; set; }
        public bool Estado { get; set; }
        public List<Servicio> Servicios { get; set; }
        public List<DetalleEmpleadoServicios> detalleEmpleadoServicios { get; set; }
        public List<EmpleadoServicios> EmpleadoServicios { get; set; }
    }
}
