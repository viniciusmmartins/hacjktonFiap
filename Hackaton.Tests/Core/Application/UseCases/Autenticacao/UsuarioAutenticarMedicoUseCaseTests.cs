using NUnit.Framework;
using Moq;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.Services.Token;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Autenticacao.Medico;
using Hackaton.Application.UseCases.Autenticacao;
using System.Threading.Tasks;
using Hackaton.Domain;

namespace Hackaton.Tests.Core.Application.UseCases.Autenticacao
{
    [TestFixture]
    public class UsuarioAutenticarMedicoUseCaseTests
    {
        private Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private Mock<ITokenService> _tokenServiceMock;
        private UsuarioAutenticarMedicoUseCase _usuarioAutenticarMedicoUseCase;

        [SetUp]
        public void SetUp()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _usuarioAutenticarMedicoUseCase = new UsuarioAutenticarMedicoUseCase(_usuarioRepositoryMock.Object, _tokenServiceMock.Object);
        }

        [Test]
        public void ExecuteAsync_MedicoIsNull_ThrowsConflictException()
        {
            // Arrange
            var input = new UsuarioAutenticarMedicoInputDto
            {
                Email = "medico@example.com",
                Senha = "senha123"
            };

            _usuarioRepositoryMock.Setup(repo => repo.GetByEmailAndSenhaAsync(input.Email, input.Senha)).ReturnsAsync((UsuarioEntity)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _usuarioAutenticarMedicoUseCase.ExecuteAsync(input));
            Assert.That(ex.Message, Is.EqualTo("Dados incorretos"));
        }

        [Test]
        public async Task ExecuteAsync_SuccessfulExecution_ReturnsOutputDto()
        {
            // Arrange
            var input = new UsuarioAutenticarMedicoInputDto
            {
                Email = "medico@example.com",
                Senha = "senha123"
            };

            var medico = new UsuarioEntity
            {
                Id = 1,
                Nome = "Dr. Medico",
                Crm = "12345",
                Email = input.Email,
                Senha = input.Senha
            };

            var expectedToken = "token123";

            _usuarioRepositoryMock.Setup(repo => repo.GetByEmailAndSenhaAsync(input.Email, input.Senha)).ReturnsAsync(medico);
            _tokenServiceMock.Setup(service => service.GetToken(medico)).Returns(expectedToken);

            // Act
            var result = await _usuarioAutenticarMedicoUseCase.ExecuteAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(medico.Id, result.Id);
            Assert.AreEqual(medico.Nome, result.Nome);
            Assert.AreEqual(medico.Crm, result.CRM);
            Assert.AreEqual(expectedToken, result.Token);
        }

    }
}


