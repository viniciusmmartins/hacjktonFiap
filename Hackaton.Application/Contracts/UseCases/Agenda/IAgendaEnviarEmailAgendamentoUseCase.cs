using Hackaton.Domain;

namespace Hackaton.Application.Contracts.UseCases.Agenda
{
    public interface IAgendaEnviarEmailAgendamentoUseCase
    {
        Task ExecuteAsync(AgendaEntity agenda);
    }
}
