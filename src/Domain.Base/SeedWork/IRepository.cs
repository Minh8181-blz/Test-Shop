using System.Threading.Tasks;

namespace Domain.Base.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork<T> UnitOfWork { get; }
        T Add(T order);

        void Update(T order);

        Task<T> GetAsync(int orderId);
    }
}
