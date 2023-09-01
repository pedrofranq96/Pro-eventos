using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.API.Extensions;
namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController : ControllerBase
    {
        private readonly IRedeSocialService _service;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;
        public RedesSociaisController(IRedeSocialService service, IEventoService eventoService, IPalestranteService palestranteService)
        {
            _service = service;
            _eventoService = eventoService;
            _palestranteService = palestranteService;
        }

        [HttpGet("evento/{eventoId}")]
        public async  Task<IActionResult> GetByEvento(int eventoId) 
        {
            try
            {
                if(!(await AutorEvento(eventoId))){
                    return Unauthorized();
                }
                var redeSocial = await _service.GetAllByEventoIdAsync(eventoId);
                if(redeSocial == null) return NoContent();         
                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar eventos: {ex.Message}");
            }
        }
        [HttpGet("palestrante")]
        public async  Task<IActionResult> GetByPalestrante() 
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByIdAsync(User.GetUserId());
                if(palestrante == null) return Unauthorized();

                var redeSocial = await _service.GetAllByPalestranteIdAsync(palestrante.Id);
                if(redeSocial == null) return NoContent();         
                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar eventos por palestrante: {ex.Message}");
            }
        }
        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models) 
        {
            try
            {
                if(!(await AutorEvento(eventoId))){
                    return Unauthorized();
                }
                var redeSocial = await _service.SaveByEvento(eventoId, models);                
                if(redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar salvar rede Social por evento: {ex.Message}");
            }
        }
        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models) 
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByIdAsync(User.GetUserId());
                if(palestrante == null) return Unauthorized();

                var redeSocial = await _service.SaveByPalestrante(palestrante.Id, models);                
                if(redeSocial == null) return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar salvar rede Social por palestrante: {ex.Message}");
            }
        }   
        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId,int redeSocialId)
        {
            try
            {
                if(!(await AutorEvento(eventoId))){
                    return Unauthorized();
                }
                var redeSocial = await _service.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if(redeSocial == null) return NoContent();

                return  await _service.DeleteByEvento(eventoId, redeSocialId)
                    ? Ok(new { message = "rede Social Excluída."})
                    : throw new Exception("Ocorreu um problema ao tentar remover rede Social por evento.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar excluir rede social por evento: {ex.Message}");
            }
        }
        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByIdAsync(User.GetUserId());
                if(palestrante == null) return Unauthorized();

                var redeSocial = await _service.GetRedeSocialPalestranteByIdsAsync(palestrante.Id, redeSocialId);
                if(redeSocial == null) return NoContent();

                return  await _service.DeleteByPalestrante(palestrante.Id, redeSocialId)
                    ? Ok(new { message = "rede Social Excluída."})
                    : throw new Exception("Ocorreu um problema ao tentar remover rede Social por palestrante.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar excluir rede social por palestrante: {ex.Message}");
            }
        } 
        [NonAction]
        private async Task<bool> AutorEvento(int eventoId)
        {
            var evento = await _eventoService.GetEventoPorIdAsync(User.GetUserId(), eventoId, false);
            if(evento == null) return false;

            return true;
        }
    }
}


