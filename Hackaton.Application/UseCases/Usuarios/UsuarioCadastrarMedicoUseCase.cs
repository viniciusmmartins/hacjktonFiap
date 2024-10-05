using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.UseCases.Usuarios;
using Hackaton.Application.Enums;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Usuario.Medico;
using Hackaton.Domain;

namespace Hackaton.Application.UseCases.Usuarios
{
    public class UsuarioCadastrarMedicoUseCase : IUsuarioCadastrarMedicoUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioPresenter _usuarioPresenter;

        public UsuarioCadastrarMedicoUseCase(
            IUsuarioRepository usuarioRepository,
            IUsuarioPresenter usuarioPresenter
        )
        {
            _usuarioRepository = usuarioRepository;
            _usuarioPresenter = usuarioPresenter;
        }

        public async Task<UsuarioCadastrarMedicoOutputDto> ExecuteAsync(UsuarioCadastrarMedicoInputDto input)
        {
            var medicoEntity = await _usuarioRepository.GetByCrmAndStateAsync(input.Crm, input.Estado);
            if(medicoEntity is not null)
            {
                throw new ConflictException($"Médico já cadastrado com CRM e Estado");
            }

            var entity = new UsuarioEntity
            {
                Email = input.Email,
                Crm = input.Crm,
                Cpf = input.Cpf,
                Senha = input.Senha,
                Perfil = nameof(EPerfil.Medico),
                Estado = input.Estado,
                Nome = input.Nome
            };

            await _usuarioRepository.AddAsync(entity);

            return _usuarioPresenter.FromEntityToCadastroMedicoOutput(entity);
        }
    }
}
