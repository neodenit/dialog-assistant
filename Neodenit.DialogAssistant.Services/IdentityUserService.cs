using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Neodenit.DialogAssistant.Shared.Interfaces;

namespace Neodenit.DialogAssistant.Services
{
    public class IdentityUserService : IIdentityUserService
    {
        private readonly IRepository<IdentityUser> repository;

        public IdentityUserService(IRepository<IdentityUser> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<string> GetAll()
        {
            var allUsers = repository.GetAll();
            return allUsers.Select(x => x.UserName);
        }
    }
}
