namespace Hackaton.Domain
{
    public class UsuarioEntity : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Crm { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty;

        public IEnumerable<AgendaEntity> Agendas { get; set; } = [];
    }
}
