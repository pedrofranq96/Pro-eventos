using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _hostEnvironment;

        public EventosController(IEventoService service, IWebHostEnvironment hostEnvironment)
        {
            _service = service;
            _hostEnvironment = hostEnvironment;
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

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId){
            try
            {
                var evento = await _service.GetEventoPorIdAsync(eventoId, true);
                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];
                if(file.Length > 0){
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await SaveImage(file);
                }
                var eventoRetorno = await _service.UpdateEvento(eventoId, evento);
                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar adicionar o evento: {ex.Message}");
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

                if(await _service.DeleteEvento(id)){
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
        public async Task<string> SaveImage(IFormFile imageFile){
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
        public void DeleteImage(string imageName){

            if (!string.IsNullOrEmpty(imageName)) {
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/Images", imageName);
                if(System.IO.File.Exists(imagePath)){
                    System.IO.File.Delete(imagePath);
                }
            }
        }
    }
}


