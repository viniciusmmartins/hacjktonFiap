using Hackaton.Application.Models.Medicos;

namespace Hackaton.Application.Contracts.UseCases.Medicos
{
    public interface IMedicoBuscarTodosUseCase
    {
        Task<IEnumerable<MedicoOutputDto>> ExecuteAsync();
    }
}
