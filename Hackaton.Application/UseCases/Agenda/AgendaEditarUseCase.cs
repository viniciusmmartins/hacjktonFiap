using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.UseCases.Agenda;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Agenda.Editar;
using Hackaton.Domain;

namespace Hackaton.Application.UseCases.Agenda
{
    public class AgendaEditarUseCase : IAgendaEditarUseCase
    {
        private readonly IAgendaRepository _agendaRepository;
        private readonly IAgendaPresenter _agendaPresenter;

        public AgendaEditarUseCase(
            IAgendaRepository agendaRepository, 
            IAgendaPresenter agendaPresenter)
        {
            _agendaRepository = agendaRepository;
            _agendaPresenter = agendaPresenter;
        }

        public async Task<AgendaEditarOutputDto> ExecuteAsync(AgendaEditarInputDto input, int medicoId)
        {
            var agenda = await _agendaRepository.GetByIdAsync(input.Id);

            if (agenda is null)
            {
                throw new NotFoundException($"Agendamento não encontrado");
            }
            else if (agenda.MedicoId != medicoId)
            {
                throw new ArgumentException($"Esse agendamento não pertence a esse médico");
            }

            var agendasConlitantes = await _agendaRepository.GetAgendasConflitantesByMedicoId(
                medicoId,
                input.HoraInicio,
                input.HoraFim,
                input.Data);

            if (agendasConlitantes.Any(a => a.Id != agenda.Id))
            {
                throw new ConflictException($"Este horário entra em conflito com outro horário cadastrado!");
            }

            agenda.Data = input.Data;
            agenda.HoraInicio = input.HoraInicio;
            agenda.HoraFim = input.HoraFim;

            await _agendaRepository.UpdateAsync(agenda);

            return _agendaPresenter.FromEntityToAgendaEditarOutput(agenda);
        }
    }
}
