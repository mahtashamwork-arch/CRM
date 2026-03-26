using CRM.API.Data;
using CRM.API.DTO.Customer;
using CRM.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CustomersController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerReadDto>>> GetAllCustomers()
        {
            var customer = await _context.Customers.AsNoTracking().
                Select(c => new CustomerReadDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.Phone,
                    CompanyName = c.CompanyName,
                    Address = c.Address,
                    CreatedAt = c.CreatedDate
                }).ToListAsync();

            return Ok(customer);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerReadDto>> GetById(int id)
        {
            var customer = await _context.Customers.
                AsNoTracking().
                Where(c => c.Id == id).
                Select(c => new CustomerReadDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.Phone,
                    Address = c.Address,
                    CompanyName = c.CompanyName,
                    CreatedAt = c.CreatedDate
                }).FirstOrDefaultAsync();

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerReadDto>> CreatCustomer(CreateCustomerDto createCustomerDto)
        {
            var customeremail = await _context.Customers.AnyAsync(c=>c.Email== createCustomerDto.Email);
            if (customeremail)
                return Conflict(new { message =$"A Customer with email {createCustomerDto.Email} Already Exits"});
            var customer = new Customer
            {
                FirstName = createCustomerDto.FirstName,
                LastName = createCustomerDto.LastName,
                Email = createCustomerDto.Email,
                Phone = createCustomerDto.Phone,
                CompanyName = createCustomerDto.CompanyName,
                Address = createCustomerDto.Address
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var customerres = MapToReadDto(customer);

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customerres);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerDto updateCustomerDto)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer is null)
                return NotFound(new { message=$"Customer with id {id} was not found"});

            customer.FirstName = updateCustomerDto.FirstName;
            customer.LastName = updateCustomerDto.LastName;
            customer.Email = updateCustomerDto.Email;
            customer.Phone = updateCustomerDto.Phone;
            customer.CompanyName = updateCustomerDto.CompanyName;
            customer.Address = updateCustomerDto.Address;

           await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer is null)
                return NotFound(new {message=$"Customer with {id} not found"});
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static CustomerReadDto MapToReadDto(Customer c)
        {
            return new CustomerReadDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address,
                CompanyName = c.CompanyName,
                CreatedAt = c.CreatedDate
            };

        }
    }
}
