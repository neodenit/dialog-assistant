using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IGPT3Service
    {
        Task<Response> GetPredictionAsync(Message message);
    }
}