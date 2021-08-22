using System;
using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Interfaces;

namespace Neodenit.DialogAssistant.Services
{
    public class TokenLimitService : ITokenLimitService
    {
        private readonly IPricingService pricingService;
        private readonly IUserRepository userRepository;
        private readonly ISettings settings;

        public TokenLimitService(IPricingService pricingService, IUserRepository userRepository, ISettings settings)
        {
            this.pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public bool CheckLimit(string request, string userName)
        {
            var user = userRepository.GetByName(userName);

            var requestPrice = pricingService.GetPrice(request);
            var maxResponcePrice = pricingService.GetPrice(settings.MaxTokens);
            var totalPrice = requestPrice + maxResponcePrice;
            var creditUsed = user.LastRequestDate == DateTime.UtcNow.Date ? user.CreditUsed : 0;

            var hasCredit = creditUsed + totalPrice < settings.DailyCreditLimit;
            return hasCredit;
        }

        public async Task UpdateLimitAsync(string userName, string request, string response)
        {
            var user = userRepository.GetByName(userName);

            var requestPrice = pricingService.GetPrice(request);
            var responsePrice = pricingService.GetPrice(response);
            var totalPrice = requestPrice + responsePrice;
            var oldCredit = user.LastRequestDate == DateTime.UtcNow.Date ? user.CreditUsed : 0;

            user.CreditUsed = oldCredit + totalPrice;
            user.LastRequestDate = DateTime.UtcNow.Date;

            await userRepository.SaveAsync();
        }

        public double GetLimit(string userName)
        {
            var user = userRepository.GetByName(userName);

            var maxResponcePrice = pricingService.GetPrice(settings.MaxTokens);
            var estimatedCredit = user.CreditUsed + maxResponcePrice;
            var result = estimatedCredit < settings.DailyCreditLimit ? user.CreditUsed / settings.DailyCreditLimit : 0;
            return result;
        }
    }
}
