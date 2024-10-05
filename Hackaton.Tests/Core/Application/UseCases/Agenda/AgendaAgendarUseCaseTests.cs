using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.UseCases.Agenda;
using Hackaton.Application.Exceptions;
using Hackaton.Application.UseCases.Agenda;
using Hackaton.Domain;
using Moq;

namespace Hackaton.Tests.Core.Application.UseCases.Agenda
{
    [TestFixture]
    public class AgendaAgendarUseCaseTests
    {
        private Mock<IAgendaRepository> _agendaRepositoryMock;
        private Mock<IAgendaEnviarEmailAgendamentoUseCase> _agendaEnviarEmailAgendamentoUseCaseMock;
        private AgendaAgendarUseCase _agendaAgendarUseCase;

        [SetUp]
        public void SetUp()
        {
            _agendaRepositoryMock = new Mock<IAgendaRepository>();
            _agendaEnviarEmailAgendamentoUseCaseMock = new Mock<IAgendaEnviarEmailAgendamentoUseCase>();
            _agendaAgendarUseCase = new AgendaAgendarUseCase(_agendaRepositoryMock.Object, _agendaEnviarEmailAgendamentoUseCaseMock.Object);
        }

        [Test]
        public void ExecuteAsync_AgendaIsNull_ThrowsNotFoundException()
        {
            // Arrange
            int agendaId = 1;
            int pacienteId = 1;
            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(agendaId)).ReturnsAsync((AgendaEntity)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _agendaAgendarUseCase.ExecuteAsync(agendaId, pacienteId));
        }

        [Test]
        public void ExecuteAsync_AgendaDataIsInThePast_ThrowsArgumentException()
        {
            // Arrange
            int agendaId = 1;
            int pacienteId = 1;
            var agenda = new AgendaEntity
            {
                Id = agendaId,
                Data = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), // Data no passado
                HoraInicio = TimeOnly.FromDateTime(DateTime.Now),
                HoraFim = TimeOnly.FromDateTime(DateTime.Now.AddHours(1))
            };
            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(agendaId)).ReturnsAsync(agenda);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _agendaAgendarUseCase.ExecuteAsync(agendaId, pacienteId));
        }

        [Test]
        public void ExecuteAsync_AgendaDataIsTodayAndHoraInicioIsInThePast_ThrowsArgumentException()
        {
            // Arrange
            int agendaId = 1;
            int pacienteId = 1;
            var now = DateTime.Now;
            var agenda = new AgendaEntity
            {
                Id = agendaId,
                Data = DateOnly.FromDateTime(now), // Data de hoje
                HoraInicio = TimeOnly.FromDateTime(now.AddHours(-1)), // Hora no passado
                HoraFim = TimeOnly.FromDateTime(now.AddHours(1))
            };
            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(agendaId)).ReturnsAsync(agenda);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _agendaAgendarUseCase.ExecuteAsync(agendaId, pacienteId));
            Assert.That(ex.Message, Is.EqualTo("Não é possível agendar horários passados"));
        }

        [Test]
        public void ExecuteAsync_AgendaAlreadyHasPaciente_ThrowsConflictException()
        {
            // Arrange
            int agendaId = 1;
            int pacienteId = 1;
            var agenda = new AgendaEntity
            {
                Id = agendaId,
                Data = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                HoraInicio = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
                HoraFim = TimeOnly.FromDateTime(DateTime.Now.AddHours(2)),
                PacienteId = 2
            };
            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(agendaId)).ReturnsAsync(agenda);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _agendaAgendarUseCase.ExecuteAsync(agendaId, pacienteId));
            Assert.That(ex.Message, Is.EqualTo("Horário já agendado"));
        }

        [Test]
        public async Task ExecuteAsync_ValidAgenda_ExecutesSuccessfully()
        {
            // Arrange
            int agendaId = 1;
            int pacienteId = 1;
            var now = DateTime.Now;
            var agenda = new AgendaEntity
            {
                Id = agendaId,
                Data = DateOnly.FromDateTime(now.AddDays(1)), // Data no futuro
                HoraInicio = TimeOnly.FromDateTime(now.AddHours(1)),
                HoraFim = TimeOnly.FromDateTime(now.AddHours(2)),
                PacienteId = null // Sem paciente agendado
            };
            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(agendaId)).ReturnsAsync(agenda);
            _agendaRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<AgendaEntity>())).ReturnsAsync(agenda);
            _agendaEnviarEmailAgendamentoUseCaseMock.Setup(useCase => useCase.ExecuteAsync(It.IsAny<AgendaEntity>())).Returns(Task.CompletedTask);

            // Act
            await _agendaAgendarUseCase.ExecuteAsync(agendaId, pacienteId);

            // Assert
            _agendaRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<AgendaEntity>(a => a.PacienteId == pacienteId)), Times.Once);
            _agendaEnviarEmailAgendamentoUseCaseMock.Verify(useCase => useCase.ExecuteAsync(It.Is<AgendaEntity>(a => a.Id == agendaId)), Times.Once);
        }
    }
}
