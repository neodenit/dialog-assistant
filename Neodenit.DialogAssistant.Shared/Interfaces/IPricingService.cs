namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IPricingService
    {
        double GetPrice(int tokenNum);

        double GetPrice(string text);
    }
}