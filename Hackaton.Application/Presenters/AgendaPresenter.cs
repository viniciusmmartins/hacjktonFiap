using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Models.Agenda.Cadastrar;
using Hackaton.Application.Models.Agenda.Editar;
using Hackaton.Application.Models.Medicos;
using Hackaton.Domain;

namespace Hackaton.Application.Presenters
{
    public class AgendaPresenter : IAgendaPresenter
    {
        public AgendaCadastrarOutputDto FromEntityToAgendaCadastrarOutput(AgendaEntity agenda)
        {
            return new AgendaCadastrarOutputDto
            {
                Id = agenda.Id,
                Data = agenda.Data,
                HoraInicio = agenda.HoraInicio,
                HoraFim = agenda.HoraFim
            };
        }

        public AgendaEditarOutputDto FromEntityToAgendaEditarOutput(AgendaEntity agenda)
        {
            return new AgendaEditarOutputDto
            {
                Id = agenda.Id,
                Data = agenda.Data,
                HoraInicio = agenda.HoraInicio,
                HoraFim = agenda.HoraFim
            };
        }

        public DetalhesAgendaDisponivelOutputDto FromEntityToDetalhesAgendaDisponivelOutput(AgendaEntity agenda)
        {
            return new DetalhesAgendaDisponivelOutputDto
            {
                Id = agenda.Id,
                Data = agenda.Data,
                HoraInicio = agenda.HoraInicio,
                HoraFim = agenda.HoraFim
            };
        }
    }
}
