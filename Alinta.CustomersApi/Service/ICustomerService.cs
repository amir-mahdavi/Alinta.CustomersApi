using System.Threading.Tasks;
using System.Collections.Generic;
using Alinta.CustomersApi.Models;

namespace Alinta.CustomersApi.Service
{
    public interface ICustomerService
    {
        Task<CustomerReadModel> CreateCustomer(CustomerCreateModel customer);
        Task<CustomerReadModel> UpdateCustomer(int id, CustomerUpdateModel customer);
        Task<bool> DeleteCustomer(int id);
        Task<CustomerReadModel> GetCustomerById(int id);
        Task<IEnumerable<CustomerReadModel>> SearchCustomersByName(string key);
    }
}
