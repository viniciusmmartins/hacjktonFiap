using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.Services.Token;
using Hackaton.Application.Contracts.UseCases.Autenticacao;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Autenticacao.Paciente;

namespace Hackaton.Application.UseCases.Autenticacao
{
    public class UsuarioAutenticarPacienteUseCase : IUsuarioAutenticarPacienteUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public UsuarioAutenticarPacienteUseCase(
            IUsuarioRepository usuarioRepository, 
            ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        public async Task<UsuarioAutenticarPacienteOutputDto> ExecuteAsync(UsuarioAutenticarPacienteInputDto input)
        {
            var paciente = await _usuarioRepository.GetByEmailAndSenhaAsync(input.Email, input.Senha);

            if (paciente is null)
            {
                throw new ConflictException($"Dados incorretos");
            }

            return new UsuarioAutenticarPacienteOutputDto
            {
                Nome = paciente.Nome,
                Cpf = paciente.Cpf,
                Token = _tokenService.GetToken(paciente)
            };
        }
    }
}
