using Microsoft.EntityFrameworkCore;
using Alinta.CustomersApi.Entities;

namespace Alinta.CustomersApi.Data
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> opt) : base(opt)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
