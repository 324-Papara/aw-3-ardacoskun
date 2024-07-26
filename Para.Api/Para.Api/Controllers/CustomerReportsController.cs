using Microsoft.AspNetCore.Mvc;
using Para.Data.DapperRepository;
using Para.Data.Domain;

namespace Para.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerReportsController : ControllerBase
    {
        private readonly CustomerRepository customerRepository;

        public CustomerReportsController(CustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        [HttpGet("{customerId}")]
        public async Task<ActionResult<Customer>> GetDetails([FromRoute] long customerId)
        {
            var customer = await customerRepository.GetCustomerDetails(customerId);

            if (customer is null)
            {
                return NotFound("Customer not found.");
            }

            return Ok(customer);
        }
    }
}