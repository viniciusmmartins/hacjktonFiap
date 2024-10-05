namespace Hackaton.Application.Models.Medicos
{
    public class DetalhesAgendaDisponivelOutputDto
    {
        public int Id { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }
    }
}
