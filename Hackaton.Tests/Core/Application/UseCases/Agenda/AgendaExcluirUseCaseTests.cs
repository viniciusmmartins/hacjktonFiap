using NUnit.Framework;
using Moq;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Exceptions;
using Hackaton.Application.UseCases.Agenda;
using System.Threading.Tasks;
using Hackaton.Domain;

namespace Hackaton.Tests.Core.Application.UseCases.Agenda
{
    [TestFixture]
    public class AgendaExcluirUseCaseTests
    {
        private Mock<IAgendaRepository> _agendaRepositoryMock;
        private AgendaExcluirUseCase _agendaExcluirUseCase;

        [SetUp]
        public void SetUp()
        {
            _agendaRepositoryMock = new Mock<IAgendaRepository>();
            _agendaExcluirUseCase = new AgendaExcluirUseCase(_agendaRepositoryMock.Object);
        }

        [Test]
        public void ExecuteAsync_AgendaIsNull_ThrowsNotFoundException()
        {
            // Arrange
            int agendaId = 1;
            int medicoId = 1;

            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(agendaId)).ReturnsAsync((AgendaEntity)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _agendaExcluirUseCase.ExecuteAsync(agendaId, medicoId));
            Assert.That(ex.Message, Is.EqualTo("Agendamento não encontrado"));
        }

        [Test]
        public void ExecuteAsync_MedicoIdDoesNotMatch_ThrowsArgumentException()
        {
            // Arrange
            int agendaId = 1;
            int medicoId = 1;
            var agenda = new AgendaEntity
            {
                Id = agendaId,
                MedicoId = 2 // Diferente do medicoId fornecido
            };

            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(agendaId)).ReturnsAsync(agenda);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _agendaExcluirUseCase.ExecuteAsync(agendaId, medicoId));
            Assert.That(ex.Message, Is.EqualTo("Esse agendamento não pertence a esse médico"));
        }

        [Test]
        public async Task ExecuteAsync_SuccessfulExecution_DeletesAgenda()
        {
            // Arrange
            int agendaId = 1;
            int medicoId = 1;
            var agenda = new AgendaEntity
            {
                Id = agendaId,
                MedicoId = medicoId
            };

            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(agendaId)).ReturnsAsync(agenda);
            _agendaRepositoryMock.Setup(repo => repo.DeleteAsync(agenda)).Returns(Task.CompletedTask);

            // Act
            await _agendaExcluirUseCase.ExecuteAsync(agendaId, medicoId);

            // Assert
            _agendaRepositoryMock.Verify(repo => repo.DeleteAsync(agenda), Times.Once);
        }
    }
}


