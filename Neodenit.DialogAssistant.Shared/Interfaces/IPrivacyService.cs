using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IPrivacyService
    {
        Dialog CleanUpDialog(Dialog dialog);
    }
}