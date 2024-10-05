using Hackaton.Application.Models.Agenda.Editar;

namespace Hackaton.Application.Contracts.UseCases.Agenda
{
    public interface IAgendaEditarUseCase
    {
        Task<AgendaEditarOutputDto> ExecuteAsync(AgendaEditarInputDto input, int medicoId);
    }
}
