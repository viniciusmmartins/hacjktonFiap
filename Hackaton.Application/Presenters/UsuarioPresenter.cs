using Hackaton.Application.Contracts.Presenters;
using Hackaton.Application.Models.Medicos;
using Hackaton.Application.Models.Usuario.Medico;
using Hackaton.Application.Models.Usuario.Paciente;
using Hackaton.Domain;

namespace Hackaton.Application.Presenters
{
    public class UsuarioPresenter : IUsuarioPresenter
    {
        public UsuarioCadastrarMedicoOutputDto FromEntityToCadastroMedicoOutput(UsuarioEntity usuario)
        {
            return new UsuarioCadastrarMedicoOutputDto
            {
                Id = usuario.Id,
                Cpf = usuario.Cpf,
                Crm = usuario.Crm,
                Email = usuario.Email,
                Estado = usuario.Estado,
                Nome = usuario.Nome,
            };
        }

        public UsuarioCadastrarPacienteOutputDto FromEntityToCadastroPacienteOutput(UsuarioEntity usuario)
        {
            return new UsuarioCadastrarPacienteOutputDto
            {
                Id = usuario.Id,
                Cpf = usuario.Cpf,
                Email = usuario.Email,
                Nome = usuario.Nome,
            };
        }

        public MedicoOutputDto FromEntityToMedicoOutput(UsuarioEntity usuario)
        {
            return new MedicoOutputDto
            {
                Id = usuario.Id,
                Cpf = usuario.Cpf,
                Crm = usuario.Crm,
                Email = usuario.Email,
                Estado = usuario.Estado,
                Nome = usuario.Nome,
            };
        }
    }
}
