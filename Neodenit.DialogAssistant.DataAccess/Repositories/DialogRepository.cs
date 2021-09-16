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
                .Include(x => x.User1)
                .Include(x => x.User2)
                .Include(x => x.Messages)
                .SingleOrDefault(d =>
                    d.User1.Name == user1 && d.User2.Name == user2 ||
                    d.User1.Name == user2 && d.User2.Name == user1);

            return dialog;
        }

        public async Task<Dialog> GetByUserNamesOrCreateEmptyAsync(string userName1, string userName2)
        {
            var dbDialog = GetByUserNames(userName1, userName2);

            if (dbDialog is null)
            {
                var user1 = await userRepository.GetByNameAsync(userName1);
                var user2 = await userRepository.GetByNameAsync(userName2);

                var emptyDialog = new Dialog { User1 = user1, User2 = user2 };

                Create(emptyDialog);

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
