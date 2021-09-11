using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> repository;

        public UserService(IRepository<User> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await repository.GetAllAsync();

        public async Task<User> GetAsync(int id) => await repository.GetAsync(id);
    }
}
