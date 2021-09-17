using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Neodenit.DialogAssistant.Shared;
using Neodenit.DialogAssistant.Shared.Interfaces;

namespace Neodenit.DialogAssistant.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<GPT3Service> logger;
        private readonly ISettings settings;

        public LoggingService(ILogger<GPT3Service> logger, ISettings settings)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void LogPrediction(string request, string prediction)
        {
            logger.LogInformation("Engine: {0}", settings.Engine);
            logger.LogInformation("Temperature: {0}", settings.Temperature);
            logger.LogInformation("StopSequences: {0}", JsonSerializer.Serialize(settings.StopSequences));
            logger.LogInformation("MaxTokens: {0}", JsonSerializer.Serialize(settings.MaxTokens));
            logger.LogInformation("Prompt:{0}{1}", Constants.MessageSeparator, request);
            logger.LogInformation("Prediction:{0}{1}", Constants.MessageSeparator, prediction);
        }
    }
}
