using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Contracts.Repositories;
using Hackaton.Application.Contracts.UseCases.Medicos;
using Hackaton.Application.Exceptions;
using Hackaton.Application.Models.Medicos;

namespace Hackaton.Application.UseCases.Medicos
{
    public class MedicoHorariosDisponiveisUseCase : IMedicoHorariosDisponiveisUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioPresenter _usuarioPresenter;
        private readonly IAgendaRepository _agendaRepository;
        private readonly IAgendaPresenter _agendaPresenter;

        public MedicoHorariosDisponiveisUseCase(
            IUsuarioRepository usuarioRepository,
            IUsuarioPresenter usuarioPresenter,
            IAgendaRepository agendaRepository,
            IAgendaPresenter agendaPresenter)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioPresenter = usuarioPresenter;
            _agendaRepository = agendaRepository;
            _agendaPresenter = agendaPresenter;
        }

        public async Task<HorariosDisponiveisMedicoOutputDto> ExecuteAsync(int id)
        {
            var medico = await _usuarioRepository.GetByIdAsync(id);

            if (medico == null)
                throw new NotFoundException("Médico não encontrado");

            var medicoOutput = new HorariosDisponiveisMedicoOutputDto();
            medicoOutput.Medico = _usuarioPresenter.FromEntityToMedicoOutput(medico);
            var agendas = await _agendaRepository.GetAgendasDisponiveisByMedicoId(medico.Id);
            
            foreach (var item in agendas.Where(a => a.Data >= DateOnly.Parse(DateTime.Now.Date.ToShortDateString())))
            {
                medicoOutput.Agendas.Add(_agendaPresenter.FromEntityToDetalhesAgendaDisponivelOutput(item));
            }

            return medicoOutput;
        }
    }
}
