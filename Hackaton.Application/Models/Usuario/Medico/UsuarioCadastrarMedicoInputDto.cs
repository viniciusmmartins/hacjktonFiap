using System.ComponentModel.DataAnnotations;

namespace Hackaton.Application.Models.Usuario.Medico
{
    public class UsuarioCadastrarMedicoInputDto : BaseInputUsuario
    {
        [Required]
        public string Crm { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        [MaxLength(2)]
        public string Estado { get; set; } = string.Empty;
    }
}
