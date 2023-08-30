using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
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
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAccountService _accountService;
        public EventosController(IEventoService service, IWebHostEnvironment hostEnvironment, 
                                IAccountService accountService)
        {
            _service = service;
            _hostEnvironment = hostEnvironment;
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
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await SaveImage(file);
                }
                var eventoRetorno = await _service.UpdateEvento(User.GetUserId(),eventoId, evento);
                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar adicionar o evento: {ex.Message}");
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
                     DeleteImage(evento.ImagemURL);
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

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10).ToArray()).Replace(' ', '-');
            
            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/Images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create)){
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }
        [NonAction]
        public void DeleteImage(string imageName)
        {
            if (!string.IsNullOrEmpty(imageName)) {
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/Images", imageName);
                if(System.IO.File.Exists(imagePath)){
                    System.IO.File.Delete(imagePath);
                }
            }
        }
    }
}


