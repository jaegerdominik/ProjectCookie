using DAL.Entities;

namespace DAL.Repository
{
    public interface IHypertableRepository<T> where T : HyperEntity
    {
        Task CreateAsync(T entity);
        Task CreateAsync(List<T> entity);
        Task<T> GetLastValue(int dp);
        Task<List<T>> GetValues(int dp, DateTimeOffset from, DateTimeOffset to);

    }
}
