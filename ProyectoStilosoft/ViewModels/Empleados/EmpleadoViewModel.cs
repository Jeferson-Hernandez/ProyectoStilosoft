using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.ViewModels.Empleados
{
    public class EmpleadoViewModel
    {
        public string EmpleadoId { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "La fecha y hora son obligatorias")]
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage = "El documento es obligatorio")]
        public string Documento { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Servicio> Servicios { get; set; }
        public List<EmpleadoServicios> EmpleadoServicios { get; set; }
    }

    public class EmpleadoServicios
    {
        public int ServicioId { get; set; }
    }
}
