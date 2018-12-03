using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Store.Data.EntityFramework
{
    public sealed class CustomersFacade : ICustomerFacade
    {
        private readonly StoreContext context;

        public CustomersFacade(StoreContext context)
        {
            this.context = context;
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(Expression<Func<Customer, bool>> predicate)
        {
            return await context.Customers.Where(predicate).ToListAsync();
        }

        public async Task SaveCustomerAsync(Customer customer)
        {
            if (customer.Id == 0)
            {
                context.Customers.Add(customer);
            }
            else
            {
                context.Customers.Update(customer);
            }

            await context.SaveChangesAsync();
        }

       
    }
}
