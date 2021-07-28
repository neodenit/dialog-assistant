using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IDialogRepository : IRepository<Dialog>
    {
        Dialog GetByUserNames(string user1, string user2);
    }
}