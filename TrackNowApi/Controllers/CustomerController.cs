using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackNowApi.Data;
using TrackNowApi.Model;

namespace TrackNowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CustomerController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpPost(Name = "CreateCustomer")]
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {
            
            _db.Add(customer);
            _db.SaveChanges();

            return Ok(customer);
        }

        [HttpGet(Name = "CustomerList")]
        public IActionResult CustomerList()
        {
            List<Customer> Customer = new List<Customer>();
            Customer = _db.Customer.ToList();

            return Ok(Customer);
            
        }
        [HttpGet("{CustomerId:Int}")]
        public IActionResult CustomerList(int CustomerId)
        {
            Customer Customer = _db.Customer.FirstOrDefault(x=>x.CustomerId == CustomerId);

            return Ok(Customer);

        }
        [HttpPut("{CustomerId:Int}")]
        public IActionResult CustomerUpdate(int CustomerId, [FromBody] Customer customer)
        {

            if (customer == null || customer.CustomerId != CustomerId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(customer);
            _db.SaveChanges();
            
            return Ok(customer);

        }
    }
}
