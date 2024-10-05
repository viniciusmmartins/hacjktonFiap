using Hackaton.Application.Models.Medicos;
using Hackaton.Application.Models.Usuario.Medico;
using Hackaton.Application.Models.Usuario.Paciente;
using Hackaton.Domain;

namespace Hackaton.Application.Contracts.Presenters
{
    public interface IUsuarioPresenter
    {
        UsuarioCadastrarMedicoOutputDto FromEntityToCadastroMedicoOutput(UsuarioEntity usuario);
        UsuarioCadastrarPacienteOutputDto FromEntityToCadastroPacienteOutput(UsuarioEntity usuario);
        MedicoOutputDto FromEntityToMedicoOutput(UsuarioEntity usuario);
    }
}
