using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.UseCases.Agenda;
using Hackaton.Application.Exceptions;
using Hackaton.Domain;

namespace Hackaton.Application.UseCases.Agenda
{
    public class AgendaExcluirUseCase : IAgendaExcluirUseCase
    {
        private readonly IAgendaRepository _agendaRepository;

        public AgendaExcluirUseCase(IAgendaRepository agendaRepository)
        {
            _agendaRepository = agendaRepository;
        }

        public async Task ExecuteAsync(int agendaId, int medicoId)
        {
            var agenda = await _agendaRepository.GetByIdAsync(agendaId);

            if (agenda is null)
            {
                throw new NotFoundException($"Agendamento não encontrado");
            }
            else if(agenda.MedicoId != medicoId)
            {
                throw new ArgumentException($"Esse agendamento não pertence a esse médico");
            }

            await _agendaRepository.DeleteAsync(agenda);
        }
    }
}
