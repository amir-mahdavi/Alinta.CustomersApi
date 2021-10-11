using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Alinta.CustomersApi.Entities;

namespace Alinta.CustomersApi.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task CreateCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }
            await _context.Customers.AddAsync(customer);
        }

        public async Task<Customer> UpdateCustomer(int id, Customer customer)
        {
            var foundCustomer = await _context.Customers.FindAsync(id);
            foundCustomer.FirstName = customer.FirstName;
            foundCustomer.LastName = customer.LastName;
            foundCustomer.DateOfBirth = customer.DateOfBirth;
            return foundCustomer;
        }

        public void DeleteCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<IEnumerable<Customer>> SearchCustomersByName(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Enumerable.Empty<Customer>();
            }

            return await _context.Customers
                .Where(c => c.FirstName.Contains(key, StringComparison.OrdinalIgnoreCase) || c.LastName.Contains(key, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public bool SaveChanges()
        {
            var saved = _context.SaveChanges();
            return saved >= 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved >= 0;
        }
    }
}
