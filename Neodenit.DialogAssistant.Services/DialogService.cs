using System;
using System.Linq;
using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Services
{
    public class DialogService : IDialogService
    {
        private readonly IUserRepository userRepository;
        private readonly IDialogRepository repository;

        public DialogService(IUserRepository userRepository, IDialogRepository repository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Dialog GetDialogForMessage(Message message)
        {
            var dialog = repository.GetByUserNames(message.Sender.Name, message.Receiver.Name);
            return dialog;
        }

        public async Task AddMessageToDialogAsync(Message message)
        {
            message.SendTime = DateTime.UtcNow;

            var dbUser1 = userRepository.GetByName(message.Sender.Name);
            var dbUser2 = userRepository.GetByName(message.Receiver.Name);

            var user1 = dbUser1 is null ? new User { Name = message.Sender.Name } : dbUser1;
            var user2 = dbUser2 is null ? new User { Name = message.Receiver.Name } : dbUser2;

            message.Sender = user1;
            message.Receiver = user2;

            Dialog dialog = GetDialogForMessage(message);

            if (dialog is null)
            {
                var newDialog = new Dialog
                {
                    User1 = user1,
                    User2 = user2,
                    Messages = new[] { message }
                };

                await repository.CreateAsync(newDialog);
            }
            else
            {
                dialog.Messages = dialog.Messages.Append(message).ToList();
            }

            await repository.SaveAsync();
        }
    }
}
