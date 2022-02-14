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
    public class Empleado
    {
        [Key]
        public int EmpleadoId { get; set; }
        [DisplayName("Empleado")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Column(TypeName = "nvarchar(50)")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [Column(TypeName = "nvarchar(50)")]
        public string Apellidos { get; set; }
        [DisplayName("Edad")]
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [Column(TypeName = "Date")]
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage = "El documento es obligatorio")]
        [Column(TypeName = "nvarchar(15)")]
        public string Documento { get; set; }
        public bool Estado { get; set; }

        public virtual List<DetalleCitaServicios> DetalleCitaServicios { get; set; }
        public virtual List<DetalleEmpleadoServicios> DetalleEmpleadoServicios { get; set; }
    }
}
