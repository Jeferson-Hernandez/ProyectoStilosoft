using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.ViewModels.EmpleadoAgenda
{
    public class EmpleadoAgendaViewModel
    {
        [DisplayName("Empleado *")]
        public string EmpleadoId { get; set; }
        [DisplayName("Fecha *")]
        public string Fecha { get; set; }
        [DisplayName("Hora de inicio *")]
        public string HoraInicio { get; set; }
        [DisplayName("Hora final *")]
        public string HoraFin { get; set; }
    }
}
