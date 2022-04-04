using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Business.Dtos.Usuarios
{
   public class UsuarioDto
    {
        public string UsuarioId { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Apellido { get; set; }
        [DisplayName("Número")]
        [Required(ErrorMessage = "El número es obligatorio")]
        [Column(TypeName = "nvarchar(10)")]
        [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
        public string Numero { get; set; }    
        [DisplayName("Documento")]
        [Required(ErrorMessage = "El documento es obligatorio")]
        [StringLength(15, ErrorMessage = "Máximo 15 caracteres")]
        public string Documento { get; set; }
        public string Rol { get; set; }
        public bool Estado { get; set; }
    }
}
