using System.ComponentModel.DataAnnotations;

namespace Hackaton.Application.Models.Agenda.Editar
{
    public class AgendaEditarInputDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateOnly Data { get; set; }
        [Required]
        public TimeOnly HoraInicio { get; set; }
        [Required]
        public TimeOnly HoraFim { get; set; }
    }
}
