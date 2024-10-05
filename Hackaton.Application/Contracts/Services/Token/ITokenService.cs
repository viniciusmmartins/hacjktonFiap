using Hackaton.Domain;

namespace Hackaton.Application.Contracts.Services.Token
{
    public interface ITokenService
    {
        string GetToken(UsuarioEntity usuario);
    }
}
