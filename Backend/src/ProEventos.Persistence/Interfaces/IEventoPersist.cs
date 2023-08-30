using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Interfaces
{
    public interface IEventoPersist
    {
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);
        Task<Evento> GetEventoPorIdAsync(int userId, int eventoId, bool includePalestrantes = false);      
    }
}