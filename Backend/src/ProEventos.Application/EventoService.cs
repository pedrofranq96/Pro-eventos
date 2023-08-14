using System;
using System.Threading.Tasks;
using ProEventos.Application.Interfaces;
using ProEventos.Persistence.Interfaces;
using ProEventos.Domain;
using ProEventos.Application.Dtos;
using AutoMapper;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventoPersist _eventoPersist;
       private readonly IMapper _mapper;

         public EventoService(IGeralPersist geralPersist,IEventoPersist eventoPersist,IMapper mapper)
        {
            _eventoPersist = eventoPersist;
            _geralPersist = geralPersist;
            _mapper = mapper;
            
        }
        public async Task<EventoDto> AddEventos(EventoDto model)
        {
           try
           {
                var evento = _mapper.Map<Evento>(model);           
                _geralPersist.Add<Evento>(evento);
                
                if(await _geralPersist.SaveChangesAsync()){
                    var eventoRetorno = await _eventoPersist.GetEventoPorIdAsync(evento.Id, false);
                   return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
           }
           catch (Exception ex)
           {
                throw new Exception(ex.Message);
            
           }
        }

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
               var evento = await _eventoPersist.GetEventoPorIdAsync(eventoId, false);
               if (evento == null) return null;
                
               model.Id = evento.Id;

              _mapper.Map(model, evento);

              _geralPersist.Update<Evento>(evento);
              
              if(await _geralPersist.SaveChangesAsync()){
                    var eventoRetorno = await _eventoPersist.GetEventoPorIdAsync(evento.Id, false);

                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                
            }
        }
        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoPorIdAsync(eventoId, false);
                if( evento == null) throw new Exception("Evento para remover não foi encontrado.");
                _geralPersist.Delete<Evento>(evento);
                return await _geralPersist.SaveChangesAsync();         
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);                
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);
                if(eventos == null) return null;

                var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosPorTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosPorTemaAsync(tema, includePalestrantes);
                if(eventos == null) return null;

                var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoPorIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoPorIdAsync(eventoId, includePalestrantes);
                if(evento == null) return null;

                var resultado = _mapper.Map<EventoDto>(evento);

                return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

    }
}