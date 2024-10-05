using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.UseCases.Agenda;
using Hackaton.Application.Exceptions;

namespace Hackaton.Application.UseCases.Agenda
{
    public class AgendaAgendarUseCase : IAgendaAgendarUseCase
    {
        private readonly IAgendaRepository _agendaRepository;
        private readonly IAgendaEnviarEmailAgendamentoUseCase _agendaEnviarEmailAgendamentoUseCase;

        public AgendaAgendarUseCase(
            IAgendaRepository agendaRepository,
            IAgendaEnviarEmailAgendamentoUseCase agendaEnviarEmailAgendamentoUseCase)
        {
            _agendaRepository = agendaRepository;
            _agendaEnviarEmailAgendamentoUseCase = agendaEnviarEmailAgendamentoUseCase;
        }

        public async Task ExecuteAsync(int agendaId, int pacienteId)
        {
            var agenda = await _agendaRepository.GetByIdAsync(agendaId);

            if (agenda == null)
            {
                throw new NotFoundException("Agenda não existente");
            }
            else if (agenda.Data < DateOnly.Parse(DateTime.Now.Date.ToShortDateString()))
            {
                throw new ArgumentException("Não é possível agendar horários passados");
            }
            else if (agenda.Data == DateOnly.Parse(DateTime.Now.Date.ToShortDateString()) && 
                agenda.HoraInicio < TimeOnly.Parse(DateTime.Now.TimeOfDay.ToString()))
            {
                throw new ArgumentException("Não é possível agendar horários passados");
            }
            else if (agenda.PacienteId != null)
            {
                throw new ConflictException("Horário já agendado");
            }

            agenda.PacienteId = pacienteId;
            await _agendaRepository.UpdateAsync(agenda);

            await _agendaEnviarEmailAgendamentoUseCase.ExecuteAsync(agenda);
        }
    }
}
