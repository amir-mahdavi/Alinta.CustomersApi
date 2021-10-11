using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Alinta.CustomersApi.Data;
using Alinta.CustomersApi.Models;
using Alinta.CustomersApi.Entities;

namespace Alinta.CustomersApi.Service
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _customerRepository;
        private IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerReadModel> CreateCustomer(CustomerCreateModel customerCreateModel)
        {
            var customer = _mapper.Map<Customer>(customerCreateModel);
            await _customerRepository.CreateCustomer(customer);
            await _customerRepository.SaveChangesAsync();
            return _mapper.Map<CustomerReadModel>(customer);
        }

        public async Task<CustomerReadModel> UpdateCustomer(int id, CustomerUpdateModel customerUpdateModel)
        {
            var customer = _mapper.Map<Customer>(customerUpdateModel);
            await _customerRepository.UpdateCustomer(id, customer);
            await _customerRepository.SaveChangesAsync();
            return _mapper.Map<CustomerReadModel>(customer);
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomerById(id);
            _customerRepository.DeleteCustomer(customer);
            return await _customerRepository.SaveChangesAsync();
        }

        public async Task<CustomerReadModel> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetCustomerById(id);
            return _mapper.Map<CustomerReadModel>(customer);
        }

        public async Task<IEnumerable<CustomerReadModel>> SearchCustomersByName(string key)
        {
            var customers = await _customerRepository.SearchCustomersByName(key);
            return _mapper.Map<IEnumerable<CustomerReadModel>>(customers);
        }
    }
}
