namespace Hackaton.Application.Models.Autenticacao.Paciente
{
    public class UsuarioAutenticarPacienteOutputDto
    {
        public string Token { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
    }
}
