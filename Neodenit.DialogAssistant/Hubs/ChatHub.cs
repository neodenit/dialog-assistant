using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace BlazorServerSignalRApp.Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IDialogService dialogService;

        public ChatHub(IDialogService dialogService)
        {
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        public async Task SendMessage(Message message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);

            await dialogService.AddMessageToDialogAsync(message);
        }
    }
}
