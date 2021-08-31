using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Neodenit.DialogAssistant.Shared.Interfaces;

namespace Neodenit.DialogAssistant.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public AuthService(AuthenticationStateProvider authenticationStateProvider)
        {
            this.authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
        }

        public async Task<string> GetNameAsync()
        {
            AuthenticationState authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var senderName = authState.User.Identity.Name;
            return senderName;
        }
    }
}
