using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Alinta.CustomersApi.Models;
using Alinta.CustomersApi.Service;

namespace Alinta.CustomersApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomerService _customerService;

        public CustomersController(ILogger<CustomersController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerReadModel>> CreateCustomer([FromBody] CustomerCreateModel customerCreateModel)
        {
            if (string.IsNullOrWhiteSpace(customerCreateModel.FirstName) ||
                string.IsNullOrWhiteSpace(customerCreateModel.LastName) ||
                customerCreateModel.DateOfBirth.Year < 1900)
            {
                return BadRequest("Invalid first name, last name or date of birth");
            }

            var createdCustomer = await _customerService.CreateCustomer(customerCreateModel);
            return CreatedAtRoute(nameof(GetCustomerById), new { Id = createdCustomer.Id }, createdCustomer);
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<CustomerReadModel>> UpdateCustomer(int id, [FromBody] CustomerUpdateModel customerUpdateModel)
        {
            if (customerUpdateModel == null || customerUpdateModel.Id != id)
            {
                return BadRequest($"Invalid update model or Id does not match model");
            }

            var foundCustomer = await _customerService.GetCustomerById(id);
            if (foundCustomer == null)
            {
                return NotFound($"No customer found with id {id}");
            }

            var updatedCustomer = await _customerService.UpdateCustomer(id, customerUpdateModel);

            return Ok(updatedCustomer);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var foundCustomer = await _customerService.GetCustomerById(id);
            if (foundCustomer == null)
            {
                return NotFound($"No customer found with id {id}");
            }

            await _customerService.DeleteCustomer(id);
            return Ok();
        }

        [HttpGet("{id}", Name = "GetCustomerById")]
        public async Task<ActionResult<CustomerReadModel>> GetCustomerById(int id)
        {
            var foundCustomer = await _customerService.GetCustomerById(id);
            if (foundCustomer == null)
            {
                return NotFound($"No customer found with id {id}");
            }
            return Ok(foundCustomer);
        }

        [HttpGet]
        [Route("searchByName/{key}")]
        public async Task<ActionResult<IEnumerable<CustomerReadModel>>> SearchCustomersByName(string key)
        {
            var customers = await _customerService.SearchCustomersByName(key);
            
            if (!customers.Any())
            {
                return NotFound($"No customers found with name partially matching '{key}'");
            }

            return Ok(customers);
        }
    }
}
