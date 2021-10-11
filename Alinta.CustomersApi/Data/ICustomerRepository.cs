using System.Threading.Tasks;
using System.Collections.Generic;
using Alinta.CustomersApi.Entities;

namespace Alinta.CustomersApi.Data
{
    public interface ICustomerRepository
    {
        bool SaveChanges();
        Task<bool> SaveChangesAsync();

        Task CreateCustomer(Customer customer);
        Task<Customer> UpdateCustomer(int id, Customer customer);
        void DeleteCustomer(Customer customer);
        Task<Customer> GetCustomerById(int id);
        Task<IEnumerable<Customer>> SearchCustomersByName(string key);
    }
}
