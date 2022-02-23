using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.ViewModels.Proveedores
{
    public class ProveedorViewModels
    {
        public int ProveedorId { get; set; }

        [DisplayName("Nit")]
        [Required(ErrorMessage = "El NIT es obligatorio")]
        [StringLength(12, ErrorMessage = "Máximo 25 caracteres")]
        [RegularExpression("^[0-9-]*$", ErrorMessage = "Ingrese valores numéricos")]
        public string Nit { get; set; }

        [DisplayName("Proveedor")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "Ingrese caracteres válidos")]
        public string Nombre { get; set; }

        [DisplayName("Dirección")]
        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(50, ErrorMessage = "Máximo 25 caracteres")]
        public string Direccion { get; set; }

        [DisplayName("Celular")]
        [Required(ErrorMessage ="El celular es obligatorio")]
        [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Ingrese valores numéricos")]
        public string Celular { get; set; }

        [DisplayName("Contacto")]
        [Required(ErrorMessage = "El correo es obligatorio")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "Ingrese caracteres válidos")]
        public string Contacto { get; set; }

        public bool Estado { get; set; }
        
    }
}
