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
            static string FixNewLines(string s) => s.Replace("\\n", "\n");

            var promptStart = FixNewLines(settings.PromptStart);
            var promptEnd = FixNewLines(settings.PromptEnd);
            var stopSequences = settings.StopSequences.Select(s => FixNewLines(s)).ToArray();

            var prompt = promptStart + request + promptEnd;

            var api = new OpenAIAPI(APIAuthentication.LoadFromEnv(), new Engine(settings.Engine));
            CompletionResult completionResult = await api.Completions.CreateCompletionAsync(prompt, settings.MaxTokens, settings.Temperature, stopSequences: stopSequences);
            var completion = completionResult.Completions.First();
            var completionText = completion.Text;

            loggingService.LogPrediction(prompt, completionText);

            var predictionText = completion.FinishReason == Constants.LengthFinishReason ? $"{completionText}{Constants.Ellipsis}" : completionText;
            return predictionText;
        }
    }
}
