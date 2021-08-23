using System;
using System.Collections.Generic;
using Neodenit.DialogAssistant.Shared;
using Neodenit.DialogAssistant.Shared.Interfaces;
using OpenAI_API;

namespace Neodenit.DialogAssistant.Services
{
    public class PricingService : IPricingService
    {
        private readonly ISettings settings;
        private readonly Dictionary<string, double> EnginePrice;

        public PricingService(ISettings settings)
        {
            EnginePrice = new Dictionary<string, double>
            {
                { Engine.Davinci.EngineName, settings.DavinciPrice },
                { Engine.Curie, settings.CuriePrice },
                { Engine.Babbage, settings.BabbagePrice },
                { Engine.Ada, settings.AdaPrice }
            };

            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public double GetPrice(string text)
        {
            var estimatedTokenNum = text.Length / Constants.ApproximateTokenLength;
            var tokenPrice = EnginePrice[settings.Engine];
            var totalPrice = tokenPrice * estimatedTokenNum / settings.TokensForPrice;
            return totalPrice;
        }

        public double GetPrice(int tokenNum)
        {
            var tokenPrice = EnginePrice[settings.Engine];
            var totalPrice = tokenPrice * tokenNum / settings.TokensForPrice;
            return totalPrice;
        }
    }
}
