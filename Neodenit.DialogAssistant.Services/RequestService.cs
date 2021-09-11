using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Services
{
    public class RequestService : IRequestService
    {
        private readonly ITextService textService;
        private readonly IDialogRepository dialogRepository;
        private readonly ISettings settings;

        public RequestService(ITextService textService, IDialogRepository dialogRepository, ISettings settings)
        {
            this.textService = textService ?? throw new ArgumentNullException(nameof(textService));
            this.dialogRepository = dialogRepository ?? throw new ArgumentNullException(nameof(dialogRepository));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<string> GetRequest(Message message)
        {
            Dialog dbDialog = await dialogRepository.GetByUserNamesOrCreateEmptyAsync(message.Sender.Name, message.Receiver.Name);

            var messages = dbDialog.Messages?.Append(message).ToList() ?? new List<Message> { message };

            var filteredMessages = settings.MessageLimit > 0 ? messages.TakeLast(settings.MessageLimit).ToList() : messages;

            var dialog = new Dialog
            {
                Messages = filteredMessages,
                User1 = dbDialog.User1,
                User2 = dbDialog.User2
            };

            string dialogText = textService.GetDialogText(dialog);
            return dialogText;
        }
    }
}
