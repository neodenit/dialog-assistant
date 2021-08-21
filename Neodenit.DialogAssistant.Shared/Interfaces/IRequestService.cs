using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IRequestService
    {
        Task<string> GetRequest(Message message);
    }
}