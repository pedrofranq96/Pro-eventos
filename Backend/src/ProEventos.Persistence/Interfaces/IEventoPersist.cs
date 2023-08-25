using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Interfaces
{
    public interface IEventoPersist
    {
        Task<Evento[]> GetAllEventosPorTemaAsync(int userId,string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes= false);
        Task<Evento> GetEventoPorIdAsync(int userId, int eventoId, bool includePalestrantes = false);      
    }
}