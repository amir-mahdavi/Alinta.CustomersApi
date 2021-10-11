using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Alinta.CustomersApi.Data;
using Alinta.CustomersApi.Models;
using Alinta.CustomersApi.Entities;
using Alinta.CustomersApi.Service;
using Alinta.CustomersApi.Profiles;
using Xunit;

namespace Alinta.CustomersApi.Tests
{
    public class CustomerServiceTests
    {
        private readonly IMapper _mapper;
        private readonly CustomerDbContext _customerDbContext;
        private readonly CustomerRepository _customerRepository;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile<CustomerProfile>());
            _mapper = config.CreateMapper();

            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase("Customers")
                .Options;
            _customerDbContext = new CustomerDbContext(options);

            _customerRepository = new CustomerRepository(_customerDbContext);

            _customerService = new CustomerService(_customerRepository, _mapper);
        }

        private void ResetCustomerDb()
        {
            _customerDbContext.Database.EnsureDeleted();
            _customerDbContext.Database.EnsureCreated();
            _customerDbContext.Customers.AddRange(
                new Customer() { FirstName = "John", LastName = "Wayne", DateOfBirth = new DateTime(1999, 12, 11) },
                new Customer() { FirstName = "Jane", LastName = "West", DateOfBirth = new DateTime(1988, 5, 23) },
                new Customer() { FirstName = "Jill", LastName = "Williams", DateOfBirth = new DateTime(2002, 6, 15) }
            );
            _customerDbContext.SaveChanges();
        }

        [Fact]
        public async void CreateCustomer_ReturnsCreatedCustomer()
        {
            // Arrange
            ResetCustomerDb();
            var createModel = new CustomerCreateModel
            {
                FirstName = "Amanda",
                LastName = "Myers",
                DateOfBirth = new DateTime(2000, 9, 12)
            };

            // Act
            var created = await _customerService.CreateCustomer(createModel);

            // Assert
            Assert.Equal(createModel.FirstName, created.FirstName);
        }

        [Fact]
        public async void UpdateCustomer_ReturnsUpdatedCustomer()
        {
            // Arrange
            ResetCustomerDb();
            var updateModel = new CustomerUpdateModel
            {
                Id = 1,
                FirstName = "Amanda",
                LastName = "Myers",
                DateOfBirth = new DateTime(2000, 9, 12)
            };

            // Act
            var updated = await _customerService.UpdateCustomer(1, updateModel);

            // Assert
            Assert.Equal(updateModel.FirstName, updated.FirstName);
        }

        [Fact]
        public async void DeleteCustomer_DeletesCustomer()
        {
            // Arrange
            ResetCustomerDb();
            var deleteId = 1;

            // Act
            var beforeDelete = await _customerService.GetCustomerById(deleteId);
            var deleted = await _customerService.DeleteCustomer(deleteId);
            var afterDelete = await _customerService.GetCustomerById(deleteId);

            // Assert
            Assert.NotNull(beforeDelete);
            Assert.True(deleted);
            Assert.Null(afterDelete);
        }

        [Fact]
        public async void GetCustomerById_ReturnsCustomerIfExists()
        {
            // Arrange
            ResetCustomerDb();
            var findId = 1;

            // Act
            var foundCustomer = await _customerService.GetCustomerById(findId);

            // Assert
            Assert.Equal(findId, foundCustomer.Id);
        }

        [Fact]
        public async void SearchCustomersByName_ReturnsCustomerIfNamePartiallyMatches()
        {
            // Arrange
            ResetCustomerDb();
            var cs = new CustomerService(_customerRepository, _mapper);
            var key = "ne";

            // Act
            var foundCustomers = await cs.SearchCustomersByName(key);

            // Assert
            Assert.NotNull(foundCustomers);
            Assert.Equal(2, foundCustomers.Count());
        }
    }
}
