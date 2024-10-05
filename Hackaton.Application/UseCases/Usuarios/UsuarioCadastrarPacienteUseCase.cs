using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.UseCases.Usuarios;
using Hackaton.Application.Enums;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Usuario.Paciente;
using Hackaton.Application.ObjetoValor;
using Hackaton.Domain;

namespace Hackaton.Application.UseCases.Usuarios
{
    public class UsuarioCadastrarPacienteUseCase : IUsuarioCadastrarPacienteUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioPresenter _usuarioPresenter;

        public UsuarioCadastrarPacienteUseCase(
            IUsuarioRepository usuarioRepository,
            IUsuarioPresenter usuarioPresenter
        )
        {
            _usuarioRepository = usuarioRepository;
            _usuarioPresenter = usuarioPresenter;
        }
        public async Task<UsuarioCadastrarPacienteOutputDto> ExecuteAsync(UsuarioCadastrarPacienteInputDto input)
        {
            var cpf = new Cpf(input.Cpf);
            var email = new Email(input.Email);
            var usuarioEntity = await _usuarioRepository.GetByCpfOrEmail(
                cpf,
                email
            );

            if (usuarioEntity is not null)
            {
                throw new ConflictException($"Usuário já cadastrado com email ou cpf");
            }

            var entity = new UsuarioEntity
            {
                Email = email.Valor,
                Cpf = cpf.Valor,
                Senha = input.Senha,
                Perfil = nameof(EPerfil.Paciente),
                Nome = input.Nome
            };

            await _usuarioRepository.AddAsync(entity);

            return _usuarioPresenter.FromEntityToCadastroPacienteOutput(entity);
        }
    }
}
