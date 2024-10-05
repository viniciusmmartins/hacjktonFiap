using Hackaton.Application.Contracts.UseCases.Medicos;
using Hackaton.Application.Enums;
using Hackaton.Application.Models.Medicos;
using Hackaton.Application.UseCases.Medicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Controllers
{
    [ApiController]
    [Route("/api/medicos")]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicoBuscarTodosUseCase _medicoBuscarTodosUseCase;
        private readonly IMedicoHorariosDisponiveisUseCase _medicoHorariosDisponiveisUseCase;

        public MedicosController(
            IMedicoBuscarTodosUseCase medicoBuscarTodosUseCase,
            IMedicoHorariosDisponiveisUseCase medicoHorariosDisponiveisUseCase)
        {
            _medicoBuscarTodosUseCase = medicoBuscarTodosUseCase;
            _medicoHorariosDisponiveisUseCase = medicoHorariosDisponiveisUseCase;
        }

        [Authorize(Roles = nameof(EPerfil.Paciente))]
        [HttpGet("todos")]
        public async Task<ActionResult<IEnumerable<MedicoOutputDto>>> BuscarTodosAsync()
        {
            return Ok(await _medicoBuscarTodosUseCase.ExecuteAsync());
        }

        [Authorize]
        [HttpGet("horarios-disponiveis/{id:int}")]
        public async Task<ActionResult<IEnumerable<HorariosDisponiveisMedicoOutputDto>>> HorariosDisponiveisMedicoAsync(int id)
        {
            return Ok(await _medicoHorariosDisponiveisUseCase.ExecuteAsync(id));
        }
    }
}
