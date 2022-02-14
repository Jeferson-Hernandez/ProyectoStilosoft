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
    public class EstadoCita
    {
        [Key]
        public int EstadoCitaId { get; set; }
        [DisplayName("Estado cita")]
        [Column(TypeName = "nvarchar(20)")]
        public string Nombre { get; set; }
    }
}
