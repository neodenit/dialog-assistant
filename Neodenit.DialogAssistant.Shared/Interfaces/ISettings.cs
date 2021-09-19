namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface ISettings
    {
        public string DomainName { get; set; }

        public int MaxTokens { get; set; }

        public int MessageLimit { get; set; }

        public double DailyCreditLimit { get; set; }

        public string Engine { get; set; }

        public double Temperature { get; set; }

        public double TopP { get; set; }

        public int NumOutputs { get; set; }

        public double DavinciPrice { get; set; }

        public double CuriePrice { get; set; }

        public double BabbagePrice { get; set; }

        public double AdaPrice { get; set; }

        public double TokensForPrice { get; set; }

        public string SenderPlaceholder { get; set; }

        public string ReceiverPlaceholder { get; set; }

        public string[] StopSequences { get; set; }

        public string PromptStart { get; set; }

        public string PromptEnd { get; set; }

        public string PredictionStart { get; set; }

        public string EmailAddress { get; set; }

        public string EmailSenderName { get; set; }

        public bool UseSamples { get; set; }

        public int SampleLength { get; set; }

        public int SampleNumber { get; set; }
    }
}