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
    public class LotesController : ControllerBase
    {
        private readonly ILoteService _service;
        public LotesController(ILoteService service)
        {
            _service = service;            
        } 

        [HttpGet("{eventoId}")]
        public async  Task<IActionResult> Get(int eventoId) {
            try
            {
                var lotes = await _service.GetLotesByEventoIdAsync(eventoId);
                if(lotes == null) return NoContent();         
                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar lotes: {ex.Message}");
            }
        } 
        

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models) {
            try
            {
                var lotes = await _service.SaveLotes(eventoId, models);
                if(lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar salvar lotes: {ex.Message}");
            }
        } 
    
        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId,int loteId) {
            try
            {
                var lote = await _service.GetLoteByIdsAsync(eventoId, loteId);
                if(lote == null) return NoContent();
                return  await _service.DeleteLote(lote.EventoId, lote.Id)
                    ? Ok(new { message = "Lote Excluído"})
                    : throw new Exception("Ocorreu um problema ao tentar remover Lote.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar excluir Lote: {ex.Message}");
            }
        } 
    }
}


