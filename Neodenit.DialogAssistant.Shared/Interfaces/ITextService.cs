using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface ITextService
    {
        string GetDialogText(Dialog dialog);

        string GetDialogSamplesText(Dialog dialog);

        bool IsFullSentence(string text);
    }
}