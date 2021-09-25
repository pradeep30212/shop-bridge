using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridge.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> Get();
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Delete(int Id);
    }
}
