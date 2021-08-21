using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IPredictionService
    {
        Task<Response> GetPredictionAsync(Message message, ResponseStatus prevStatus);
    }
}