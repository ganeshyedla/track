﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
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
        [HttpPost("CreateCustomerBusinessCategory")]
        public IActionResult CreateCustomerBusinessCategory([FromBody] CustomerBusinessCategory []CustomerBusinessCategory)
        {
            foreach (CustomerBusinessCategory  Bc in CustomerBusinessCategory)
            {
                _db.Add(Bc);
            }
            
            _db.SaveChanges();
            return Ok(CustomerBusinessCategory);
        }

        [HttpGet("CustomerList")]
        public IActionResult CustomerList()
        {
            return Ok((from o in _db.Customer
                       select new
                       {
                           CustomerID = o.CustomerId,
                           CustomerName = o.CustomerName,
                           BusinessCategory = (from i in _db.BusinessCategoryMaster
                                               join j in _db.CustomerBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                               where j.CustomerId== o.CustomerId
                                               select new { i.BusinessCategoryId,i.BusinessCategoryName }).ToList(),
                           Address = o.Address,
                           TaxNumber = o.TaxNumber,
                           Phone = o.Phone,
                           Email = o.Email,
                           ZipCode = o.ZipCode,
                           Juristiction = o.Juristiction,
                           ParentCustomer = _db.Customer.Where(u => u.CustomerId == o.ParentCustomerID).Select(u => u.CustomerName).SingleOrDefault()
                        }
                        )
                       );

        }

        [HttpGet("CustomerList{CustomerId:Int}")]
        public IActionResult CustomerList(int CustomerId)
        {
            return Ok((from o in _db.Customer
                       where o.CustomerId == CustomerId
                       select new
                       {
                           CustomerID = o.CustomerId,
                           CustomerName = o.CustomerName,
                           BusinessCategory = (from i in _db.BusinessCategoryMaster
                                               join j in _db.CustomerBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                               where j.CustomerId == o.CustomerId
                                               select new { i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                           Address = o.Address,
                           TaxNumber = o.TaxNumber,
                           Phone = o.Phone,
                           Email = o.Email,
                           ZipCode = o.ZipCode,
                           Juristiction = o.Juristiction,
                           ParentCustomer = _db.Customer.Where(u => u.CustomerId == o.ParentCustomerID).Select(u => u.CustomerName).SingleOrDefault()
                       }
            )
           );
        }

        [HttpGet("CustomerSearch")]
        public IActionResult CustomerSearch(string? BusinessCategory, string? Juristiction, string? State, string? City)
        {
            return Ok((from o in _db.Customer
                       join c in _db.CustomerBusinessCategory on o.CustomerId equals c.CustomerId
                       join i in _db.BusinessCategoryMaster on c.BusinessCategoryId equals i.BusinessCategoryId
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
                           BusinessCategory = (from i in _db.BusinessCategoryMaster
                                               join j in _db.CustomerBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                               where j.CustomerId == o.CustomerId
                                               select new { i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                           Address = o.Address,
                           TaxNumber = o.TaxNumber,
                           Phone = o.Phone,
                           Email = o.Email,
                           ZipCode = o.ZipCode,
                           Juristiction = o.Juristiction,

                       })); ;

        }
        [HttpGet("BusinessCategoryList")]
        public IActionResult BusinessCategoryList()
        {
            return Ok((from i in _db.BusinessCategoryMaster
                       select new
                       {
                           BusinessCategoryId = i.BusinessCategoryId,
                           BusinessCategory = i.BusinessCategoryName,
                           BusinessCategoryName = i.BusinessCategoryName

                       })); ;

        }
        
        [HttpGet("CustomerfulltextSearch")]
        public IActionResult CustomerfulltextSearch(string? Textsearch)
        {
            return Ok((from o in _db.Customer
                       join c in _db.CustomerBusinessCategory on o.CustomerId equals c.CustomerId
                       join i in _db.BusinessCategoryMaster on c.BusinessCategoryId equals i.BusinessCategoryId
                       where o.Juristiction != null && o.CustomerName != null
                                && o.Address != null && o.ZipCode != null
                                && o.Email != null && o.State != null && o.City != null
                                &&
                                (
                                o.Juristiction.Contains(Textsearch) || o.CustomerName.Contains(Textsearch) ||
                                o.Address.Contains(Textsearch) || o.ZipCode.ToString().Contains(Textsearch) ||
                                o.Email.Contains(Textsearch) || o.State.Contains(Textsearch) || o.City.Contains(Textsearch)
                                )
                       select new
                       {
                           CustomerID = o.CustomerId,
                           CustomerName = o.CustomerName,
                           BusinessCategory = (from i in _db.BusinessCategoryMaster
                                               join j in _db.CustomerBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                               where j.CustomerId == o.CustomerId
                                               select new { i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                           Address = o.Address,
                           TaxNumber = o.TaxNumber,
                           Phone = o.Phone,
                           Email = o.Email,
                           ZipCode = o.ZipCode,
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

            CustomerHistory CustomerHistory = new CustomerHistory();
            CustomerHistory.Title= customer.Title;
            CustomerHistory.CustomerId=customer.CustomerId;
            CustomerHistory.CustomerName=customer.CustomerName;
            CustomerHistory.Address = customer.Address;
            CustomerHistory.City = customer.City;
            CustomerHistory.State = customer.State;
            CustomerHistory.LocationCode = customer.ZipCode;
            CustomerHistory.Email = customer.Email;
            CustomerHistory.TaxNumber = customer.TaxNumber;
            CustomerHistory.CreateDate = customer.CreateDate;
            CustomerHistory.CreateUser = customer.CreateUser;
            CustomerHistory.Phone = customer.Phone;
            CustomerHistory.UpdateDate = customer.UpdateDate;
            CustomerHistory.UpdateUser = customer.UpdateUser;
            CustomerHistory.Juristiction = customer.Juristiction;
            CustomerHistory.Notes = customer.Notes;
            CustomerHistory.JSI_POC = customer.JSI_POC;
            CustomerHistory.Customer_POC = customer.Customer_POC;
            CustomerHistory.Dboperation = "Update Customer";
            CustomerHistory.Source = "CustomerUpdate API";
            _db.Update(customer);
            CreateCustomerHistory(CustomerHistory);
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

        [HttpGet("CustomerFilingMasterList")]
        public IActionResult CustomerFilingMasterList(int CustomerId)
        {
            return Ok(from
                      c in _db.Customer
                      join i in _db.CustomerFilingMaster on c.CustomerId equals i.CustomerId
                      join b in _db.FilingMaster on i.FilingId equals b.FilingId
                      where i.CustomerId == (CustomerId == 0 ? i.CustomerId : CustomerId )

                      select new
                      {
                          CustomerId = i.CustomerId,
                          customeName=c.CustomerName,
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

        [HttpGet("CustomerFilingMasterWorkflowList")]
        public IActionResult CustomerFilingMasterWorkflowList()
        {
            return Ok(from o in _db.CustomerFilingMasterDraft
                       join c in _db.Customer on o.CustomerID equals c.CustomerId
                       join w in _db.CustomerFilingMasterWorkflow on o.DraftId equals w.DraftId
                       join f in _db.FilingMaster on o.FilingID equals f.FilingId
                       join s in _db.Approvers on w.CurrentApproverID equals s.ApproverID
                       select new
                       {
                           CustomerID = c.CustomerId,
                           Customername = c.CustomerName,
                           FilingID = f.FilingId,
                           FilingName = f.FilingName,
                           FilingDescription = f.FilingDescription,
                           FilingFrequency = f.FilingFrequency,
                           StateInfo = f.StateInfo,
                           RuleInf = f.RuleInfo,
                           Required = f.Required,
                           BusinessCategory = (from i in _db.BusinessCategoryMaster
                                               join j in _db.FilingBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                               where j.FilingId == f.FilingId
                                               select new { i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                           Jsidept = f.Jsidept,
                           JsicontactName = f.JsicontactName,
                           JsicontactEmail = f.JsicontactEmail,
                           Juristiction = f.Juristiction,
                           Notes = f.Notes,
                           CreateDate = f.CreateDate,
                           CreateUser = f.CreateUser,
                           UpdateDate = f.UpdateDate,
                           UpdateUser = f.UpdateUser,
                           ChangesInprogress = f.ChangesInprogress,
                           ApproverName = s.ApproverName
                       }
                      );

        }
        [HttpGet("CustomerFilingMasterWorkflowListByApprover{UserID:Int}")]
        public IActionResult CustomerFilingMasterWorkflowListByApprover(long UserID)
        {
            return Ok((from o in _db.CustomerFilingMasterDraft
                       join c in _db.Customer on o.CustomerID equals c.CustomerId
                       join w in _db.CustomerFilingMasterWorkflow on o.DraftId equals w.DraftId
                       join f in _db.FilingMaster on o.FilingID equals f.FilingId
                       join s in _db.Approvers on w.CurrentApproverID equals s.ApproverID
                       where s.ApproverID == UserID
                       select new
                       {
                           CustomerID = c.CustomerId,
                           Customername = c.CustomerName,
                           FilingID = f.FilingId,
                           FilingName = f.FilingName,
                           FilingDescription = f.FilingDescription,
                           FilingFrequency = f.FilingFrequency,
                           StateInfo = f.StateInfo,
                           RuleInf = f.RuleInfo,
                           Required = f.Required,
                           BusinessCategory = (from i in _db.BusinessCategoryMaster
                                               join j in _db.FilingBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                               where j.FilingId == f.FilingId
                                               select new { i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                           Jsidept = f.Jsidept,
                           JsicontactName = f.JsicontactName,
                           JsicontactEmail = f.JsicontactEmail,
                           Juristiction = f.Juristiction,
                           Notes = f.Notes,
                           CreateDate = f.CreateDate,
                           CreateUser = f.CreateUser,
                           UpdateDate = f.UpdateDate,
                           UpdateUser = f.UpdateUser,
                           ChangesInprogress = f.ChangesInprogress,
                           ApproverName = s.ApproverName
                       }
                      ));
        }

    }
}
