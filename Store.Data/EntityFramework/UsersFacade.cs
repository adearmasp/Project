using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Store.Data.EntityFramework
{
    public sealed class UsersFacade : IUsersFacade
    {
        private readonly StoreContext context;

        public UsersFacade(StoreContext context)
        {
            this.context = context;
        }

        public async Task AddUserAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>> predicate)
        {
            return await context.Users.Where(predicate).ToListAsync();
        }

        public async Task SaveUserAsync(User user)
        {
            if (user.Id == 0)
            {
                context.Add(user);
            }
            else
            {
                context.Update(user);
            }

            await context.SaveChangesAsync();
        }
    }
}
