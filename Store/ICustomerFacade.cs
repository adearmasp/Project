using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store
{
    public interface ICustomerFacade
    {
        Task<IEnumerable<Customer>> GetCustomersAsync(Expression<Func<Customer, bool>> predicate);
        Task SaveCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Customer customer);            
    }
}