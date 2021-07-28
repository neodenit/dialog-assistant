﻿using System.Collections.Generic;
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

        Task CreateAsync(T entity);

        void Create(IEnumerable<T> entities);

        Task CreateAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity, int id);

        void Delete(T entity);

        void Save();

        Task SaveAsync(CancellationToken token = default);
    }
}
