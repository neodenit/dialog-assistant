using System.Linq;
using Neodenit.DialogAssistant.Shared;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Services
{
    public class TextService : ITextService
    {
        private readonly IPrivacyService privacyService;

        public TextService(IPrivacyService privacyService)
        {
            this.privacyService = privacyService ?? throw new System.ArgumentNullException(nameof(privacyService));
        }

        public string GetDialogText(Dialog dialog)
        {
            Dialog cleanDialog = privacyService.CleanUpDialog(dialog);
            var messages = cleanDialog.Messages.Select(m => $"{m.Sender.Name}: {m.Text}");
            var text = string.Join(Constants.MessageSeparator, messages);
            return text;
        }

        public bool IsFullSentence(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                return false;
            }
            else
            {
                var trimmedText = text.Trim();
                var lastChar = trimmedText.Last().ToString();

                var result = Constants.SentenceEndings.Contains(lastChar);
                return result;
            }
        }
    }
}
