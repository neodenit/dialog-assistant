using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.DataAccess.Repositories
{
    public class DialogRepository : EFRepository<Dialog>, IDialogRepository
    {
        private readonly IUserRepository userRepository;

        public DialogRepository(DbContext dbContext, IUserRepository userRepository) : base(dbContext)
        {
            this.userRepository = userRepository;
        }

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

        public async Task<Dialog> GetByUserNamesOrCreateEmptyAsync(string userName1, string userName2)
        {
            var dbDialog = GetByUserNames(userName1, userName2);

            if (dbDialog is null)
            {
                var user1 = userRepository.GetByName(userName1);
                var user2 = userRepository.GetByName(userName2);

                var emptyDialog = new Dialog { User1 = user1, User2 = user2 };

                await CreateAsync(emptyDialog);

                await SaveAsync();

                return emptyDialog;
            }
            else
            {
                return dbDialog;
            }
        }
    }
}
