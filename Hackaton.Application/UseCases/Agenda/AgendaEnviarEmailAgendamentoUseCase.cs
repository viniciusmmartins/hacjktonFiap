using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.Services.Email;
using Hackaton.Application.Contracts.UseCases.Agenda;
using Hackaton.Domain;

namespace Hackaton.Application.UseCases.Agenda
{
    public class AgendaEnviarEmailAgendamentoUseCase : IAgendaEnviarEmailAgendamentoUseCase
    {
        private readonly IEmailService _emailService;
        private readonly IUsuarioRepository _usuarioRepository;

        public AgendaEnviarEmailAgendamentoUseCase(
            IEmailService emailService, 
            IUsuarioRepository usuarioRepository)
        {
            _emailService = emailService;
            _usuarioRepository = usuarioRepository;
        }

        public async Task ExecuteAsync(AgendaEntity agenda)
        {
            var medico = await _usuarioRepository.GetByIdAsync(agenda.MedicoId);
            var paciente = await _usuarioRepository.GetByIdAsync(agenda.PacienteId.Value);

            var mensagem = @$"Olá, Dr.{medico.Nome}!                
                Você tem uma nova consulta marcada! 
                Paciente: {paciente.Nome}.
                Date e horário: {agenda.Data} às {agenda.HoraInicio}.";

            await _emailService.SendEmailAsync(medico.Email, "”Health&Med - Nova consulta agendada", mensagem);
        }
    }
}
