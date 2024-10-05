using NUnit.Framework;
using Moq;
using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Exceptions;
using Hackaton.Application.UseCases.Medicos;
using System.Threading.Tasks;
using Hackaton.Domain;
using Hackaton.Application.Models.Medicos;

namespace Hackaton.Tests.Core.Application.UseCases.Medicos
{
    [TestFixture]
    public class MedicoHorariosDisponiveisUseCaseTests
    {
        private Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private Mock<IUsuarioPresenter> _usuarioPresenterMock;
        private Mock<IAgendaRepository> _agendaRepositoryMock;
        private Mock<IAgendaPresenter> _agendaPresenterMock;
        private MedicoHorariosDisponiveisUseCase _medicoHorariosDisponiveisUseCase;

        [SetUp]
        public void SetUp()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _usuarioPresenterMock = new Mock<IUsuarioPresenter>();
            _agendaRepositoryMock = new Mock<IAgendaRepository>();
            _agendaPresenterMock = new Mock<IAgendaPresenter>();
            _medicoHorariosDisponiveisUseCase = new MedicoHorariosDisponiveisUseCase(
                _usuarioRepositoryMock.Object,
                _usuarioPresenterMock.Object,
                _agendaRepositoryMock.Object,
                _agendaPresenterMock.Object
            );
        }

        [Test]
        public void ExecuteAsync_MedicoIsNull_ThrowsNotFoundException()
        {
            // Arrange
            int medicoId = 1;
            _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(medicoId)).ReturnsAsync((UsuarioEntity)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _medicoHorariosDisponiveisUseCase.ExecuteAsync(medicoId));
            Assert.That(ex.Message, Is.EqualTo("Médico não encontrado"));
        }

        [Test]
        public async Task ExecuteAsync_SuccessfulExecution_ReturnsHorariosDisponiveisMedicoOutputDto()
        {
            // Arrange
            int medicoId = 1;
            var medico = new UsuarioEntity
            {
                Id = medicoId,
                Nome = "Dr. João",
                Email = "joao@example.com",
                Cpf = "12345678900",
                Crm = "CRM123",
                Estado = "SP"
            };

            var agendas = new List<AgendaEntity>
            {
                new AgendaEntity { Id = 1, MedicoId = medicoId, Data = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), HoraInicio = new TimeOnly(9, 0), HoraFim = new TimeOnly(10, 0) },
                new AgendaEntity { Id = 2, MedicoId = medicoId, Data = DateOnly.FromDateTime(DateTime.Now.AddDays(2)), HoraInicio = new TimeOnly(10, 0), HoraFim = new TimeOnly(11, 0) }
            };

            _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(medicoId)).ReturnsAsync(medico);
            _agendaRepositoryMock.Setup(repo => repo.GetAgendasDisponiveisByMedicoId(medicoId)).ReturnsAsync(agendas);
            _usuarioPresenterMock.Setup(presenter => presenter.FromEntityToMedicoOutput(medico))
                .Returns(new MedicoOutputDto { Id = medico.Id, Nome = medico.Nome, Email = medico.Email, Cpf = medico.Cpf, Crm = medico.Crm, Estado = medico.Estado });
            _agendaPresenterMock.Setup(presenter => presenter.FromEntityToDetalhesAgendaDisponivelOutput(It.IsAny<AgendaEntity>()))
                .Returns((AgendaEntity agenda) => new DetalhesAgendaDisponivelOutputDto { Id = agenda.Id, Data = agenda.Data, HoraInicio = agenda.HoraInicio, HoraFim = agenda.HoraFim });

            // Act
            var result = await _medicoHorariosDisponiveisUseCase.ExecuteAsync(medicoId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(medico.Id, result.Medico.Id);
            Assert.AreEqual(medico.Nome, result.Medico.Nome);
            Assert.AreEqual(2, result.Agendas.Count);
            Assert.AreEqual(agendas[0].Id, result.Agendas[0].Id);
            Assert.AreEqual(agendas[1].Id, result.Agendas[1].Id);
        }
    }
}

