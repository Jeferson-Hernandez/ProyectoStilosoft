using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Model.Entities
{
    public class DetalleCitaServicios
    {
        [Key]
        public int CitaServicioId { get; set; }
        public int CitaId { get; set; }
        public int EmpleadoId { get; set; }
        public int ServicioId { get; set; }

        public virtual Cita Cita { get; set; }
        public virtual Empleado Empleado { get; set; }
        public virtual Servicio Servicio { get; set; }
    }
}
