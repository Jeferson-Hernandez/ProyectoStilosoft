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
    public class Proveedor
    {
        [Key]
        public int ProveedorId { get; set; }

        [DisplayName("Nit")]
        [Required(ErrorMessage = "El nit es obligatorio")]
        [Column(TypeName = "nvarchar(12)")]
        public string Nit { get; set; }

        [DisplayName("Proveedor")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Column(TypeName = "nvarchar(50)")]
        public string Nombre { get; set; }

        [DisplayName("Dirección")]
        [Required(ErrorMessage = "La direccion es obligatoria")]
        [Column(TypeName = "nvarchar(25)")]
        public string Direccion { get; set; }

        [DisplayName("Celular")]
        [Required(ErrorMessage = "El celular es obligatorio")]
        [Column(TypeName = "nvarchar(10)")]
        public string Celular { get; set; }

        [DisplayName("Correo")]
        [Required(ErrorMessage = "El correo es obligatorio")]
        [Column(TypeName = "nvarchar(50)")]
        public string Correo { get; set; }

        [DisplayName ("Contacto")]
        [Required(ErrorMessage = "El nombre del contacto es obligatorio")]
        [Column(TypeName ="nvarchar(50)")]
        public string Contacto { get; set; }
        [Required]

        public bool Estado { get; set; }
    }
}
