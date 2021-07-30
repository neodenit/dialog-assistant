using Neodenit.DialogAssistant.Shared.Models;
using System.Threading.Tasks;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IDialogRepository : IRepository<Dialog>
    {
        Dialog GetByUserNames(string user1, string user2);

        Task<Dialog> GetByUserNamesOrCreateEmptyAsync(string userName1, string userName2);
    }
}