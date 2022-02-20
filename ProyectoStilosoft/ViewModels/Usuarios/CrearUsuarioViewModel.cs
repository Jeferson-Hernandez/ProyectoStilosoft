using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Stilosoft.ViewModels.Usuarios
{
    public class CrearUsuarioViewModel
    {
        [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Ingrese caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Ingrese caracteres")]
        [StringLength(30, ErrorMessage = "Máximo 30 caracteres")]
        public string Apellido { get; set; }
        [DisplayName("Celular")]
        [Required(ErrorMessage = "El número es obligatorio")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Ingrese valores numéricos")]
        [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
        public string Numero { get; set; }
        [DisplayName("Documento")]
        [Required(ErrorMessage = "El documento es obligatorio")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Ingrese valores numéricos")]
        [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
        public string Documento { get; set; }
        [Required(ErrorMessage = "El email es obligatorio")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Correo electrónico incorrecto")]
        public string Email { get; set; }
        [DisplayName("Contraseña")]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
        [Required(ErrorMessage = "El rol es obligatorio")]
        public string Rol { get; set; }
        public bool Estado { get; set; }

    }
}
