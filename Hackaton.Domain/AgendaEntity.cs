namespace Hackaton.Domain
{
    public class AgendaEntity : BaseEntity
    {
        public int MedicoId { get; set; }
        public int? PacienteId { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }

    }
}
