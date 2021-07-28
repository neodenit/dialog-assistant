using System.Threading.Tasks;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetNameAsync();
    }
}