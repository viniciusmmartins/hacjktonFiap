using System.ComponentModel.DataAnnotations;

namespace Hackaton.Application.Models.Agenda.Cadastrar
{
    public class AgendaCadastrarOutputDto
    {
        public int Id { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }
    }
}
