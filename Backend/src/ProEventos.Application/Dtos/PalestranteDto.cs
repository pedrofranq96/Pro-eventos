using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class PalestranteDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string MiniCurriculo { get; set; }
        [RegularExpression(@".*(gif|jpe?g|bmp|png)$",ErrorMessage = "Não é uma imagem válida. (.gif.jpg,jpeg, bmp ou png)")]
        public string ImagemURL { get; set; }
        [Required(ErrorMessage ="O campo {0} é obrigatório."),
         Phone(ErrorMessage = "O campo {0} está com número inválido.")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório."),
         Display(Name = "E-mail"),
         EmailAddress(ErrorMessage = "É necessário ser um {0} válido.")]    
        public string Email { get; set; }
        public IEnumerable<RedeSocialDto> RedesSocias { get; set; }
        public IEnumerable<PalestranteDto> Palestrantes { get; set;} 
    }
}