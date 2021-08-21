using System.Threading.Tasks;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IGPT3Service
    {
        Task<string> GetCompletion(string dialogTextWithReceiver);
    }
}