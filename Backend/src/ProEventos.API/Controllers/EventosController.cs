using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Interfaces;
using ProEventos.Domain;

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
                if(eventos == null) return NotFound("Nenhum evento encontrado!");

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
                if(evento == null) return NotFound("Evento por id não encontrado!");

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
                if(eventos == null) return NotFound("Eventos por tema não encontrados!");

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar o evento: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model){
            try
            {
                var evento = await _service.AddEventos(model);
                if(evento == null) return BadRequest("Erro ao tentar adicionar um novo evento.");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar adicionar o evento: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Evento model) {
            try
            {
                var evento = await _service.UpdateEvento(id, model);
                if(evento == null) return BadRequest("Erro ao tentar atualizar o evento.");

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
                return  await _service.DeleteEvento(id) ? Ok("Excluído.") : BadRequest("Evento não Excluído.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar excluir o evento: {ex.Message}");
            }
        } 
    }
}


