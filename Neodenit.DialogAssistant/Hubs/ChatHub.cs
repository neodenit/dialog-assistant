using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Neodenit.DialogAssistant.Shared.Models;

namespace BlazorServerSignalRApp.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
