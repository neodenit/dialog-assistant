using System.Collections.Generic;
using System.Linq;
using Neodenit.DialogAssistant.Shared;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Services
{
    public class PrivacyService : IPrivacyService
    {
        public Dialog CleanUpDialog(Dialog dialog)
        {
            var orderedMessages = dialog.Messages.OrderBy(m => m.SendTime);
            var lastMessage = orderedMessages.Last();

            var user1 = new User { Name = lastMessage.Sender.Name == dialog.User1.Name ? Constants.SenderPlaceholder : Constants.ReceiverPlaceholder };
            var user2 = new User { Name = lastMessage.Sender.Name == dialog.User2.Name ? Constants.SenderPlaceholder : Constants.ReceiverPlaceholder };

            var users = new Dictionary<string, User>
            {
                { dialog.User1.Name, user1 },
                { dialog.User2.Name, user2 }
            };

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
