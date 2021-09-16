using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        T Get(int id);

        Task<T> GetAsync(int id);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task SaveAsync();
    }
}
