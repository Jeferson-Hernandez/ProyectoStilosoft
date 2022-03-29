using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Model.Entities
{
    public class EmpleadoNovedad
    {
        [Key]
        public int EmpleadoNovedadId { get; set; }
        public string EmpleadoId { get; set; }
        public string Fecha { get; set; }
        [DisplayName("Hora de inicio")]
        public string HoraInicio { get; set; }
        [DisplayName("Hora final")]
        public string HoraFin { get; set; }

        public virtual Empleado Empleado { get; set; }
    }
}
