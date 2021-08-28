using System;
using System.Linq;
using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared;
using Neodenit.DialogAssistant.Shared.Interfaces;
using OpenAI_API;

namespace Neodenit.DialogAssistant.Services
{
    public class GPT3Service : IGPT3Service
    {
        private readonly ILoggingService loggingService;
        private readonly ISettings settings;

        public GPT3Service(ILoggingService loggingService, ISettings settings)
        {
            this.loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<string> GetCompletion(string request)
        {
            var stopSequences = new[] { $"{settings.SenderPlaceholder}:", $"{settings.ReceiverPlaceholder}:" };

            var api = new OpenAIAPI(APIAuthentication.LoadFromEnv(), new Engine(settings.Engine));
            CompletionResult completionResult = await api.Completions.CreateCompletionAsync(request, settings.MaxTokens, settings.Temperature, stopSequences: stopSequences);
            var completion = completionResult.Completions.First();
            var completionText = completion.Text;

            loggingService.LogPrediction(request, completionText);

            var predictionText = completion.FinishReason == Constants.LengthFinishReason ? $"{completionText}{Constants.Ellipsis}" : completionText;
            return predictionText;
        }
    }
}
