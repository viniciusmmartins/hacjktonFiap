using Hackaton.Domain;

namespace Hackaton.Application.Contracts.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);

        Task<T> GetByIdAsync(int id);

        Task<IList<T>> GetAllAsync();

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
