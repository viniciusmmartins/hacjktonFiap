using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Agenda.Cadastrar;
using Hackaton.Application.UseCases.Agenda;
using Hackaton.Domain;
using Moq;

namespace Hackaton.Tests.Core.Application.UseCases.Agenda
{
    [TestFixture]
    public class AgendaCadastrarUseCaseTests
    {
        private Mock<IAgendaRepository> _agendaRepositoryMock;
        private Mock<IAgendaPresenter> _agendaPresenterMock;
        private AgendaCadastrarUseCase _agendaCadastrarUseCase;

        [SetUp]
        public void SetUp()
        {
            _agendaRepositoryMock = new Mock<IAgendaRepository>();
            _agendaPresenterMock = new Mock<IAgendaPresenter>();
            _agendaCadastrarUseCase = new AgendaCadastrarUseCase(_agendaRepositoryMock.Object, _agendaPresenterMock.Object);
        }

        [Test]
        public void ExecuteAsync_AgendaConflitante_ThrowsConflictException()
        {
            // Arrange
            int medicoId = 1;
            var input = new AgendaCadastrarInputDto
            {
                Data = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                HoraInicio = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
                HoraFim = TimeOnly.FromDateTime(DateTime.Now.AddHours(2))
            };
            var agendasConflitantes = new List<AgendaEntity>
            {
                new AgendaEntity
                {
                    MedicoId = medicoId,
                    Data = input.Data,
                    HoraInicio = input.HoraInicio,
                    HoraFim = input.HoraFim,
                    PacienteId = 2 // Já possui um paciente agendado
                }
            };
            _agendaRepositoryMock.Setup(repo => repo.GetAgendasConflitantesByMedicoId(medicoId, input.HoraInicio, input.HoraFim, input.Data))
                                 .ReturnsAsync(agendasConflitantes);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ConflictException>(() => _agendaCadastrarUseCase.ExecuteAsync(input, medicoId));
            Assert.That(ex.Message, Is.EqualTo("Este horário entra em conflito com outro horário cadastrado!"));
        }

        [Test]
        public async Task ExecuteAsync_ValidAgenda_ExecutesSuccessfully()
        {
            // Arrange
            int medicoId = 1;
            var input = new AgendaCadastrarInputDto
            {
                Data = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                HoraInicio = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
                HoraFim = TimeOnly.FromDateTime(DateTime.Now.AddHours(2))
            };
            var agendaEntity = new AgendaEntity
            {
                MedicoId = medicoId,
                Data = input.Data,
                HoraInicio = input.HoraInicio,
                HoraFim = input.HoraFim
            };
            var outputDto = new AgendaCadastrarOutputDto
            {
                Id = agendaEntity.Id,
                Data = agendaEntity.Data,
                HoraInicio = agendaEntity.HoraInicio,
                HoraFim = agendaEntity.HoraFim
            };

            _agendaRepositoryMock.Setup(repo => repo.GetAgendasConflitantesByMedicoId(medicoId, input.HoraInicio, input.HoraFim, input.Data))
                                 .ReturnsAsync(new List<AgendaEntity>());
            _agendaRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<AgendaEntity>())).ReturnsAsync(agendaEntity);
            _agendaPresenterMock.Setup(presenter => presenter.FromEntityToAgendaCadastrarOutput(It.IsAny<AgendaEntity>())).Returns(outputDto);

            // Act
            var result = await _agendaCadastrarUseCase.ExecuteAsync(input, medicoId);

            // Assert
            Assert.AreEqual(outputDto, result);
            _agendaRepositoryMock.Verify(repo => repo.AddAsync(It.Is<AgendaEntity>(a => a.MedicoId == medicoId && a.Data == input.Data && a.HoraInicio == input.HoraInicio && a.HoraFim == input.HoraFim)), Times.Once);
            _agendaPresenterMock.Verify(presenter => presenter.FromEntityToAgendaCadastrarOutput(It.Is<AgendaEntity>(a => a.MedicoId == medicoId && a.Data == input.Data && a.HoraInicio == input.HoraInicio && a.HoraFim == input.HoraFim)), Times.Once);
        }
    }
}
