namespace Hackaton.Application.Contracts.UseCases.Agenda
{
    public interface IAgendaAgendarUseCase
    {
        Task ExecuteAsync(int agendaId, int pacienteId);
    }
}
