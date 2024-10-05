using NUnit.Framework;
using Moq;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.Services.Token;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Autenticacao.Paciente;
using Hackaton.Application.UseCases.Autenticacao;
using System.Threading.Tasks;
using Hackaton.Domain;

namespace Hackaton.Tests.Core.Application.UseCases.Autenticacao
{
    [TestFixture]
    public class UsuarioAutenticarPacienteUseCaseTests
    {
        private Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private Mock<ITokenService> _tokenServiceMock;
        private UsuarioAutenticarPacienteUseCase _usuarioAutenticarPacienteUseCase;

        [SetUp]
        public void SetUp()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _usuarioAutenticarPacienteUseCase = new UsuarioAutenticarPacienteUseCase(_usuarioRepositoryMock.Object, _tokenServiceMock.Object);
        }

        [Test]
        public void ExecuteAsync_PacienteIsNull_ThrowsConflictException()
        {
            // Arrange
            var input = new UsuarioAutenticarPacienteInputDto
            {
                Email = "paciente@example.com",
                Senha = "senha123"
            };

            _usuarioRepositoryMock.Setup(repo => repo.GetByEmailAndSenhaAsync(input.Email, input.Senha)).ReturnsAsync((UsuarioEntity)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _usuarioAutenticarPacienteUseCase.ExecuteAsync(input));
            Assert.That(ex.Message, Is.EqualTo("Dados incorretos"));
        }

        [Test]
        public async Task ExecuteAsync_SuccessfulExecution_ReturnsOutputDto()
        {
            // Arrange
            var input = new UsuarioAutenticarPacienteInputDto
            {
                Email = "paciente@example.com",
                Senha = "senha123"
            };

            var paciente = new UsuarioEntity
            {
                Id = 1,
                Nome = "Paciente",
                Cpf = "12345678900",
                Email = input.Email,
                Senha = input.Senha
            };

            var expectedToken = "token123";

            _usuarioRepositoryMock.Setup(repo => repo.GetByEmailAndSenhaAsync(input.Email, input.Senha)).ReturnsAsync(paciente);
            _tokenServiceMock.Setup(service => service.GetToken(paciente)).Returns(expectedToken);

            // Act
            var result = await _usuarioAutenticarPacienteUseCase.ExecuteAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(paciente.Nome, result.Nome);
            Assert.AreEqual(paciente.Cpf, result.Cpf);
            Assert.AreEqual(expectedToken, result.Token);
        }
    }
}



