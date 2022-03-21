using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoStilosoft.ViewModels.Servicios
{
    public class ServicioViewModel
    {
        public int ServicioId { get; set; }
        [DisplayName("Servicio*")]
        [Required(ErrorMessage = "El servicio es obligatorio")]
        [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "Ingrese caracteres")]
        public string Nombre { get; set; }
        [DisplayName("Duración*")]
        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(10, 999, ErrorMessage = "Ingrese un valor entre 10 min y 999 min")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Ingrese valores numéricos")]
        public int Duracion { get; set; }
        [DisplayName("Costo*")]
        [Required(ErrorMessage = "El costo es obligatorio")]
        [Range(5000, 1000000, ErrorMessage = "Ingrese un valor entre 5.000 y 1.000.000")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Ingrese valores numéricos")]
        public long Costo { get; set; }
        public bool Estado { get; set; }
    }
}
