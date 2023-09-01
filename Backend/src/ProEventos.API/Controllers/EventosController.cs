using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.API.Helpers;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Persistence.Models;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {        
        private readonly IEventoService _service;
        private readonly IUtil _util;
        private readonly IAccountService _accountService;
        private readonly string _destino = "Images";
        public EventosController(IEventoService service, IUtil util, 
                                IAccountService accountService)
        {
            _service = service;
            _util = util;
            _accountService = accountService;
        } 
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams) 
        {
            try
            {
                var eventos = await _service.GetAllEventosAsync(User.GetUserId(),pageParams, true);
                if(eventos == null) return NoContent();    

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar eventos: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            try
            {
                var evento = await _service.GetEventoPorIdAsync(User.GetUserId(),id, true);
                if(evento == null) return NoContent(); 

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar o evento: {ex.Message}");
            }
        }
        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _service.GetEventoPorIdAsync(User.GetUserId(),eventoId, true);
                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];
                if(file.Length > 0){
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    evento.ImagemURL = await _util.SaveImage(file, _destino);
                }
                var eventoRetorno = await _service.UpdateEvento(User.GetUserId(),eventoId, evento);
                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar adicionar a imagem do evento: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _service.AddEventos(User.GetUserId(),model);
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
        public async Task<IActionResult> Put(int id, EventoDto model) 
        {
            try
            {
                var evento = await _service.UpdateEvento(User.GetUserId(),id, model);
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
        public async Task<IActionResult> Delete(int id) 
        {
            try
            {
                var evento = await _service.GetEventoPorIdAsync(User.GetUserId(),id, true);
                if(evento == null) return NoContent(); 

                if(await _service.DeleteEvento(User.GetUserId(),id)){
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    return Ok(new { message = "Excluído"});
                } else {
                 throw new Exception("Something went wrong");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar excluir o evento: {ex.Message}");
            }
        }

       
    }
}


