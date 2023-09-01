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
    public class PalestrantesController : ControllerBase
    {        
        private readonly IPalestranteService _service;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAccountService _accountService;
        public PalestrantesController(IPalestranteService service, IWebHostEnvironment hostEnvironment, 
                                IAccountService accountService)
        {
            _service = service;
            _hostEnvironment = hostEnvironment;
            _accountService = accountService;
        } 
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams) 
        {
            try
            {
                var palestrantes = await _service.GetAllPalestrantesAsync(pageParams, true);
                if(palestrantes == null) return NoContent();    

                Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPages);
                return Ok(palestrantes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar palestrantes: {ex.Message}");
            }
        }
        [HttpGet()]
        public async Task<IActionResult> GetPalestrantes() 
        {
            try
            {
                var palestrante = await _service.GetPalestranteByIdAsync(User.GetUserId(), true);
                if(palestrante == null) return NoContent(); 

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar palestrantes: {ex.Message}");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDto model)
        {
            try
            {
                var palestrante = await _service.GetPalestranteByIdAsync(User.GetUserId(), false);
                if (palestrante == null)
                {
                    palestrante = await _service.AddPalestrantes(User.GetUserId(), model);
                }     
                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar adicionar o palestrante: {ex.Message}");
            }
        }
        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto model) 
        {
            try
            {
                var palestrante = await _service.UpdatePalestrante(User.GetUserId(),model);
                if(palestrante == null) return NoContent();

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar atualizar o palestrante: {ex.Message}");
            }
        }            
        
    }
}


