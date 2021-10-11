using AutoMapper;
using Alinta.CustomersApi.Entities;
using Alinta.CustomersApi.Models;

namespace Alinta.CustomersApi.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerReadModel>();
            CreateMap<CustomerCreateModel, Customer>();
            CreateMap<CustomerUpdateModel, Customer>();
        }
    }
}
