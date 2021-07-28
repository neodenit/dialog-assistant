using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByName(string name);
    }
}