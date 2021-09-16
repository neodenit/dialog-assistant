using System.Threading.Tasks;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface ITokenLimitService
    {
        Task<bool> CheckLimitAsync(string request, string userName);

        Task UpdateLimitAsync(string userName, string request, string response);

        Task<double> GetLimitAsync(string userName);
    }
}