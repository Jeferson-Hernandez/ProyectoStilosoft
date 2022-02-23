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
    public class Cliente
    {
        [Key, ForeignKey("IdentityUser")]
        public string ClienteId { get; set; }
        [DisplayName("Cliente")]
        [Column(TypeName = "nvarchar(50)")]
        public string Nombre { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Apellido { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string Numero { get; set; }
        [DisplayName("Cédula")] 
        [Column(TypeName = "nvarchar(15)")]
        public string Documento { get; set; }
        public bool Estado { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
    }
}
