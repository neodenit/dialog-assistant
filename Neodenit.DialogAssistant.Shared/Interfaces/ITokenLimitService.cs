using System.Threading.Tasks;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface ITokenLimitService
    {
        bool CheckLimit(string request, string userName);

        Task UpdateLimitAsync(string userName, string request, string response);

        double GetLimit(string userName);
    }
}