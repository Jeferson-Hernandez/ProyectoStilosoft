using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoStilosoft.ViewModels.Citas
{
    public class CitasCrearViewModel
    {
        [DisplayName("Cliente *")]
        [Required(ErrorMessage = "El cliente es obligatorio")]
        public string ClienteId { get; set; }
        [DisplayName("Fecha *")]
        [Required(ErrorMessage = "La fecha es obligatoria")]
        public string Fecha { get; set; }
        [DisplayName("Hora *")]
        [Required(ErrorMessage = "La hora es obligatoria")]
        public string Hora { get; set; }
        [Required]
        public long Total { get; set; }
        [Required]
        public int Duracion { get; set; }
        [DisplayName("Empleado *")]
        [Required(ErrorMessage = "El empleado es obligatorio")]
        public string EmpleadoId { get; set; }
        [DisplayName("Servicio *")]
        [Required(ErrorMessage = "El servicio es obligatorio")]
        public int ServicioId { get; set; }
        public List<Servicio> Servicios { get; set; }
    }
}
