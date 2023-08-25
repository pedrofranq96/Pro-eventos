using System;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public decimal Preco { get; set; }  
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        [Required, Range(1, 50, ErrorMessage = "{0} não pode ser menor que 1 e maior que 50")]
        public int Quantidade { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int EventoId { get; set; }
        public EventoDto Evento { get; set; }
    }
}