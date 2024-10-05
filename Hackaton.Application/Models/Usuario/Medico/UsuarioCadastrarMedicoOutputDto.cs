namespace Hackaton.Application.Models.Usuario.Medico
{
    public class UsuarioCadastrarMedicoOutputDto : BaseOutputUsuario
    {
        public string Crm { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
