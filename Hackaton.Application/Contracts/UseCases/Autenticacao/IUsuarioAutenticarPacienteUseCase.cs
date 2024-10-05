using Hackaton.Application.Models.Autenticacao.Paciente;

namespace Hackaton.Application.Contracts.UseCases.Autenticacao
{
    public interface IUsuarioAutenticarPacienteUseCase
    {
        Task<UsuarioAutenticarPacienteOutputDto> ExecuteAsync(UsuarioAutenticarPacienteInputDto input);
    }
}
