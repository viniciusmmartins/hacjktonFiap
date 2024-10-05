using Hackaton.Application.Models.Usuario.Paciente;

namespace Hackaton.Application.Contracts.UseCases.Usuarios
{
    public interface IUsuarioCadastrarPacienteUseCase
    {
        Task<UsuarioCadastrarPacienteOutputDto> ExecuteAsync(UsuarioCadastrarPacienteInputDto input);
    }
}
