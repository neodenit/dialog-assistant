using System;
using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly ITextService textService;
        private readonly IRequestService requestService;
        private readonly ITokenLimitService limitCheckerService;
        private readonly IGPT3Service gpt3Service;

        public PredictionService(ITextService textService, IRequestService requestService, ITokenLimitService limitCheckerService, IGPT3Service gpt3Service)
        {
            this.textService = textService ?? throw new ArgumentNullException(nameof(textService));
            this.requestService = requestService ?? throw new ArgumentNullException(nameof(requestService));
            this.limitCheckerService = limitCheckerService ?? throw new ArgumentNullException(nameof(limitCheckerService));
            this.gpt3Service = gpt3Service ?? throw new ArgumentNullException(nameof(gpt3Service));
        }

        public async Task<Response> GetPredictionAsync(Message message, ResponseStatus prevStatus)
        {
            message.SendTime = DateTime.UtcNow;

            bool isFullSentence = textService.IsFullSentence(message.Text);

            if (isFullSentence && prevStatus == ResponseStatus.NoSentences)
            {
                string request = await requestService.GetRequest(message);

                bool hasCredit = await limitCheckerService.CheckLimitAsync(request, message.Sender.Name);

                if (hasCredit)
                {
                    string predictionText = await gpt3Service.GetCompletion(request);

                    await limitCheckerService.UpdateLimitAsync(message.Sender.Name, request, predictionText);

                    double credit = await limitCheckerService.GetLimitAsync(message.Sender.Name);

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
