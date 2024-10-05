using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Enums;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Usuario.Paciente;
using Hackaton.Application.ObjetoValor;
using Hackaton.Application.UseCases.Usuarios;
using Hackaton.Domain;
using Moq;

namespace Hackaton.Tests.Core.Application.UseCases.Usuarios
{
    [TestFixture]
    public class UsuarioCadastrarPacienteUseCaseTests
    {
        private Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private Mock<IUsuarioPresenter> _usuarioPresenterMock;
        private UsuarioCadastrarPacienteUseCase _usuarioCadastrarPacienteUseCase;

        [SetUp]
        public void SetUp()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _usuarioPresenterMock = new Mock<IUsuarioPresenter>();
            _usuarioCadastrarPacienteUseCase = new UsuarioCadastrarPacienteUseCase(
                _usuarioRepositoryMock.Object,
                _usuarioPresenterMock.Object
            );
        }

        [Test]
        public void ExecuteAsync_UsuarioEntityIsNotNull_ThrowsConflictException()
        {
            // Arrange
            var input = new UsuarioCadastrarPacienteInputDto
            {
                Email = "paciente@example.com",
                Cpf = "12345678900",
                Senha = "senha123",
                Nome = "João"
            };

            var existingUsuario = new UsuarioEntity
            {
                Id = 1,
                Email = input.Email,
                Cpf = input.Cpf,
                Senha = input.Senha,
                Nome = input.Nome,
                Perfil = nameof(EPerfil.Paciente)
            };

            var cpf = new Cpf(input.Cpf);
            var email = new Email(input.Email);

            _usuarioRepositoryMock.Setup(repo => repo.GetByCpfOrEmail(It.IsAny<Cpf>(), It.IsAny<Email>())).ReturnsAsync(existingUsuario);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _usuarioCadastrarPacienteUseCase.ExecuteAsync(input));
            Assert.That(ex.Message, Is.EqualTo("Usuário já cadastrado com email ou cpf"));
        }

        [Test]
        public async Task ExecuteAsync_SuccessfulExecution_ReturnsUsuarioCadastrarPacienteOutputDto()
        {
            // Arrange
            var input = new UsuarioCadastrarPacienteInputDto
            {
                Email = "paciente@example.com",
                Cpf = "12345678900",
                Senha = "senha123",
                Nome = "João"
            };

            var newPaciente = new UsuarioEntity
            {
                Id = 1,
                Email = input.Email,
                Cpf = input.Cpf,
                Senha = input.Senha,
                Nome = input.Nome,
                Perfil = nameof(EPerfil.Paciente)
            };

            var expectedOutput = new UsuarioCadastrarPacienteOutputDto
            {
                Id = newPaciente.Id,
                Email = newPaciente.Email,
                Cpf = newPaciente.Cpf,
                Nome = newPaciente.Nome
            };

            var cpf = new Cpf(input.Cpf);
            var email = new Email(input.Email);

            _usuarioRepositoryMock.Setup(repo => repo.GetByCpfOrEmail(cpf, email)).ReturnsAsync((UsuarioEntity)null);
            _usuarioRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<UsuarioEntity>())).ReturnsAsync(newPaciente);
            _usuarioPresenterMock.Setup(presenter => presenter.FromEntityToCadastroPacienteOutput(It.IsAny<UsuarioEntity>())).Returns(expectedOutput);

            // Act
            var result = await _usuarioCadastrarPacienteUseCase.ExecuteAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOutput.Id, result.Id);
            Assert.AreEqual(expectedOutput.Email, result.Email);
            Assert.AreEqual(expectedOutput.Cpf, result.Cpf);
            Assert.AreEqual(expectedOutput.Nome, result.Nome);
        }
    }
}

