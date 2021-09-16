using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neodenit.DialogAssistant.Shared.Interfaces;
using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;

        public UserService(IUserRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await repository.GetAllAsync();

        public async Task<User> GetAsync(int id) => await repository.GetAsync(id);

        public async Task<User> GetByNameAsync(string name) => await repository.GetByNameAsync(name);
    }
}
