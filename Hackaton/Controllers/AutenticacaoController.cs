using Hackaton.Application.Contracts.UseCases.Autenticacao;
using Hackaton.Application.Models.Autenticacao.Medico;
using Hackaton.Application.Models.Autenticacao.Paciente;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Controllers
{
    [ApiController]
    [Route("/api/autenticacao")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IUsuarioAutenticarMedicoUseCase _usuarioAutenticarMedicoUseCase;
        private readonly IUsuarioAutenticarPacienteUseCase _usuarioAutenticarPacienteUseCase;

        public AutenticacaoController(IUsuarioAutenticarMedicoUseCase usuarioAutenticarMedicoUseCase, IUsuarioAutenticarPacienteUseCase usuarioAutenticarPacienteUseCase)
        {
            _usuarioAutenticarMedicoUseCase = usuarioAutenticarMedicoUseCase;
            _usuarioAutenticarPacienteUseCase = usuarioAutenticarPacienteUseCase;
        }

        [HttpPost("medico")]
        public async Task<ActionResult<UsuarioAutenticarMedicoOutputDto>> CadastrarMedico(UsuarioAutenticarMedicoInputDto input)
        {
            return Ok(await _usuarioAutenticarMedicoUseCase.ExecuteAsync(input));
        }

        [HttpPost("paciente")]
        public async Task<ActionResult<UsuarioAutenticarPacienteOutputDto>> CadastrarPaciente(UsuarioAutenticarPacienteInputDto input)
        {
            return Ok(await _usuarioAutenticarPacienteUseCase.ExecuteAsync(input));
        }
    }
}
