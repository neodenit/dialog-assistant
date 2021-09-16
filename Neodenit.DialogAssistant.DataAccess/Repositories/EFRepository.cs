using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Neodenit.DialogAssistant.DataAccess.Repositories
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<T> dbSet;

        public EFRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public IEnumerable<T> GetAll() => dbSet.ToList();

        public async Task<IEnumerable<T>> GetAllAsync() => await dbSet.ToListAsync();

        public T Get(int id) => dbSet.Find(id);

        public async Task<T> GetAsync(int id) => await dbSet.FindAsync(id);

        public void Create(T entity)
        {
            dbContext.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Added;
        }

        public void Update(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
