using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IDialogService
    {
        Dialog GetDialogForMessage(Message message);

        Task AddMessageToDialogAsync(Message message);
    }
}