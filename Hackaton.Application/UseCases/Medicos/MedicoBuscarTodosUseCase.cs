using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.UseCases.Medicos;
using Hackaton.Application.Models.Medicos;

namespace Hackaton.Application.UseCases.Medicos
{
    public class MedicoBuscarTodosUseCase : IMedicoBuscarTodosUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioPresenter _usuarioPresenter;
        private readonly IAgendaRepository _agendaRepository;

        public MedicoBuscarTodosUseCase(
            IUsuarioRepository usuarioRepository,
            IUsuarioPresenter usuarioPresenter,
            IAgendaRepository agendaRepository
        )
        {
            _usuarioRepository = usuarioRepository;
            _usuarioPresenter = usuarioPresenter;
            _agendaRepository = agendaRepository;
        }

        public async Task<IEnumerable<MedicoOutputDto>> ExecuteAsync()
        {
            var data = await _usuarioRepository.GetAllMedicos();

            var medicosComAgenda = new List<MedicoOutputDto>();

            foreach (var item in data)
            {
                var agendas = await _agendaRepository.GetAgendasDisponiveisByMedicoId(item.Id);
                if (!agendas.Any(a => a.Data >= DateOnly.Parse(DateTime.Now.Date.ToShortDateString())))
                {
                    continue;
                }

                medicosComAgenda.Add(_usuarioPresenter.FromEntityToMedicoOutput(item));
            }

            return medicosComAgenda;
        }
    }
}
