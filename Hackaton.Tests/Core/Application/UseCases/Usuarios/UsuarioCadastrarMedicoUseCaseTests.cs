using NUnit.Framework;
using Moq;
using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Usuario.Medico;
using Hackaton.Application.UseCases.Usuarios;
using Hackaton.Domain;
using System.Threading.Tasks;
using Hackaton.Application.Enums;

namespace Hackaton.Tests.Core.Application.UseCases.Usuarios
{
    [TestFixture]
    public class UsuarioCadastrarMedicoUseCaseTests
    {
        private Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private Mock<IUsuarioPresenter> _usuarioPresenterMock;
        private UsuarioCadastrarMedicoUseCase _usuarioCadastrarMedicoUseCase;

        [SetUp]
        public void SetUp()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _usuarioPresenterMock = new Mock<IUsuarioPresenter>();
            _usuarioCadastrarMedicoUseCase = new UsuarioCadastrarMedicoUseCase(
                _usuarioRepositoryMock.Object,
                _usuarioPresenterMock.Object
            );
        }

        [Test]
        public void ExecuteAsync_MedicoEntityIsNotNull_ThrowsConflictException()
        {
            // Arrange
            var input = new UsuarioCadastrarMedicoInputDto
            {
                Email = "medico@example.com",
                Crm = "CRM123",
                Cpf = "12345678900",
                Senha = "senha123",
                Estado = "SP",
                Nome = "Dr. João"
            };

            var existingMedico = new UsuarioEntity
            {
                Id = 1,
                Email = "medico@example.com",
                Crm = "CRM123",
                Cpf = "12345678900",
                Senha = "senha123",
                Estado = "SP",
                Nome = "Dr. João"
            };

            _usuarioRepositoryMock.Setup(repo => repo.GetByCrmAndStateAsync(input.Crm, input.Estado)).ReturnsAsync(existingMedico);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _usuarioCadastrarMedicoUseCase.ExecuteAsync(input));
            Assert.That(ex.Message, Is.EqualTo("Médico já cadastrado com CRM e Estado"));
        }

        [Test]
        public async Task ExecuteAsync_SuccessfulExecution_ReturnsUsuarioCadastrarMedicoOutputDto()
        {
            // Arrange
            var input = new UsuarioCadastrarMedicoInputDto
            {
                Email = "medico@example.com",
                Crm = "CRM123",
                Cpf = "12345678900",
                Senha = "senha123",
                Estado = "SP",
                Nome = "Dr. João"
            };

            var newMedico = new UsuarioEntity
            {
                Id = 1,
                Email = input.Email,
                Crm = input.Crm,
                Cpf = input.Cpf,
                Senha = input.Senha,
                Estado = input.Estado,
                Nome = input.Nome,
                Perfil = nameof(EPerfil.Medico)
            };

            var expectedOutput = new UsuarioCadastrarMedicoOutputDto
            {
                Id = newMedico.Id,
                Email = newMedico.Email,
                Cpf = newMedico.Cpf,
                Nome = newMedico.Nome,
                Crm = newMedico.Crm,
                Estado = newMedico.Estado
            };

            _usuarioRepositoryMock.Setup(repo => repo.GetByCrmAndStateAsync(input.Crm, input.Estado)).ReturnsAsync((UsuarioEntity)null);
            _usuarioRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<UsuarioEntity>())).ReturnsAsync(newMedico);
            _usuarioPresenterMock.Setup(presenter => presenter.FromEntityToCadastroMedicoOutput(It.IsAny<UsuarioEntity>())).Returns(expectedOutput);

            // Act
            var result = await _usuarioCadastrarMedicoUseCase.ExecuteAsync(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOutput.Id, result.Id);
            Assert.AreEqual(expectedOutput.Email, result.Email);
            Assert.AreEqual(expectedOutput.Cpf, result.Cpf);
            Assert.AreEqual(expectedOutput.Nome, result.Nome);
            Assert.AreEqual(expectedOutput.Crm, result.Crm);
            Assert.AreEqual(expectedOutput.Estado, result.Estado);
        }
    }
}

