using Hackaton.Application.Models.Autenticacao.Medico;

namespace Hackaton.Application.Contracts.UseCases.Autenticacao
{
    public interface IUsuarioAutenticarMedicoUseCase
    {
        Task<UsuarioAutenticarMedicoOutputDto> ExecuteAsync(UsuarioAutenticarMedicoInputDto input);
    }
}
