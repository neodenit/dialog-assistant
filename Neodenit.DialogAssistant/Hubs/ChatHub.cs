using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace BlazorServerSignalRApp.Server.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IDialogService dialogService;

        public ChatHub(IDialogService dialogService)
        {
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        public async Task SendMessage(Message message)
        {
            await Clients.Groups(message.Receiver.Name).SendAsync("ReceiveMessage", message);

            await dialogService.AddMessageToDialogAsync(message);
        }

        public async override Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await base.OnConnectedAsync();
        }
    }
}
