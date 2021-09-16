using System;
using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Services
{
    public class DialogService : IDialogService
    {
        private readonly IDialogRepository repository;
        private readonly IRepository<Message> messageRepository;

        public DialogService(IDialogRepository repository, IRepository<Message> messageRepository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
        }

        public Dialog GetDialogForMessage(Message message)
        {
            var dialog = repository.GetByUserNames(message.Sender.Name, message.Receiver.Name);
            return dialog;
        }

        public async Task AddMessageToDialogAsync(Message message)
        {
            message.SendTime = DateTime.UtcNow;

            Dialog dialog = GetDialogForMessage(message);

            if (dialog is null)
            {
                var newDialog = new Dialog
                {
                    User1 = message.Sender,
                    User2 = message.Receiver,
                    Messages = new[] { message }
                };

                repository.Create(newDialog);

                await repository.SaveAsync();
            }
            else
            {
                message.DialogID = dialog.ID;

                messageRepository.Create(message);
                
                await messageRepository.SaveAsync();
            }
        }
    }
}
