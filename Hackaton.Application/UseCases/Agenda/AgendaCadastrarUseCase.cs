using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.UseCases.Agenda;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Agenda.Cadastrar;
using Hackaton.Domain;

namespace Hackaton.Application.UseCases.Agenda
{
    public class AgendaCadastrarUseCase : IAgendaCadastrarUseCase
    {
        private readonly IAgendaRepository _agendaRepository;
        private readonly IAgendaPresenter _agendaPresenter;

        public AgendaCadastrarUseCase(
            IAgendaRepository agendaRepository, 
            IAgendaPresenter agendaPresenter)
        {
            _agendaRepository = agendaRepository;
            _agendaPresenter = agendaPresenter;
        }

        public async Task<AgendaCadastrarOutputDto> ExecuteAsync(AgendaCadastrarInputDto input, int medicoId)
        {
            var agendasConlitantes = await _agendaRepository.GetAgendasConflitantesByMedicoId(
                medicoId,
                input.HoraInicio,
                input.HoraFim,
                input.Data);

            if (agendasConlitantes.Any())
            {
                throw new ConflictException($"Este horário entra em conflito com outro horário cadastrado!");
            }

            var entity = new AgendaEntity
            {
                MedicoId = medicoId,
                Data = input.Data,
                HoraInicio = input.HoraInicio,
                HoraFim = input.HoraFim
            };

            await _agendaRepository.AddAsync(entity);

            return _agendaPresenter.FromEntityToAgendaCadastrarOutput(entity);
        }
    }
}
