using System.Collections.Generic;

namespace Neodenit.DialogAssistant.Shared.Interfaces
{
    public interface IIdentityUserService
    {
        IEnumerable<string> GetAll();
    }
}