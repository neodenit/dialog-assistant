using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;
using OpenAI_API;

namespace Neodenit.DialogAssistant.Services
{
    public class GPT3Service : IGPT3Service
    {
        private readonly ITokenLimitService limitCheckerService;
        private readonly ITextService textService;
        private readonly IDialogRepository dialogRepository;
        private readonly ISettings settings;

        public GPT3Service(ITokenLimitService limitCheckerService, ITextService textService, IDialogRepository dialogRepository, ISettings settings)
        {
            this.limitCheckerService = limitCheckerService ?? throw new ArgumentNullException(nameof(limitCheckerService));
            this.textService = textService ?? throw new ArgumentNullException(nameof(textService));
            this.dialogRepository = dialogRepository ?? throw new ArgumentNullException(nameof(dialogRepository));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<Response> GetPredictionAsync(Message message, ResponseStatus prevStatus)
        {
            bool isFullSentence = textService.IsFullSentence(message.Text);

            message.SendTime = DateTime.UtcNow;

            if (isFullSentence && prevStatus == ResponseStatus.NoSentences)
            {
                var dbDialog = dialogRepository.GetByUserNames(message.Sender.Name, message.Receiver.Name);

                var dialog = new Dialog
                {
                    Messages = dbDialog?.Messages?.Append(message).ToList() ?? new List<Message>(),
                    User1 = dbDialog?.User1 ?? message.Sender,
                    User2 = dbDialog?.User2 ?? message.Receiver
                };

                if (settings.MessageLimit > 0)
                {
                    dialog.Messages = dialog.Messages.TakeLast(settings.MessageLimit).ToList();
                }

                string dialogText = textService.GetDialogText(dialog);
                string dialogTextWithReceiver = $"{dialogText}{Constants.MessageSeparator}{nameof(Dialog.User2)}:";

                bool hasCredit = limitCheckerService.CheckLimit(dialogTextWithReceiver, message.Sender.Name);

                if (hasCredit)
                {
                    var api = new OpenAIAPI(settings.ApiKeys, new Engine(settings.Engine));
                    CompletionResult completionResult = await api.Completions.CreateCompletionAsync(dialogTextWithReceiver, settings.MaxTokens, stopSequences: Constants.StopSequences2);
                    var completion = completionResult.Completions.First();
                    var predictionText = completion.FinishReason == Constants.LengthFinishReason ? $"{completion.Text}{Constants.Ellipsis}" : completion.Text;

                    await limitCheckerService.UpdateLimitAsync(message.Sender.Name, dialogTextWithReceiver, predictionText);
                    double credit = limitCheckerService.GetLimit(message.Sender.Name);

                    return new Response { Status = ResponseStatus.Success, Text = predictionText, Credit = credit };
                }
                else
                {
                    return new Response { Status = ResponseStatus.NoCredit };
                }
            }
            else
            {
                return new Response { Status = isFullSentence ? ResponseStatus.Editing : ResponseStatus.NoSentences };
            }
        }
    }
}
