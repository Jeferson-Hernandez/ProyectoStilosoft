using Microsoft.AspNetCore.Identity;
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
    public class Usuario
    {
        [Key, ForeignKey("IdentityUser")]
        public string UsuarioId { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Column(TypeName = "nvarchar(50)")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [Column(TypeName = "nvarchar(50)")]
        public string Apellido { get; set; }
        [DisplayName("Número")]
        [Required(ErrorMessage = "El número es obligatorio")]
        [Column(TypeName = "nvarchar(10)")]
        public string Numero { get; set; }
        [DisplayName("Cédula")]
        [Required(ErrorMessage = "La cédula es obligatoria")]
        [Column(TypeName = "nvarchar(15)")]
        public string Documento { get; set; }
        public string Rol { get; set; }
        public bool Estado { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
    }
}
