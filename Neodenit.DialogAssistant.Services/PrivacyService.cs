using System.Collections.Generic;
using System.Linq;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Services
{
    public class PrivacyService : IPrivacyService
    {
        public Dialog CleanUpDialog(Dialog dialog)
        {
            var user1 = new User { Name = nameof(Dialog.User1) };
            var user2 = new User { Name = nameof(Dialog.User2) };

            var users = new Dictionary<string, User>
            {
                { dialog.User1.Name, user1 },
                { dialog.User2.Name, user2 }
            };

            var orderedMessages = dialog.Messages.OrderBy(m => m.SendTime);
            var cleanMessages = orderedMessages.Select(m => new Message
            {
                Text = m.Text,
                Sender = users[m.Sender.Name],
                Receiver = users[m.Receiver.Name]
            });

            var cleanDialog = new Dialog { Messages = cleanMessages.ToList(), User1 = user1, User2 = user2 };
            return cleanDialog;
        }
    }
}
