using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetAsync(int id);

        User GetByName(string name);
    }
}