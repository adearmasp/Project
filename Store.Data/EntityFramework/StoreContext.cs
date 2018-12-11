using Microsoft.EntityFrameworkCore;

namespace Store.Data.EntityFramework
{
    public sealed class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)  : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Customer> Customers { get; set; }
       
    }
}
