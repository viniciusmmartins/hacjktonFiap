using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.Services.Token;
using Hackaton.Application.Contracts.UseCases.Autenticacao;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Autenticacao.Medico;

namespace Hackaton.Application.UseCases.Autenticacao
{
    public class UsuarioAutenticarMedicoUseCase : IUsuarioAutenticarMedicoUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public UsuarioAutenticarMedicoUseCase(
            IUsuarioRepository usuarioRepository, 
            ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        public async Task<UsuarioAutenticarMedicoOutputDto> ExecuteAsync(UsuarioAutenticarMedicoInputDto input)
        {
            var medico = await _usuarioRepository.GetByEmailAndSenhaAsync(input.Email, input.Senha);

            if (medico is null)
            {
                throw new ConflictException($"Dados incorretos");
            }

            return new UsuarioAutenticarMedicoOutputDto
            {
                Id = medico.Id,
                Nome = medico.Nome,
                CRM = medico.Crm,
                Token = _tokenService.GetToken(medico)
            };
        }
    }
}
