using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(int userId, EventoDto model);
        Task<EventoDto> UpdateEvento(int userId,int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int userId,int id);
        Task<EventoDto[]> GetAllEventosAsync(int userId,bool includePalestrantes = false);
        Task<EventoDto[]> GetAllEventosPorTemaAsync(int userId,string tema, bool includePalestrantes = false);
        Task<EventoDto> GetEventoPorIdAsync(int userId,int eventoId, bool includePalestrantes = false);      
    }
}