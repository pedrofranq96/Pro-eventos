using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(EventoDto model);
        Task<EventoDto> UpdateEvento(int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int id);

        Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<EventoDto[]> GetAllEventosPorTemaAsync(string tema, bool includePalestrantes = false);
        Task<EventoDto> GetEventoPorIdAsync(int eventoId, bool includePalestrantes = false);
       
    }
}