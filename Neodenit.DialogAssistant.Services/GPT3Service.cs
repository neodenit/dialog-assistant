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
            CompletionResult completionResult = await api.Completions.CreateCompletionAsync(prompt, settings.MaxTokens, settings.Temperature, settings.TopP, settings.NumOutputs, stopSequences: stopSequences);
            
            var completions = completionResult.Completions.Select(c =>
                c.FinishReason == Constants.LengthFinishReason
                    ? $"{c.Text}{Constants.Ellipsis}"
                    : c.Text);

            var completionText = string.Join(Constants.MessageSeparator, completions);
            var prediction = settings.PredictionStart + completionText;

            loggingService.LogPrediction(prompt, prediction);

            return prediction;
        }
    }
}
