using NUnit.Framework;
using Moq;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Agenda.Editar;
using Hackaton.Application.UseCases.Agenda;
using Hackaton.Domain;
using System;
using System.Threading.Tasks;

namespace Hackaton.Tests.Core.Application.UseCases.Agenda
{
    [TestFixture]
    public class AgendaEditarUseCaseTests
    {
        private Mock<IAgendaRepository> _agendaRepositoryMock;
        private Mock<IAgendaPresenter> _agendaPresenterMock;
        private AgendaEditarUseCase _agendaEditarUseCase;

        [SetUp]
        public void SetUp()
        {
            _agendaRepositoryMock = new Mock<IAgendaRepository>();
            _agendaPresenterMock = new Mock<IAgendaPresenter>();
            _agendaEditarUseCase = new AgendaEditarUseCase(_agendaRepositoryMock.Object, _agendaPresenterMock.Object);
        }

        [Test]
        public void ExecuteAsync_AgendaIsNull_ThrowsNotFoundException()
        {
            // Arrange
            int medicoId = 1;
            var input = new AgendaEditarInputDto
            {
                Id = 1,
                Data = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                HoraInicio = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
                HoraFim = TimeOnly.FromDateTime(DateTime.Now.AddHours(2))
            };

            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(input.Id)).ReturnsAsync((AgendaEntity)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _agendaEditarUseCase.ExecuteAsync(input, medicoId));
            Assert.That(ex.Message, Is.EqualTo("Agendamento não encontrado"));
        }

        [Test]
        public void ExecuteAsync_MedicoIdDoesNotMatch_ThrowsArgumentException()
        {
            // Arrange
            int medicoId = 1;
            var input = new AgendaEditarInputDto
            {
                Id = 1,
                Data = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                HoraInicio = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
                HoraFim = TimeOnly.FromDateTime(DateTime.Now.AddHours(2))
            };
            var agenda = new AgendaEntity
            {
                Id = input.Id,
                MedicoId = 2, // Diferente do medicoId fornecido
                Data = input.Data,
                HoraInicio = input.HoraInicio,
                HoraFim = input.HoraFim
            };

            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(input.Id)).ReturnsAsync(agenda);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _agendaEditarUseCase.ExecuteAsync(input, medicoId));
            Assert.That(ex.Message, Is.EqualTo("Esse agendamento não pertence a esse médico"));
        }

        [Test]
        public void ExecuteAsync_ConflictingAgendas_ThrowsConflictException()
        {
            // Arrange
            int medicoId = 1;
            var input = new AgendaEditarInputDto
            {
                Id = 1,
                Data = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                HoraInicio = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
                HoraFim = TimeOnly.FromDateTime(DateTime.Now.AddHours(2))
            };
            var agenda = new AgendaEntity
            {
                Id = input.Id,
                MedicoId = medicoId,
                Data = input.Data,
                HoraInicio = input.HoraInicio,
                HoraFim = input.HoraFim
            };
            var conflictingAgenda = new AgendaEntity
            {
                Id = 2, // Diferente do Id da agenda atual
                MedicoId = medicoId,
                Data = input.Data,
                HoraInicio = input.HoraInicio,
                HoraFim = input.HoraFim
            };

            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(input.Id)).ReturnsAsync(agenda);
            _agendaRepositoryMock.Setup(repo => repo.GetAgendasConflitantesByMedicoId(medicoId, input.HoraInicio, input.HoraFim, input.Data))
                                 .ReturnsAsync(new List<AgendaEntity> { conflictingAgenda });

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _agendaEditarUseCase.ExecuteAsync(input, medicoId));
            Assert.That(ex.Message, Is.EqualTo("Este horário entra em conflito com outro horário cadastrado!"));
        }

        [Test]
        public async Task ExecuteAsync_SuccessfulExecution_ReturnsAgendaEditarOutputDto()
        {
            // Arrange
            int medicoId = 1;
            var input = new AgendaEditarInputDto
            {
                Id = 1,
                Data = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                HoraInicio = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
                HoraFim = TimeOnly.FromDateTime(DateTime.Now.AddHours(2))
            };
            var agenda = new AgendaEntity
            {
                Id = input.Id,
                MedicoId = medicoId,
                Data = input.Data,
                HoraInicio = input.HoraInicio,
                HoraFim = input.HoraFim
            };
            var output = new AgendaEditarOutputDto
            {
                Id = agenda.Id,
                Data = agenda.Data,
                HoraInicio = agenda.HoraInicio,
                HoraFim = agenda.HoraFim
            };

            _agendaRepositoryMock.Setup(repo => repo.GetByIdAsync(input.Id)).ReturnsAsync(agenda);
            _agendaRepositoryMock.Setup(repo => repo.GetAgendasConflitantesByMedicoId(medicoId, input.HoraInicio, input.HoraFim, input.Data))
                                 .ReturnsAsync(new List<AgendaEntity>());
            _agendaRepositoryMock.Setup(repo => repo.UpdateAsync(agenda)).ReturnsAsync(agenda);
            _agendaPresenterMock.Setup(presenter => presenter.FromEntityToAgendaEditarOutput(agenda)).Returns(output);

            // Act
            var result = await _agendaEditarUseCase.ExecuteAsync(input, medicoId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(output.Id, result.Id);
            Assert.AreEqual(output.Data, result.Data);
            Assert.AreEqual(output.HoraInicio, result.HoraInicio);
            Assert.AreEqual(output.HoraFim, result.HoraFim);
        }
    }
}
