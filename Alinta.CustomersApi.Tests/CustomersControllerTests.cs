using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Alinta.CustomersApi.Models;
using Alinta.CustomersApi.Service;
using Alinta.CustomersApi.Controllers;
using Xunit;
using Moq;

namespace Alinta.CustomersApi.Tests
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerService> _customerService;

        public CustomersControllerTests()
        {
            _customerService = new Mock<ICustomerService>();
        }

        [Fact]
        public async Task CreateCustomer_WhenInvalidModel_ShouldReturnBadRequest()
        {
            // Arrange
            //_customerService.Setup(s => s.CreateCustomer(It.IsAny<CustomerCreateModel>())).Returns(Task.FromResult<CustomerReadModel>(null));
            var customersController = new CustomersController(null, _customerService.Object);

            // Act
            var result = await customersController.CreateCustomer(new CustomerCreateModel());

            // Assert
            Assert.True(result.Result is BadRequestObjectResult);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.Equal("Invalid first name, last name or date of birth", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task CreateCustomer_WhenValidModel_ShouldReturnOk()
        {
            // Arrange
            var newCustomer = new CustomerCreateModel
            {
                FirstName = "Jonathan",
                LastName = "Stewart",
                DateOfBirth = new DateTime(1976, 6, 14)
            };
            var createdCustomer = new CustomerReadModel
            {
                Id = 1,
                FirstName = newCustomer.FirstName,
                LastName = newCustomer.LastName,
                DateOfBirth = newCustomer.DateOfBirth
            };
            _customerService.Setup(s => s.CreateCustomer(It.IsAny<CustomerCreateModel>())).Returns(Task.FromResult<CustomerReadModel>(createdCustomer));
            var customersController = new CustomersController(null, _customerService.Object);

            // Act
            var result = await customersController.CreateCustomer(newCustomer);

            // Assert
            Assert.True(result.Result is CreatedAtRouteResult);
            var createdAtResult = result.Result as CreatedAtRouteResult;
            Assert.Equal("GetCustomerById", createdAtResult.RouteName);
            Assert.Equal(createdCustomer.Id, ((CustomerReadModel)createdAtResult.Value).Id);
        }

        [Fact]
        public async Task UpdateCustomer_WhenInvalidModel_ShouldReturnBadRequest()
        {
            // Arrange
            var updateId = 1;
            var updatedCustomer = new CustomerUpdateModel
            {
                Id = updateId + 1,
                FirstName = "George",
                LastName = "Michael",
                DateOfBirth = new DateTime(1986, 1, 19)
            };
            var customersController = new CustomersController(null, _customerService.Object);

            // Act
            var result = await customersController.UpdateCustomer(updateId, updatedCustomer);

            // Assert
            Assert.True(result.Result is BadRequestObjectResult);
        }

        [Fact]
        public async Task UpdateCustomer_WhenValidModel_ShouldReturnOk()
        {
            // Arrange
            var updateId = 1;
            var updatedCustomer = new CustomerUpdateModel
            {
                Id = updateId,
                FirstName = "George",
                LastName = "Michael",
                DateOfBirth = new DateTime(1986, 1, 19)
            };
            _customerService.Setup(s => s.GetCustomerById(It.IsAny<int>())).Returns(Task.FromResult(new CustomerReadModel()));
            _customerService.Setup(s => s.UpdateCustomer(It.IsAny<int>(), It.IsAny<CustomerUpdateModel>())).Returns(Task.FromResult(new CustomerReadModel()));
            var customersController = new CustomersController(null, _customerService.Object);

            // Act
            var result = await customersController.UpdateCustomer(updateId, updatedCustomer);

            // Assert
            Assert.True(result.Result is OkObjectResult);
            var okResult = result.Result as OkObjectResult;
            Assert.True(okResult.Value is CustomerReadModel);
        }

        [Fact]
        public async Task DeleteCustomer_WhenNonExistingCustomer_ShouldReturnNotFound()
        {
            // Arrange
            var deleteId = 1;
            _customerService.Setup(s => s.GetCustomerById(It.IsAny<int>())).Returns(Task.FromResult<CustomerReadModel>(null));
            var customersController = new CustomersController(null, _customerService.Object);

            // Act
            var result = await customersController.DeleteCustomer(deleteId);

            // Assert
            Assert.True(result is NotFoundObjectResult);
        }

        [Fact]
        public async Task DeleteCustomer_WhenExistingCustomer_ShouldDeleteCustomer()
        {
            // Arrange
            var deleteId = 1;
            _customerService.Setup(s => s.GetCustomerById(It.IsAny<int>())).Returns(Task.FromResult(new CustomerReadModel()));
            _customerService.Setup(s => s.DeleteCustomer(It.IsAny<int>())).Returns(Task.FromResult(true));
            var customersController = new CustomersController(null, _customerService.Object);

            // Act
            var result = await customersController.DeleteCustomer(deleteId);

            // Assert
            Assert.True(result is OkResult);
        }

        [Fact]
        public async Task GetCustomerById_WhenNonExistingCustomer_ShouldReturnNotFound()
        {
            // Arrange
            var findId = 1;
            _customerService.Setup(s => s.GetCustomerById(It.IsAny<int>())).Returns(Task.FromResult<CustomerReadModel>(null));
            var customersController = new CustomersController(null, _customerService.Object);

            // Act
            var result = await customersController.GetCustomerById(findId);

            // Assert
            Assert.True(result.Result is NotFoundObjectResult);
        }

        [Fact]
        public async Task GetCustomerById_WhenExistingCustomer_ShouldReturnNotFound()
        {
            // Arrange
            var findId = 1;
            _customerService.Setup(s => s.GetCustomerById(It.IsAny<int>())).Returns(Task.FromResult(new CustomerReadModel()));
            var customersController = new CustomersController(null, _customerService.Object);

            // Act
            var result = await customersController.GetCustomerById(findId);

            // Assert
            Assert.True(result.Result is OkObjectResult);
        }

        [Fact]
        public async Task SearchCustomersByName_WhenNoCustomersMatch_ShouldReturnNotFound()
        {
            // Arrange
            var key = "jane";
            _customerService.Setup(s => s.SearchCustomersByName(It.IsAny<string>())).Returns(Task.FromResult(Enumerable.Empty<CustomerReadModel>()));
            var customersController = new CustomersController(null, _customerService.Object);

            // Act
            var result = await customersController.SearchCustomersByName(key);

            // Assert
            Assert.True(result.Result is NotFoundObjectResult);
        }

        [Fact]
        public async Task SearchCustomersByName_WhenAnyCustomersMatch_ShouldReturnOk()
        {
            // Arrange
            var key = "jane";
            _customerService.Setup(s => s.SearchCustomersByName(It.IsAny<string>())).Returns(Task.FromResult(Enumerable.Repeat(new CustomerReadModel(), 5)));
            var customersController = new CustomersController(null, _customerService.Object);

            // Act
            var result = await customersController.SearchCustomersByName(key);

            // Assert
            Assert.True(result.Result is OkObjectResult);
            var okResult = result.Result as OkObjectResult;
            var okValue = okResult.Value as IEnumerable<CustomerReadModel>;
            Assert.Equal(5, okValue.Count());
        }
    }
}
