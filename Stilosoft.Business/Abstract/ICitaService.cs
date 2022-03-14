using Stilosoft.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stilosoft.Business.Abstract
{
    public interface ICitaService
    {
        Task<IEnumerable<Cita>> ObtenerListaCitas();
        Task<Cita> ObtenerCitaPorId(int id);
        Task GuardarCita(Cita cita);
        Task EditarCita(Cita cita);
    }
}
