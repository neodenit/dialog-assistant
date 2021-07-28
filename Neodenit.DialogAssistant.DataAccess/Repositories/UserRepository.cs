using System.Linq;
using Microsoft.EntityFrameworkCore;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.DataAccess.Repositories
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext) { }

        public User GetByName(string name)
        {
            var user = dbSet.SingleOrDefault(u => u.Name == name);
            return user;
        }
    }
}
