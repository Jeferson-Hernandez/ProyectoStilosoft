﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Business.Dtos.Usuarios
{
    public class ResetearPasswordDto
    {
        [Required(ErrorMessage = "El cooreo es requerido")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password",
            ErrorMessage = "Las contraseñas deben coincidir")]
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string ConfirmarPassword { get; set; }

        public string Token { get; set; }
    }
}
