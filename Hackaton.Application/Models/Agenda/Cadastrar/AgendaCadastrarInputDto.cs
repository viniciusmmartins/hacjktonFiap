using System.ComponentModel.DataAnnotations;

namespace Hackaton.Application.Models.Agenda.Cadastrar
{
    public class AgendaCadastrarInputDto
    {
        [Required]
        public DateOnly Data { get; set; }
        [Required]
        public TimeOnly HoraInicio { get; set; }
        [Required]
        public TimeOnly HoraFim { get; set; }
    }
}
