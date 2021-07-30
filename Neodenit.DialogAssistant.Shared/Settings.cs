﻿using Neodenit.DialogAssistant.Shared.Interfaces;

namespace Neodenit.DialogAssistant.Shared
{
    public class Settings : ISettings
    {
        public int MaxTokens { get; set; }

        public double DailyCreditLimit { get; set; }

        public int MessageLimit { get; set; }

        public int MessageNumber { get; set; }

        public string ApiKeys { get; set; }

        public string Engine { get; set; }

        public double DavinciPrice { get; set; }

        public double CuriePrice { get; set; }

        public double BabbagePrice { get; set; }

        public double AdaPrice { get; set; }

        public double TokensForPrice { get; set; }
    }
}