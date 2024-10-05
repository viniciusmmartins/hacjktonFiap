namespace Hackaton.Application.Models.Medicos
{
    public class HorariosDisponiveisMedicoOutputDto
    {
        public MedicoOutputDto Medico { get; set; }
        public List<DetalhesAgendaDisponivelOutputDto> Agendas { get; set; } = new List<DetalhesAgendaDisponivelOutputDto>();
    }
}
