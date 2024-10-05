using Hackaton.Application.Models.Agenda.Cadastrar;
using Hackaton.Application.Models.Agenda.Editar;
using Hackaton.Application.Models.Medicos;
using Hackaton.Domain;

namespace Hackaton.Application.Contracts.Presenters
{
    public interface IAgendaPresenter
    {
        AgendaCadastrarOutputDto FromEntityToAgendaCadastrarOutput(AgendaEntity agenda);
        AgendaEditarOutputDto FromEntityToAgendaEditarOutput(AgendaEntity agenda);
        DetalhesAgendaDisponivelOutputDto FromEntityToDetalhesAgendaDisponivelOutput(AgendaEntity agenda);
    }
}
