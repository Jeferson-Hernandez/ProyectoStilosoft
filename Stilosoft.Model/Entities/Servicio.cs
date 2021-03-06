using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Model.Entities
{
    public class Servicio
    {
        [Key]
        public int ServicioId { get; set; }
        [DisplayName("Servicio")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Column(TypeName = "nvarchar(50)")]
        public string Nombre { get; set; }
        [DisplayName("Duración")]
        [Required(ErrorMessage = "La duración es obligatoria")]
        public int Duracion { get; set; }
        [Required(ErrorMessage = "El costo es obligatorio")]
        public long Costo { get; set; }
        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Column(TypeName = "nvarchar(100)")]
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        [DisplayName("Mostrar servicio")]
        public bool EstadoLanding { get; set; }

        public virtual List<DetalleEmpleadoServicios> DetalleEmpleadoServicios { get; set; }
    }
}
