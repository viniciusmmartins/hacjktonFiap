using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Enums;
using Hackaton.Data.DataContext;
using Hackaton.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Data.Repositories
{
    public class AgendaRepository : BaseRepository<AgendaEntity>, IAgendaRepository
    {
        public AgendaRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AgendaEntity>> GetAgendasDisponiveisByMedicoId(int medicoId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.MedicoId.Equals(medicoId) && !p.PacienteId.HasValue)
                .ToListAsync();
        }

        public async Task<IEnumerable<AgendaEntity>> GetAgendasConflitantesByMedicoId(
            int medicoId,
            TimeOnly HoraInicio,
            TimeOnly HoraFim,
            DateOnly Data)
        {
            return await _dbSet
               .AsNoTracking()
               .Where(p => p.MedicoId == medicoId
                           && p.Data == Data
                           && ((p.HoraInicio < HoraFim && p.HoraFim > HoraInicio)
                               || (p.HoraInicio >= HoraInicio && p.HoraInicio < HoraFim)
                               || (p.HoraFim > HoraInicio && p.HoraFim <= HoraFim)))
               .ToListAsync();
        }

        public async Task<IEnumerable<AgendaEntity>> GetAgendasByMedicoId(int medicoId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.MedicoId.Equals(medicoId))
                .ToListAsync();
        }
    }
}
