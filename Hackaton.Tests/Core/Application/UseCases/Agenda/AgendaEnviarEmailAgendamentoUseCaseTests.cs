using NUnit.Framework;
using Moq;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.Services.Email;
using Hackaton.Application.UseCases.Agenda;
using Hackaton.Domain;
using System.Threading.Tasks;

namespace Hackaton.Tests.Core.Application.UseCases.Agenda
{
    [TestFixture]
    public class AgendaEnviarEmailAgendamentoUseCaseTests
    {
        private Mock<IEmailService> _emailServiceMock;
        private Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private AgendaEnviarEmailAgendamentoUseCase _agendaEnviarEmailAgendamentoUseCase;

        [SetUp]
        public void SetUp()
        {
            _emailServiceMock = new Mock<IEmailService>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _agendaEnviarEmailAgendamentoUseCase = new AgendaEnviarEmailAgendamentoUseCase(_emailServiceMock.Object, _usuarioRepositoryMock.Object);
        }

        [Test]
        public async Task ExecuteAsync_SuccessfulExecution_SendsEmail()
        {
            // Arrange
            var agenda = new AgendaEntity
            {
                MedicoId = 1,
                PacienteId = 2,
                Data = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                HoraInicio = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
                HoraFim = TimeOnly.FromDateTime(DateTime.Now.AddHours(2))
            };
            var medico = new UsuarioEntity
            {
                Id = 1,
                Nome = "Dr. João",
                Email = "dr.joao@example.com"
            };
            var paciente = new UsuarioEntity
            {
                Id = 2,
                Nome = "Maria"
            };

            _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(agenda.MedicoId)).ReturnsAsync(medico);
            _usuarioRepositoryMock.Setup(repo => repo.GetByIdAsync(agenda.PacienteId.Value)).ReturnsAsync(paciente);
            _emailServiceMock.Setup(service => service.SendEmailAsync(medico.Email, It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(Task.CompletedTask);

            // Act
            await _agendaEnviarEmailAgendamentoUseCase.ExecuteAsync(agenda);

            // Assert
            _emailServiceMock.Verify(service => service.SendEmailAsync(
                medico.Email,
                "”Health&Med - Nova consulta agendada",
                It.Is<string>(msg => msg.Contains("Dr. João") && msg.Contains("Maria") && msg.Contains(agenda.Data.ToString()) && msg.Contains(agenda.HoraInicio.ToString()))
            ), Times.Once);
        }
    }
}


