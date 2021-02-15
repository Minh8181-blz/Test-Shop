using System.Threading.Tasks;

namespace Domain.Base.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        T Add(T order);

        void Update(T entity);

        Task<T> GetAsync(int id);
    }
}
