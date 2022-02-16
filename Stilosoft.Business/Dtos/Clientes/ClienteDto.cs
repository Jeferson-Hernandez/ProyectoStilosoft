using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Business.Dtos.Clientes
{
    public class ClienteDto
    {
        public string ClienteId { get; set; }
        [DisplayName("Cliente")]
        [Required (ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }        
        [Required(ErrorMessage = "El celular es obligatorio")]
        public string Celular { get; set; }
        [DisplayName("Cédula")]
        [Required(ErrorMessage = "La cedela es obligatoria")]
        public string Cedula { get; set; }        
        public bool Estado { get; set; }
    }
}
