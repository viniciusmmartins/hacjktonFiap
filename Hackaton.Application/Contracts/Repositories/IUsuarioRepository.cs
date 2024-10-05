using Hackaton.Application.ObjetoValor;
using Hackaton.Domain;

namespace Hackaton.Application.Contracts.Repositories
{
    public interface IUsuarioRepository : IBaseRepository<UsuarioEntity>
    {
        Task<UsuarioEntity?> GetByCrmAndStateAsync(string crm, string estado);
        Task<UsuarioEntity?> GetByCpfOrEmail(Cpf cpf, Email email);
        Task<IEnumerable<UsuarioEntity>> GetAllMedicos();
        Task<UsuarioEntity?> GetByEmailAndSenhaAsync(string email, string senha);

    }
}
