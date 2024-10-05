using System.ComponentModel.DataAnnotations;

namespace Hackaton.Application.Models.Usuario
{
    public class BaseInputUsuario
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MinLength(4)]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public string Cpf { get; set; } = string.Empty;

        [Required]
        public string Nome { get; set; } = string.Empty;
    }
}
