using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stilosoft.ViewModels.Usuarios
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Correo electrónico incorrecto")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "La contraseña es obligatoria")]        
        public string Password { get; set; }
        public bool RecordarMe { get; set; }
    }
}
