using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.ViewModels.Empleados
{
    public class EmpleadoViewModel
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
        [DisplayName("Correo *")]
        public string Email { get; set; }
        [DisplayName("Contraseña *")]
        public string Password { get; set; }
        public List<Servicio> Servicios { get; set; }
        public List<EmpleadoServicios> EmpleadoServicios { get; set; }
    }

    public class EmpleadoServicios
    {
        public int ServicioId { get; set; }
    }
}
