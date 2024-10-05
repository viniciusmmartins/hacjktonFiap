namespace Hackaton.Application.Models.Medicos
{
    public class MedicoOutputDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Crm { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
