namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface ILoggingService
    {
        void LogPrediction(string dialogTextWithReceiver, string prediction);
    }
}