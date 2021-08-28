﻿namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface ISettings
    {
        public string DomainName { get; set; }

        public int MaxTokens { get; set; }

        public int MessageLimit { get; set; }

        public double DailyCreditLimit { get; set; }

        string Engine { get; set; }

        public double Temperature { get; set; }

        public double DavinciPrice { get; set; }

        public double CuriePrice { get; set; }

        public double BabbagePrice { get; set; }

        public double AdaPrice { get; set; }

        double TokensForPrice { get; set; }

        public string SenderPlaceholder { get; set; }

        public string ReceiverPlaceholder { get; set; }
    }
}