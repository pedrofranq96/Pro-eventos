using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {        
        private readonly IEventoService _service;
        public EventosController(IEventoService service)
        {
            _service = service;            
        } 

        [HttpGet]
        public async  Task<IActionResult> Get() {
            try
            {
                var eventos = await _service.GetAllEventosAsync(true);
                if(eventos == null) return NoContent();         
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar eventos: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) {
            try
            {
                var evento = await _service.GetEventoPorIdAsync(id, true);
                if(evento == null) return NoContent(); 

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar o evento: {ex.Message}");
            }
        }

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema) {
            try
            {
                var eventos = await _service.GetAllEventosPorTemaAsync(tema);
                if(eventos == null) return NoContent(); 

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar o evento: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model){
            try
            {
                var evento = await _service.AddEventos(model);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar adicionar o evento: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model) {
            try
            {
                var evento = await _service.UpdateEvento(id, model);
                if(evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar atualizar o evento: {ex.Message}");
            }
        } 
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            try
            {
                var evento = await _service.GetEventoPorIdAsync(id, true);
                if(evento == null) return NoContent(); 

                return  await _service.DeleteEvento(id) ? Ok("Excluído.") : throw new Exception("Something went wrong");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar excluir o evento: {ex.Message}");
            }
        } 
    }
}


