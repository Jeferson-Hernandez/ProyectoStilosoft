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
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "Ingrese caracteres válidos")]
        
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "Ingrese caracteres válidos")]
        [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
        public string Apellido { get; set; }
        [DisplayName("Celular")]
        [Required(ErrorMessage = "El número es obligatorio")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Ingrese valores numéricos")]
        [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
        public string Celular { get; set; }
        [DisplayName("Documento")]
        [Required(ErrorMessage = "El documento es obligatorio")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Ingrese valores numéricos")]
        [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
        public string Documento { get; set; }        
        public bool Estado { get; set; }
    }
}
