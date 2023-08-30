using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(int userId, EventoDto model);
        Task<EventoDto> UpdateEvento(int userId,int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int userId,int id);
        Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);
        Task<EventoDto> GetEventoPorIdAsync(int userId,int eventoId, bool includePalestrantes = false);      
    }
}