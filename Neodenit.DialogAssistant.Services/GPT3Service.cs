using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GPT3Service> logger;
        private readonly ISettings settings;

        public GPT3Service(ITokenLimitService limitCheckerService, ITextService textService, IDialogRepository dialogRepository, ILogger<GPT3Service> logger, ISettings settings)
        {
            this.limitCheckerService = limitCheckerService ?? throw new ArgumentNullException(nameof(limitCheckerService));
            this.textService = textService ?? throw new ArgumentNullException(nameof(textService));
            this.dialogRepository = dialogRepository ?? throw new ArgumentNullException(nameof(dialogRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<Response> GetPredictionAsync(Message message, ResponseStatus prevStatus)
        {
            bool isFullSentence = textService.IsFullSentence(message.Text);

            message.SendTime = DateTime.UtcNow;

            if (isFullSentence && prevStatus == ResponseStatus.NoSentences)
            {
                var dbDialog = await dialogRepository.GetByUserNamesOrCreateEmptyAsync(message.Sender.Name, message.Receiver.Name);

                var messages = dbDialog.Messages?.Append(message).ToList() ?? new List<Message> { message };

                var filteredMessages = settings.MessageLimit > 0 ? messages.TakeLast(settings.MessageLimit).ToList() : messages;

                var dialog = new Dialog
                {
                    Messages = filteredMessages,
                    User1 = dbDialog.User1,
                    User2 = dbDialog.User2
                };

                string dialogText = textService.GetDialogText(dialog);

                string receiverPlaceholder = dialog.User1.Name == message.Receiver.Name ? nameof(Dialog.User1) : nameof(Dialog.User2);

                string dialogTextWithReceiver = $"{dialogText}{Constants.MessageSeparator}{receiverPlaceholder}:";

                bool hasCredit = limitCheckerService.CheckLimit(dialogTextWithReceiver, message.Sender.Name);

                if (hasCredit)
                {
                    var api = new OpenAIAPI(settings.ApiKeys, new Engine(settings.Engine));
                    CompletionResult completionResult = await api.Completions.CreateCompletionAsync(dialogTextWithReceiver, settings.MaxTokens, settings.Temperature, stopSequences: Constants.StopSequences2);
                    var completion = completionResult.Completions.First();
                    var predictionText = completion.FinishReason == Constants.LengthFinishReason ? $"{completion.Text}{Constants.Ellipsis}" : completion.Text;

                    logger.LogInformation("Engine: {0}", settings.Engine);
                    logger.LogInformation("Temperature: {0}", settings.Temperature);
                    logger.LogInformation("Prompt:{0}{1}", Constants.MessageSeparator, dialogTextWithReceiver);
                    logger.LogInformation("Completion:{1}", completion.Text);

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
