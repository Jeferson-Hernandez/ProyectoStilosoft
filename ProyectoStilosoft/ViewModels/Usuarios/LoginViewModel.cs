using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.ViewModels.Usuarios
{
    public class LoginViewModel
    {
        [DisplayName("Correo *")]
        [Required(ErrorMessage = "El correo es obligatorio")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Correo electrónico incorrecto")]
        [StringLength(70, ErrorMessage = "Máximo 70 caracteres")]
        public string Email { get; set; }
        [DisplayName("Contraseña *")]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        public string Password { get; set; }
        public bool RecordarMe { get; set; }
    }
}
