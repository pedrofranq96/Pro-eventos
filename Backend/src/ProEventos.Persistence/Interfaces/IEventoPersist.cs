using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Interfaces
{
    public interface IEventoPersist
    {
        Task<Evento[]> GetAllEventosPorTemaAsync(string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes= false);
        Task<Evento> GetEventoPorIdAsync(int eventoId, bool includePalestrantes = false);
       
    }
}