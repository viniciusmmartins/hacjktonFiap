using Hackaton.Application.Contracts.UseCases.Usuarios;
using Hackaton.Application.Models.Usuario.Medico;
using Hackaton.Application.Models.Usuario.Paciente;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Controllers
{
    [ApiController]
    [Route("/api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioCadastrarMedicoUseCase _usuarioCadastrarMedicoUseCase;
        private readonly IUsuarioCadastrarPacienteUseCase _usuarioCadastrarPacienteUseCase;

        public UsuarioController(
            IUsuarioCadastrarMedicoUseCase usuarioCadastrarMedicoUseCase,
            IUsuarioCadastrarPacienteUseCase usuarioCadastrarPacienteUseCase
        )
        {
            _usuarioCadastrarMedicoUseCase = usuarioCadastrarMedicoUseCase;
            _usuarioCadastrarPacienteUseCase = usuarioCadastrarPacienteUseCase;
        }

        [HttpPost("medico")]
        public async Task<ActionResult<UsuarioCadastrarMedicoOutputDto>> CadastrarMedico(UsuarioCadastrarMedicoInputDto input)
        {
            return Ok(await _usuarioCadastrarMedicoUseCase.ExecuteAsync(input));
        }

        [HttpPost("paciente")]
        public async Task<ActionResult<UsuarioCadastrarPacienteOutputDto>> CadastrarPaciente(UsuarioCadastrarPacienteInputDto input)
        {
            return Ok(await _usuarioCadastrarPacienteUseCase.ExecuteAsync(input));
        }
    }
}
