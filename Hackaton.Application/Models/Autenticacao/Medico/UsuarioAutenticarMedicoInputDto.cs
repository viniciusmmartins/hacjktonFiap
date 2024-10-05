using Hackaton.Application.ObjetoValor;

namespace Hackaton.Application.Models.Autenticacao.Medico
{
    public class UsuarioAutenticarMedicoInputDto
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
