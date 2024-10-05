namespace Hackaton.Application.Models.Autenticacao.Medico
{
    public class UsuarioAutenticarMedicoOutputDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CRM { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
