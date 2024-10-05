namespace Hackaton.Application.Models.Usuario
{
    public class BaseOutputUsuario
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
    }
}
