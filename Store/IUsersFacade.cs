using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store
{
    public interface IUsersFacade
    {
        Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>> predicate);
        Task SaveUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}