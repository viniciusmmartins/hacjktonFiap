using Hackaton.Application.Models.Medicos;

namespace Hackaton.Application.Contracts.UseCases.Medicos
{
    public interface IMedicoHorariosDisponiveisUseCase
    {
        Task<HorariosDisponiveisMedicoOutputDto> ExecuteAsync(int id);
    }
}
