using System.Linq;
using Microsoft.EntityFrameworkCore;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.DataAccess.Repositories
{
    public class DialogRepository : EFRepository<Dialog>, IDialogRepository
    {
        public DialogRepository(DbContext dbContext) : base(dbContext) { }

        public Dialog GetByUserNames(string user1, string user2)
        {
            var dialog = dbSet
                .Include(nameof(Dialog.User1))
                .Include(nameof(Dialog.User2))
                .Include(nameof(Dialog.Messages))
                .SingleOrDefault(d => d.User1.Name == user1 && d.User2.Name == user2 ||
                    d.User1.Name == user2 && d.User2.Name == user1);

            return dialog;
        }
    }
}
