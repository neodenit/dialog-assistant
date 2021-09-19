using System.Linq;
using Neodenit.DialogAssistant.Shared;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Services
{
    public class TextService : ITextService
    {
        private readonly ISettings settings;

        public TextService(ISettings settings)
        {
            this.settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
        }

        public string GetDialogText(Dialog dialog)
        {
            var messages = dialog.Messages.Select(m => $"{m.Sender.Name}: {m.Text}");
            var text = string.Join(Constants.MessageSeparator, messages);
            return text;
        }

        public string GetDialogSamplesText(Dialog dialog)
        {
            var recepientResponses = dialog.Messages.Where(m => m.Sender.Name == settings.ReceiverPlaceholder);
            var lastRecepientResponses = recepientResponses.TakeLast(settings.SampleNumber);

            var reversedMessages = dialog.Messages.Reverse();
            var samples = lastRecepientResponses.Select(x =>
                reversedMessages
                    .SkipWhile(m => m != x)
                    .Take(settings.SampleLength))
                    .Select(x => x.Reverse());

            var query = dialog.Messages.TakeLast(settings.SampleLength - 1);

            var samplesWithQueryMessages = samples.Append(query);
            var samplesWithQueryTexts = samplesWithQueryMessages.Select(x => x.Select(y => $"{y.Sender.Name}: {y.Text}"));

            var textSamples = samplesWithQueryTexts.Select(s => string.Join(Constants.MessageSeparator, s));
            var fullSeparator = Constants.MessageSeparator + Constants.SampleSeparator + Constants.MessageSeparator;
            var prompt = string.Join(fullSeparator, textSamples);
            return prompt;
        }

        public bool IsFullSentence(string text)
        {
            if (string.IsNullOrEmpty(text))
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
