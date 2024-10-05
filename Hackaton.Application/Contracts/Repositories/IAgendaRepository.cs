using Hackaton.Domain;

namespace Hackaton.Application.Contracts.Repositories
{
    public interface IAgendaRepository : IBaseRepository<AgendaEntity>
    {
        public Task<IEnumerable<AgendaEntity>> GetAgendasByMedicoId(int medicoId);
        public Task<IEnumerable<AgendaEntity>> GetAgendasDisponiveisByMedicoId(int medicoId);
        public Task<IEnumerable<AgendaEntity>> GetAgendasConflitantesByMedicoId(
            int medicoId,
            TimeOnly HoraInicio,
            TimeOnly HoraFim,
            DateOnly Data);
    }
}
