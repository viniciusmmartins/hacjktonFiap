using Hackaton.Application.Models.Agenda.Cadastrar;

namespace Hackaton.Application.Contracts.UseCases.Agenda
{
    public interface IAgendaCadastrarUseCase
    {
        Task<AgendaCadastrarOutputDto> ExecuteAsync(AgendaCadastrarInputDto input, int medicoId);
    }
}
