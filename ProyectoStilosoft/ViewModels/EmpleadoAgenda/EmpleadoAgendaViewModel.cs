using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.ViewModels.EmpleadoAgenda
{
    public class EmpleadoAgendaViewModel
    {
        [DisplayName("Empleado *")]
        [Required(ErrorMessage = "El empleado es obligatorio")]
        public string EmpleadoId { get; set; }
        [DisplayName("Fecha *")]
        [Required(ErrorMessage = "La fecha es obligatoria")]
        public string Fecha { get; set; }
        [DisplayName("Hora inicio *")]
        [Required(ErrorMessage = "La hora de inicio es obligatoria")]
        public string HoraInicio { get; set; }
        [DisplayName("Hora final *")]
        [Required(ErrorMessage = "La hora final es obligatoria")]
        public string HoraFin { get; set; }
    }
}
