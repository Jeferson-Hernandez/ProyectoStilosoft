﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Model.Entities
{
    public class Cita
    {
        [Key]
        public int CitaId { get; set; }
        [ForeignKey("Cliente")]
        public string ClienteId { get; set; }
        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "Date")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "La hora es obligatoria")]
        [Column(TypeName = "nvarchar(20)")]
        public string Hora { get; set; }
        [Required]
        public long Total { get; set; }
        [Required]
        public int EstadoCitaId { get; set; }

        public virtual EstadoCita EstadoCita { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual DetalleCitaServicios DetalleCitaServicios { get; set; }
    }
}
