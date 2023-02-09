using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using System.Text.RegularExpressions;
using TrackNowApi.Data;
using TrackNowApi.Model;

namespace TrackNowApi.Controllers
{
    //public static class MyStringExtensions
    //{
    //    public static bool Like(this string toSearch, string toFind)
    //    {
    //        if (toFind != null)
    //        {
    //            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
    //        }
    //        else
    //            return true;
                
    //    }   
                
    //}
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CustomerController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpPost("CreateCustomer")]
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {

            _db.Add(customer);
            _db.SaveChanges();

            return Ok(customer);
        }

        [HttpGet("CustomerList")]
        public IActionResult CustomerList()
        {
            return Ok((from o in _db.Customer
                       join i in _db.BusinessCategoryMaster
                       on o.BusinessCatergoryId equals i.BusinessCatergoryId
                       select new
                       {
                           CustomerID = o.CustomerId,
                           CustomerName = o.CustomerName,
                           BusinessCategory = i.BusinessCategoryName,
                           Address = o.Address,
                           TaxNumber = o.TaxNumber,
                           Phone = o.Phone,
                           Mail = o.Mail,
                           LocationCode = o.LocationCode,
                           Juristiction = o.Juristiction,

                       }));

        }

        [HttpGet("CustomerList{CustomerId:Int}")]
        public IActionResult CustomerList(int CustomerId)
        {
            return Ok((from o in _db.Customer
                       join i in _db.BusinessCategoryMaster
                       on o.BusinessCatergoryId equals i.BusinessCatergoryId
                       where o.CustomerId == CustomerId
                       select new
                       {
                           CustomerID = o.CustomerId,
                           CustomerName = o.CustomerName,
                           BusinessCategory = i.BusinessCategoryName,
                           Address = o.Address,
                           TaxNumber = o.TaxNumber,
                           Phone = o.Phone,
                           Mail = o.Mail,
                           LocationCode = o.LocationCode,
                           Juristiction = o.Juristiction,

                       }));

        }

        [HttpGet("CustomerSearch")]
        public IActionResult CustomerSearch(string? BusinessCategory, string? Juristiction, string? State, string? City)
        {
            return Ok((from o in _db.Customer
                       join i in _db.BusinessCategoryMaster
                       on o.BusinessCatergoryId equals i.BusinessCatergoryId
                       where o.Juristiction != null && i.BusinessCategoryName != null
                                && o.State != null && o.City != null
                                && i.BusinessCategoryName.Contains(BusinessCategory)
                                && o.Juristiction.Contains(Juristiction)
                                && o.State.Contains(State)
                                && o.City.Contains(City)

                       select new
                       {
                           CustomerID = o.CustomerId,
                           CustomerName = o.CustomerName,
                           BusinessCategory = i.BusinessCategoryName,
                           Address = o.Address,
                           TaxNumber = o.TaxNumber,
                           Phone = o.Phone,
                           Mail = o.Mail,
                           LocationCode = o.LocationCode,
                           Juristiction = o.Juristiction,

                       })); ;

        }
        [HttpGet("BusinessCategoryList")]
        public IActionResult BusinessCategoryList()
        {
            return Ok((from i in _db.BusinessCategoryMaster
                       select new
                       {
                           BusinessCatergoryId = i.BusinessCatergoryId,
                           BusinessCategory = i.BusinessCategoryName,
                           BusinessCategoryName = i.BusinessCategoryName

                       })); ;

        }
        [HttpGet("StateList")]
        public IActionResult StateList()
        {
            List<string> StateList = new List<string>(){ "Alabama", "Alaska", "American Samoa", "Arizona", "Arkansas", "California",
                "Colorado", "Connecticut", "Delaware", "District of Columbia", "Federated States of Micronesia", "Florida", "Georgia",
                "Guam", "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana", "Maine", "Marshall Islands",
                "Maryland", "Massachusetts", "Michigan", "Minnesota", "Mississippi", "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire",
                "New Jersey", "New Mexico", "New York", "North Carolina", "North Dakota", "Northern Mariana Islands", "Ohio", "Oklahoma", "Oregon",
                "Palau", "Pennsylvania", "Puerto Rico", "Rhode Island", "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah", "Vermont",
                "Virgin Island", "Virginia", "Washington", "West Virginia", "Wisconsin", "Wyoming" };
            return Ok(StateList);

        }

        [HttpGet("CustomerfulltextSearch")]
        public IActionResult CustomerfulltextSearch(string? Textsearch)
        {
            return Ok((from o in _db.Customer
                       join i in _db.BusinessCategoryMaster
                       on o.BusinessCatergoryId equals i.BusinessCatergoryId
                       where o.Juristiction != null && o.CustomerName != null
                                && o.Address != null && o.LocationCode != null
                                && o.Mail != null && o.State != null && o.City != null
                                &&
                                (
                                o.Juristiction.Contains(Textsearch) || o.CustomerName.Contains(Textsearch) ||
                                o.Address.Contains(Textsearch) || o.LocationCode.ToString().Contains(Textsearch) ||
                                o.Mail.Contains(Textsearch) || o.State.Contains(Textsearch) || o.City.Contains(Textsearch)
                                )
                       select new
                       {
                           CustomerID = o.CustomerId,
                           CustomerName = o.CustomerName,
                           BusinessCategory = i.BusinessCategoryName,
                           Address = o.Address,
                           TaxNumber = o.TaxNumber,
                           Phone = o.Phone,
                           Mail = o.Mail,
                           LocationCode = o.LocationCode,
                           Juristiction = o.Juristiction,

                       })); ;

        }
        [HttpDelete("CustomerDelete{CustomerId:Int}")]
        public void CustomerDelete(int CustomerId)
        {
            Customer Customer;

            Customer = _db.Customer.Where(d => d.CustomerId == CustomerId).First();
            _db.Customer.Remove(Customer);
            _db.SaveChanges();

        }

        [HttpPut("CustomerUpdate{CustomerId:Int}")]
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
        [HttpPost("CreateCustomerHistory")]
        public IActionResult CreateCustomerHistory([FromBody] CustomerHistory CustomerHistory)
        {

            _db.Add(CustomerHistory);
            _db.SaveChanges();

            return Ok(CustomerHistory);
        }

        [HttpPost("CreateCustomerFilingMaster")]
        public IActionResult CreateCustomerFilingMaster([FromBody] CustomerFilingMaster CustomerFilingMaster)
        {

            _db.Add(CustomerFilingMaster);
            _db.SaveChanges();

            return Ok(CustomerFilingMaster);
        }

        [HttpGet("CustomerFilingList")]
        public IActionResult CustomerFilingList(int CustomerId)
        {
            return Ok(from i in _db.CustomerFilingMaster
                      join b in _db.FilingMaster on i.FilingId equals b.FilingId
                      where i.CustomerId == CustomerId

                      select new
                      {
                          FilingID = i.FilingId,
                          FilingDescription = b.FilingDescription,
                          FilingFrequency = b.FilingFrequency,
                          StateInfo = b.StateInfo,
                          RuleInfo = b.RuleInfo,
                          Juristiction = b.Juristiction,
                          Required = b.Required,
                          Jsidept = b.Jsidept,
                          JsicontactName = b.JsicontactName,
                          JsiContactEmail = b.JsicontactEmail

                      });

        }

    }
}
