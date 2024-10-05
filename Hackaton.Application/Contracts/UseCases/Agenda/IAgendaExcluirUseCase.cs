namespace Hackaton.Application.Contracts.UseCases.Agenda
{
    public interface IAgendaExcluirUseCase
    {
        Task ExecuteAsync(int agendaId, int medicoId);
    }
}
