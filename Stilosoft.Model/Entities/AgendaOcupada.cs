using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Model.Entities
{
    public class AgendaOcupada
    {
        [Key]
        public int AgendaOcupadaId { get; set; }
        public string EmpleadoId { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }

        public virtual Empleado Empleado { get; set; }
    }
}
