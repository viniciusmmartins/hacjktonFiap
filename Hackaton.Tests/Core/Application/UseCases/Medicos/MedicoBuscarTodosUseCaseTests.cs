using NUnit.Framework;
using Moq;
using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Models.Medicos;
using Hackaton.Application.UseCases.Medicos;
using Hackaton.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackaton.Tests.Core.Application.UseCases.Medicos
{
    [TestFixture]
    public class MedicoBuscarTodosUseCaseTests
    {
        private Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private Mock<IUsuarioPresenter> _usuarioPresenterMock;
        private Mock<IAgendaRepository> _agendaRepositoryMock;
        private MedicoBuscarTodosUseCase _medicoBuscarTodosUseCase;

        [SetUp]
        public void SetUp()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _usuarioPresenterMock = new Mock<IUsuarioPresenter>();
            _agendaRepositoryMock = new Mock<IAgendaRepository>();
            _medicoBuscarTodosUseCase = new MedicoBuscarTodosUseCase(_usuarioRepositoryMock.Object, _usuarioPresenterMock.Object, _agendaRepositoryMock.Object);
        }

        [Test]
        public async Task ExecuteAsync_ReturnsOnlyMedicosWithAvailableAgendas()
        {
            // Arrange
            var medicos = new List<UsuarioEntity>
            {
                new UsuarioEntity { Id = 1, Nome = "Medico 1" },
                new UsuarioEntity { Id = 2, Nome = "Medico 2" }
            };

            var agendasMedico1 = new List<AgendaEntity>
            {
                new AgendaEntity { MedicoId = 1, Data = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) }
            };

            var agendasMedico2 = new List<AgendaEntity>
            {
                new AgendaEntity { MedicoId = 2, Data = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) }
            };

            _usuarioRepositoryMock.Setup(repo => repo.GetAllMedicos()).ReturnsAsync(medicos);
            _agendaRepositoryMock.Setup(repo => repo.GetAgendasDisponiveisByMedicoId(1)).ReturnsAsync(agendasMedico1);
            _agendaRepositoryMock.Setup(repo => repo.GetAgendasDisponiveisByMedicoId(2)).ReturnsAsync(agendasMedico2);
            _usuarioPresenterMock.Setup(presenter => presenter.FromEntityToMedicoOutput(It.IsAny<UsuarioEntity>()))
                .Returns((UsuarioEntity medico) => new MedicoOutputDto { Id = medico.Id, Nome = medico.Nome });

            // Act
            var result = await _medicoBuscarTodosUseCase.ExecuteAsync();

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().Id);
            Assert.AreEqual("Medico 1", result.First().Nome);
        }
    }
}
