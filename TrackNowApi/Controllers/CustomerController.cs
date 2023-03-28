using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text.RegularExpressions;
using TrackNowApi.Data;
using TrackNowApi.Model;

namespace TrackNowApi.Controllers
{
    //[Authorize]
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
                           CustomerId = o.CustomerId,
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
                           Notes= o.Notes,
                           City= o.City,
                           State= o.State,
                           ParentCustomer = _db.Customer.Where(u => u.CustomerId == o.ParentCustomerId).Select(u => u.CustomerName).SingleOrDefault(),
                           JSI_POC = o.JSI_POC,
                           Customer_POC = o.Customer_POC
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
                           CustomerId = o.CustomerId,
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
                           Notes = o.Notes,
                           City = o.City,
                           State = o.State,
                           ParentCustomer = _db.Customer.Where(u => u.CustomerId == o.ParentCustomerId).Select(u => u.CustomerName).SingleOrDefault(),
                           JSI_POC = o.JSI_POC,
                           Customer_POC = o.Customer_POC
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
                           CustomerId = o.CustomerId,
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
                           Notes = o.Notes,
                           City = o.City,
                           State = o.State,
                           ParentCustomer = _db.Customer.Where(u => u.CustomerId == o.ParentCustomerId).Select(u => u.CustomerName).SingleOrDefault(),
                           JSI_POC = o.JSI_POC,
                           Customer_POC = o.Customer_POC
                       })); ;

        }
        [HttpGet("BusinessCategoryList")]
        public IActionResult BusinessCategoryList()
        {
            return Ok((from i in _db.BusinessCategoryMaster
                       select new
                       {
                           BusinessCategoryId = i.BusinessCategoryId,
                           BusinessCategoryName = i.BusinessCategoryName,
                           BusinessCategoryDescription = i.BusinessCategoryDescription
                       })); ;

        }
        [HttpGet("CustomerBusinessCategoryList")]
        public IActionResult CustomerBusinessCategoryList()
        {
            return Ok((from c in _db.Customer                           
                       join cb in _db.CustomerBusinessCategory on c.CustomerId equals cb.CustomerId
                       join i in _db.BusinessCategoryMaster on cb.BusinessCategoryId equals i.BusinessCategoryId
                       select new
                       {
                           CustomerId=c.CustomerId, CustomerName=c.CustomerName,
                           BusinessCategoryId = i.BusinessCategoryId,
                           BusinessCategoryName = i.BusinessCategoryName,
                           BusinessCategoryDescription = i.BusinessCategoryDescription,
                           State = cb.State
                       })); ;

        }
        [HttpGet("CustomerBusinessCategoryList{CustomerId:Int}")]
        public IActionResult CustomerBusinessCategoryList(int CustomerId)
        {
            return Ok((from c in _db.Customer
                       join cb in _db.CustomerBusinessCategory on c.CustomerId equals cb.CustomerId
                       join i in _db.BusinessCategoryMaster on cb.BusinessCategoryId equals i.BusinessCategoryId
                       where c.CustomerId== CustomerId
                       select new
                       {
                           CustomerId = c.CustomerId,
                           CustomerName = c.CustomerName,
                           BusinessCategoryId = i.BusinessCategoryId,
                           BusinessCategoryName = i.BusinessCategoryName,
                           BusinessCategoryDescription = i.BusinessCategoryDescription,
                           State = cb.State
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
                           CustomerId = o.CustomerId,
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
                           Notes = o.Notes,
                           City = o.City,
                           State = o.State

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

        //[HttpPost("CreateCustomerFilingMaster")]
        //public IActionResult CreateCustomerFilingMaster([FromBody] CustomerFilingMaster CustomerFilingMaster)
        //{

        //    _db.Add(CustomerFilingMaster);
        //    _db.SaveChanges();

        //    return Ok(CustomerFilingMaster);
        //}

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
                          FilingId = i.FilingId,
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
        [HttpGet("BusinessBasedFilingMasterList")]
        public IActionResult BusinessBasedFilingMasterList(int CustomerId)
        {
            return Ok(from
                      c in _db.Customer
                      join cb in _db.CustomerBusinessCategory on c.CustomerId equals cb.CustomerId
                      join b in _db.BusinessCategoryMaster on cb.BusinessCategoryId equals b.BusinessCategoryId
                      join fm in _db.FilingBusinessCategory on b.BusinessCategoryId equals fm.BusinessCategoryId
                      join f in _db.FilingMaster on fm.FilingId equals f.FilingId
                      where (cb.CustomerId == CustomerId && f.StateInfo==cb.State ) || (cb.CustomerId == CustomerId && cb.State == null)

                      select new
                      {
                          CustomerId = c.CustomerId,
                          customeName = c.CustomerName,
                          FilingId = f.FilingId,
                          FilingDescription = f.FilingDescription,
                          FilingFrequency = f.FilingFrequency,
                          StateInfo = f.StateInfo,
                          RuleInfo = f.RuleInfo,
                          Juristiction = f.Juristiction,
                          Required = f.Required,
                          Jsidept = f.Jsidept,
                          JsicontactName = f.JsicontactName,
                          JsiContactEmail = f.JsicontactEmail

                      });

        }
        [HttpPut("CustomerFilingReject{WorkflowId:Int}")]
        public IActionResult CustomerFilingReject(int WorkflowId, string Userid, [FromBody] CustomerFilingMasterDraft CustomerFilingMasterDraft)
        {
            CustomerFilingMasterWorkflow CustomerFilingMasterWorkflow = (CustomerFilingMasterWorkflow)
                                                                        (from w in _db.CustomerFilingMasterWorkflow
                                                                         where w.WorkflowId == WorkflowId
                                                                         select w);
            if (CustomerFilingMasterWorkflow == null)
            {
                return BadRequest(ModelState);
            }
            CustomerFilingMasterWorkflow.WorkflowStatus = "Rejected";
            CustomerFilingMasterWorkflow.UpdateDate = DateTime.Now;
            CustomerFilingMasterWorkflow.UpdateUser = Userid;
            _db.CustomerFilingMasterWorkflow.Attach(CustomerFilingMasterWorkflow);
            _db.Entry(CustomerFilingMasterWorkflow).Property(x => x.WorkflowStatus).IsModified = true;
            _db.Entry(CustomerFilingMasterWorkflow).Property(x => x.UpdateDate).IsModified = true;
            _db.Entry(CustomerFilingMasterWorkflow).Property(x => x.UpdateUser).IsModified = true;

            //CustomerFilingMaster CustomerFilingMaster = (CustomerFilingMaster)
            //                                                         (from w in _db.CustomerFilingMaster
            //                                                          where w.FilingId == CustomerFilingMasterDraft.FilingId


            //CustomerFilingMaster. = "Rejected";
            //CustomerFilingMasterWorkflow.UpdateDate = DateTime.Now;
            //CustomerFilingMasterWorkflow.UpdateUser = Userid;
            //_db.CustomerFilingMasterWorkflow.Attach(CustomerFilingMasterWorkflow);
            //_db.Entry(CustomerFilingMasterWorkflow).Property(x => x.WorkflowStatus).IsModified = true;
            //_db.Entry(CustomerFilingMasterWorkflow).Property(x => x.UpdateDate).IsModified = true;
            //select w);


            _db.SaveChanges();

            return Ok(CustomerFilingMasterWorkflow);
        }
        [HttpPut("CustomerFilingApprove{WorkflowId:Int}")]
        public IActionResult CustomerFilingApprove(int WorkflowId, string Userid, [FromBody] CustomerFilingMasterDraft CustomerFilingMasterDraft)
        {
            CustomerFilingMasterWorkflow CustomerFilingMasterWorkflow = (CustomerFilingMasterWorkflow)
                                                                        (from w in _db.CustomerFilingMasterWorkflow
                                                                         where w.WorkflowId == WorkflowId
                                                                         select w);
            if (CustomerFilingMasterWorkflow == null)
            {
                return BadRequest(ModelState);
            }
            CustomerFilingMasterWorkflow.WorkflowStatus = "Rejected";
            CustomerFilingMasterWorkflow.UpdateDate = DateTime.Now;
            CustomerFilingMasterWorkflow.UpdateUser = Userid;
            _db.CustomerFilingMasterWorkflow.Attach(CustomerFilingMasterWorkflow);
            _db.Entry(CustomerFilingMasterWorkflow).Property(x => x.WorkflowStatus).IsModified = true;
            _db.Entry(CustomerFilingMasterWorkflow).Property(x => x.UpdateDate).IsModified = true;
            _db.Entry(CustomerFilingMasterWorkflow).Property(x => x.UpdateUser).IsModified = true;


            _db.SaveChanges();

            return Ok(CustomerFilingMasterWorkflow);
        }

        [HttpGet("CustomerFilingMasterWorkflowList")]
        public IActionResult CustomerFilingMasterWorkflowList()
        {
            return Ok(from o in _db.CustomerFilingMasterDraft
                       join c in _db.Customer on o.CustomerId equals c.CustomerId
                       join w in _db.CustomerFilingMasterWorkflow on o.DraftId equals w.DraftId
                       join f in _db.FilingMaster on o.FilingId equals f.FilingId
                       join s in _db.Approvers on w.CurrentApproverId equals s.ApproverId
                       select new
                       {
                           WorkflowId = w.WorkflowId,
                           DraftId = w.DraftId,
                           CustomerId = c.CustomerId,
                           Customername = c.CustomerName,
                           FilingId = f.FilingId,
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
        [HttpGet("CustomerFilingMasterWorkflowListByApprover{UserId:Int}")]
        public IActionResult CustomerFilingMasterWorkflowListByApprover(long UserId)
        {
            return Ok((from o in _db.CustomerFilingMasterDraft          
                       join c in _db.Customer on o.CustomerId equals c.CustomerId
                       join w in _db.CustomerFilingMasterWorkflow on o.DraftId equals w.DraftId
                       join f in _db.FilingMaster on o.FilingId equals f.FilingId
                       join s in _db.Approvers on w.CurrentApproverId equals s.ApproverId
                       where s.ApproverId == UserId
                       select new
                       {
                           WorkflowId= w.WorkflowId,
                           DraftId = w.DraftId,
                           CustomerId = c.CustomerId,
                           Customername = c.CustomerName,
                           FilingId = f.FilingId,
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

        [HttpPost("CreateCustomerComments")]
        public IActionResult CreateCustomerComments([FromBody] CustomerComments CustomerComments)
        {

            _db.Add(CustomerComments);
            _db.SaveChanges();

            return Ok(CustomerComments);
        }
        [HttpPost("CreateCustomerFilingComments")]
        public IActionResult CreateCustomerFilingComments([FromBody] CustomerFilingComments CustomerFilingComments)
        {

            _db.Add(CustomerFilingComments);
            _db.SaveChanges();

            return Ok(CustomerFilingComments);
        }
        [HttpGet("CustomerFilingWorkflowNotificationsList")]
        public IActionResult CustomerFilingWorkflowNotificationsList()
        {
            var CustomerFilingWorkflowNotifications = _db.CustomerFilingWorkflowNotifications.ToList();
            return Ok(CustomerFilingWorkflowNotifications);
            //    return await _db.CustomerFilingWorkflowNotifications.ToListAsync();
        }
        [HttpPost("CreateCustomerFilingDraftComments")]
        public IActionResult CreateCustomerFilingDraftComments([FromBody] CustomerFilingDraftComments CustomerFilingDraftComments)
        {

            _db.Add(CustomerFilingDraftComments);
            _db.SaveChanges();

            return Ok(CustomerFilingDraftComments);
        }
        [HttpPost("CreateCustomerFilingTrackingComments")]
        public IActionResult CreateCustomerFilingTrackingComments([FromBody] CustomerFilingTrackingComments CustomerFilingTrackingComments)
        {

            _db.Add(CustomerFilingTrackingComments);
            _db.SaveChanges();

            return Ok(CustomerFilingTrackingComments);
        }
        [HttpPost("CreateCustomerFilingWorkflowComments")]
        public IActionResult CreateCustomerFilingWorkflowComments([FromBody] CustomerFilingWorkflowComments CustomerFilingWorkflowComments)
        {

            _db.Add(CustomerFilingWorkflowComments);
            _db.SaveChanges();

            return Ok(CustomerFilingWorkflowComments);
        }
[       HttpPost("CustomerFilingWorkflowNotifications/Create")]
        public IActionResult Create([FromBody] CustomerFilingWorkflowNotifications item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _db.CustomerFilingWorkflowNotifications.Add(item);
            _db.SaveChanges();
            return Ok(item);

        }

        [HttpDelete("CustomerCommentsDelete{CommentsId:Int}")]
        public void CustomerCommentsDelete(int CommentsId)
        {
            CustomerComments CustomerComments;

            CustomerComments = _db.CustomerComments.Where(d => d.CommentsId == CommentsId).First();
            _db.CustomerComments.Remove(CustomerComments);
            _db.SaveChanges();

        }
        [HttpDelete("CustomerFilingCommentsDelete{CommentId:Int}")]
        public void CustomerFilingCommentsDelete(int CommentsId)
        {
            CustomerFilingComments CustomerFilingComments;

            CustomerFilingComments = _db.CustomerFilingComments.Where(d => d.CommentsId == CommentsId).First();
            _db.CustomerFilingComments.Remove(CustomerFilingComments);
            _db.SaveChanges();

        }
        [HttpDelete("CustomerFilingDraftCommentsDelete{CommentId:Int}")]
        public void CustomerFilingDraftCommentsDelete(int CommentsId)
        {
            CustomerFilingDraftComments CustomerFilingDraftComments;

            CustomerFilingDraftComments = _db.CustomerFilingDraftComments.Where(d => d.CommentsId == CommentsId).First();
            _db.CustomerFilingDraftComments.Remove(CustomerFilingDraftComments);
            _db.SaveChanges();

        }
        //[HttpGet("getbyid/{WorkflowId:int}")]
        //public ActionResult<CustomerFilingWorkflowNotifications> GetById(int WorkflowId)
        //{
        //    var res = _db.CustomerFilingWorkflowNotifications.FirstOrDefault(p => p.WorkflowId == WorkflowId);


        //    if (res != null)
        //    {
        //        return Ok(res);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }

           
        //}


        //[HttpPut("{WorkflowId}")]
        //public IActionResult Update(int WorkflowId, CustomerFilingWorkflowNotifications updatedCustomer)
        //{
        //    var customer = _db.CustomerFilingWorkflowNotifications.FirstOrDefault(p => p.WorkflowId == WorkflowId);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    customer.EmailFrom = updatedCustomer.EmailFrom;
           

        //    _db.SaveChanges();

        //    return Ok();
        //}

        [HttpDelete("CustomerFilingTrackingCommentsDelete{CommentId:Int}")]
        public void CustomerFilingTrackingCommentsDelete(int CommentsId)
        {
            CustomerFilingTrackingComments CustomerFilingTrackingComments;

            CustomerFilingTrackingComments = _db.CustomerFilingTrackingComments.Where(d => d.CommentsId == CommentsId).First();
            _db.CustomerFilingTrackingComments.Remove(CustomerFilingTrackingComments);
            _db.SaveChanges();

        }
        [HttpDelete("CustomerFilingWorkflowCommentsDelete{CommentId:Int}")]
        public void CustomerFilingWorkflowCommentsDelete(int CommentsId)
        {
            CustomerFilingWorkflowComments CustomerFilingWorkflowComments;

            CustomerFilingWorkflowComments = _db.CustomerFilingWorkflowComments.Where(d => d.CommentsId == CommentsId).First();
            _db.CustomerFilingWorkflowComments.Remove(CustomerFilingWorkflowComments);
            _db.SaveChanges();

        }
		//[HttpDelete("(delete/{WorkflowId:int}")]
  //      public IActionResult Delete(int WorkflowId)
  //      {
  //          var res = _db.CustomerFilingWorkflowNotifications.FirstOrDefault(t => t.WorkflowId == WorkflowId);
  //          if (res == null)
  //          {
  //              return NotFound();
  //          }

  //          _db.CustomerFilingWorkflowNotifications.Remove(res);
  //          _db.SaveChanges();
  //          return new NoContentResult();
  //      }
        
        [HttpGet("CustomerCommentsbyId{CommentsId:Int}")]
        public IActionResult CustomerCommentsbyId(int CommentsId)
        {
            return Ok((from o in _db.CustomerComments
                       where o.CommentsId == CommentsId
                       select new
                       {
                           CommentsId = o.CommentsId,
                           CommentsText= o.CommentsText,
                           InformationRead= o.InformationRead,
                           InformationDeleted= o.InformationDeleted,
                           CreateDate = o.CreateDate, UpdateDate = o.UpdateDate,
                           CreateUser= o.CreateUser,UpdateUser= o.UpdateUser

                        }));

        }
        [HttpGet("CustomerFilingCommentsbyId{CommentsId:Int}")]
        public IActionResult CustomerFilingCommentsbyId(int CommentsId)
        {
            return Ok((from o in _db.CustomerFilingComments
                       where o.CommentsId == CommentsId
                       select new
                       {
                           CommentsId = o.CommentsId,
                           CustomerId = o.CustomerId,
                           FilingId =o.FilingId,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }
        [HttpGet("CustomerFilingDraftCommentsbyId{CommentsId:Int}")]
        public IActionResult CustomerFilingDraftCommentsbyId(int CommentsId)
        {
            return Ok((from o in _db.CustomerFilingDraftComments
                       where o.CommentsId == CommentsId
                       select new
                       {
                           CommentsId = o.CommentsId,
                           DraftId = o.DraftId,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }
        [HttpGet("CustomerFilingTrackingCommentsbyId{CommentsId:Int}")]
        public IActionResult CustomerFilingTrackingCommentsbyId(int CommentsId)
        {
            return Ok((from o in _db.CustomerFilingTrackingComments
                       where o.CommentsId == CommentsId
                       select new
                       {
                           CommentsId = o.CommentsId,
                           FileTrackingId = o.FileTrackingId,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }
        [HttpGet("CustomerFilingWorkflowCommentsbyId{CommentsId:Int}")]
        public IActionResult CustomerFilingWorkflowCommentsbyId(int CommentsId)
        {
            return Ok((from o in _db.CustomerFilingWorkflowComments
                       where o.CommentsId == CommentsId
                       select new
                       {
                           CommentsId = o.CommentsId,
                           FileTrackingId = o.WorkflowId,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }
        

        [HttpGet("CustomerFilingDraftComments{DraftId:Int}")]
        public IActionResult CustomerFilingDraftComments(int DraftId)
        {
            return Ok((from o in _db.CustomerFilingDraftComments
                       where o.DraftId == DraftId
                       select new
                       {
                           CommentsId = o.CommentsId,
                           DraftId = o.DraftId,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }
        [HttpPut("CustomerCommentsUpdate{CommentsId:Int}")]
        public IActionResult CustomerCommentsUpdate(int CommentsId, [FromBody] CustomerComments CustomerComments)
        {
            if (CustomerComments == null || CustomerComments.CommentsId != CommentsId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(CustomerComments);
            _db.SaveChanges();
            return Ok(CustomerComments);
        }
        [HttpPut("CustomerFilingCommentsUpdate{CommentsId:Int}")]
        public IActionResult CustomerFilingCommentsUpdate(int CommentsId, [FromBody] CustomerFilingComments CustomerFilingComments)
        {
            if (CustomerFilingComments == null || CustomerFilingComments.CommentsId != CommentsId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(CustomerFilingComments);
            _db.SaveChanges();
            return Ok(CustomerFilingComments);
        }
        [HttpPut("CustomerFilingDraftCommentsUpdate{CommentsId:Int}")]
        public IActionResult CustomerFilingDraftCommentsUpdate(int CommentsId, [FromBody] CustomerFilingDraftComments CustomerFilingDraftComments)
        {
            if (CustomerFilingDraftComments == null || CustomerFilingDraftComments.CommentsId != CommentsId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(CustomerFilingDraftComments);
            _db.SaveChanges();
            return Ok(CustomerFilingDraftComments);
        }
        [HttpPut("CustomerFilingTrackingCommentsUpdate{CommentsId:Int}")]
        public IActionResult CustomerFilingTrackingCommentsUpdate(int CommentsId, [FromBody] CustomerFilingTrackingComments CustomerFilingTrackingComments)
        {
            if (CustomerFilingTrackingComments == null || CustomerFilingTrackingComments.CommentsId != CommentsId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(CustomerFilingTrackingComments);
            _db.SaveChanges();
            return Ok(CustomerFilingTrackingComments);
        }
        [HttpPut("CustomerFilingWorkflowCommentsUpdate{CommentsId:Int}")]
        public IActionResult CustomerFilingWorkflowCommentsUpdate(int CommentsId, [FromBody] CustomerFilingWorkflowComments CustomerFilingWorkflowComments)
        {
            if (CustomerFilingWorkflowComments == null || CustomerFilingWorkflowComments.CommentsId != CommentsId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(CustomerFilingWorkflowComments);
            _db.SaveChanges();
            return Ok(CustomerFilingWorkflowComments);
        }

        [HttpPost("CreateCustomerFilingTrackingCommentsAttachments")]
        public IActionResult CreateCustomerFilingTrackingCommentsAttachments([FromBody] CustomerFilingTrackingCommentsAttachments item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _db.CustomerFilingTrackingCommentsAttachments.Add(item);
            _db.SaveChanges();
            return Ok(item);


        }


        [HttpGet("CustomerFilingTrackingCommentsAttachmentstrackbyid/{AttachmentId:int}")]
        public ActionResult<CustomerFilingTrackingCommentsAttachments> CustomerFilingTrackingCommentsAttachmentsGetById(int AttachmentId)
        {
            var res = _db.CustomerFilingTrackingCommentsAttachments.FirstOrDefault(p => p.AttachmentId == AttachmentId);


            if (res != null)
            {
                return Ok(res);
            }
            else
            {
                return NotFound();
            }


        }


        [HttpPut("CustomerFilingTrackingCommentsAttachmentsupdate/{AttachmentId}")]
        public IActionResult CustomerFilingTrackingCommentsAttachmentsUpdate(int AttachmentId, CustomerFilingTrackingCommentsAttachments updatedCustomer)
        {
            var customer = _db.CustomerFilingTrackingCommentsAttachments.FirstOrDefault(p => p.AttachmentId == AttachmentId);

            if (customer == null)
            {
                return NotFound();
            }

            customer.AttachmentPath= updatedCustomer.AttachmentPath;


            _db.SaveChanges();

            return Ok();
        }



        [HttpDelete("(CustomerFilingTrackingCommentsAttachmentsdelete/{AttachmentId:int}")]
        public IActionResult CustomerFilingTrackingCommentsAttachmentsDelete(int AttachmentId)
        {
            var res = _db.CustomerFilingTrackingCommentsAttachments.FirstOrDefault(t => t.AttachmentId == AttachmentId);
            if (res == null)
            {
                return NotFound();
            }

            _db.CustomerFilingTrackingCommentsAttachments.Remove(res);
            _db.SaveChanges();
            return new NoContentResult();
        }

        [HttpGet("CustomerComments{CustomerId:Int}")]
        public IActionResult CustomerComments(int CustomerId)
        {
            return Ok((from o in _db.CustomerComments
                       where o.CustomerId == CustomerId
                       select new
                       {
                           CommentsId = o.CommentsId,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser

                       }));

        }
        [HttpGet("CustomerFilingComments{CustomerId:Int}")]
        public IActionResult CustomerFilingComments(int CustomerId)
        {
            return Ok((from o in _db.CustomerFilingComments
                       where o.CustomerId == CustomerId 
                       select new
                       {
                           CommentsId = o.CommentsId,
                           CustomerId = o.CustomerId,
                           FilingId = o.FilingId,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }
        [HttpGet("CustomerFilingTrackingComments{FileTrackingId:Int}")]
        public IActionResult CustomerFilingTrackingComments(int FileTrackingId)
        {
            return Ok((from o in _db.CustomerFilingTrackingComments
                       where o.FileTrackingId == FileTrackingId
                       select new
                       {
                           CommentsId = o.CommentsId,
                           FileTrackingId = o.FileTrackingId,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }
        [HttpGet("CustomerFilingWorkflowComments{WorkflowId:Int}")]
        public IActionResult CustomerFilingWorkflowComments(int WorkflowId)
        {
            return Ok((from o in _db.CustomerFilingWorkflowComments
                       where o.WorkflowId == WorkflowId
                       select new
                       {
                           CommentsId = o.CommentsId,
                           FileTrackingId = o.WorkflowId,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }
//===================================================================================

        [HttpPost("createCustomerAttachments")]
        public IActionResult createCustomerAttachments(CustomerAttachments Customer)
        {
            try
            {
                    _db.CustomerAttachments.Add(Customer);
                    _db.SaveChanges();
                    return Ok(Customer); 
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("ListCustomerAttachments")]
        public IActionResult ListCustomerAttachments()
        {
            try
            {
                
                    var CustomerAttachments = _db.CustomerAttachments.ToList();
                    return Ok(CustomerAttachments);
                

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ViewCustomerAttachments/{CustomerId:Int}")]
        public IActionResult ViewCustomerAttachments(int CustomerId)
        {
            try
            {

                var CustomerAttachments = _db.CustomerAttachments
                                       .FirstOrDefault(F => F.CustomerId == CustomerId);
                if (CustomerAttachments != null)
                {
                    return Ok(CustomerAttachments);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteCustomerAttachments/{CustomerId:Int}")]
        public IActionResult DeleteCustomerAttachments(int CustomerId)
        {
            try
            {

                var CustomerAttachments = _db.CustomerAttachments
                                                 .FirstOrDefault(a => a.CustomerId == CustomerId);

                if (CustomerAttachments != null)
                {
                    _db.CustomerAttachments.Remove(CustomerAttachments);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }

        [HttpPut("UpdateCustomerAttachments/{CustomerId:int}")]
        public IActionResult UpdateCustomerAttachments(int CustomerId, [FromBody] CustomerAttachments CustomerAttachments)
        {
            try
            {

                var existingNotification = _db.CustomerAttachments.
                                      FirstOrDefault(n => n.CustomerId == CustomerId);

                if (existingNotification != null)

                {
                    existingNotification.CustomerId = CustomerAttachments.CustomerId;
                    existingNotification.AttachmentId = CustomerAttachments.AttachmentId;
                    existingNotification.AttachmentPath = CustomerAttachments.AttachmentPath;
                    existingNotification.CreateDate = CustomerAttachments.CreateDate;
                    existingNotification.CreateUser = CustomerAttachments.CreateUser;
                    existingNotification.UpdatedDate = CustomerAttachments.UpdatedDate;
                    existingNotification.UpdatedUser = CustomerAttachments.UpdatedUser;

                    _db.SaveChanges();
                    return Ok(existingNotification);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

//================================================================================================
        [HttpPost("createCustomerFilingAttachments")]
        public IActionResult createCustomerFilingAttachments(CustomerFilingAttachments Customer)
        {
            try
            {
                _db.CustomerFilingAttachments.Add(Customer);
                _db.SaveChanges();
                return Ok(Customer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("ListCustomerFilingAttachments")]
        public IActionResult ListCustomerFilingAttachments()
        {
            try
            {

                var CustomerFilingAttachments = _db.CustomerFilingAttachments.ToList();
                return Ok(CustomerFilingAttachments);


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ViewCustomerFilingAttachments/{FollowupId:Int}")]
        public IActionResult ViewCustomerFilingAttachments(int FollowupId)
        {
            try
            {

                var CustomerFilingAttachments = _db.CustomerFilingAttachments
                                       .FirstOrDefault(F => F.FollowupId == FollowupId);
                if (CustomerFilingAttachments != null)
                {
                    return Ok(CustomerFilingAttachments);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteCustomerFilingAttachments/{FollowupId:Int}")]
        public IActionResult DeleteCustomerFilingAttachments(int FollowupId)
        {
            try
            {

                var CustomerFilingAttachments = _db.CustomerFilingAttachments
                                                 .FirstOrDefault(a => a.FollowupId == FollowupId);

                if (CustomerFilingAttachments != null)
                {
                    _db.CustomerFilingAttachments.Remove(CustomerFilingAttachments);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }

        [HttpPut("UpdateCustomerFilingAttachments/{FollowupId:int}")]
        public IActionResult UpdateCustomerFilingAttachments(int FollowupId, [FromBody] CustomerFilingAttachments CustomerFilingAttachments)
        {
            try
            {

                var existingNotification = _db.CustomerFilingAttachments.
                                      FirstOrDefault(n => n.FollowupId == FollowupId);

                if (existingNotification != null)

                {
                    existingNotification.AttachmentId = CustomerFilingAttachments.AttachmentId;
                    

                    _db.SaveChanges();
                    return Ok(existingNotification);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
//===================================================================================================================


        [HttpPost("CreateCustomerFilingCommentsAttachments")]
        public IActionResult CreateCustomerFilingCommentsAttachments(CustomerFilingCommentsAttachments Customer)
        {
            try
            {
                _db.CustomerFilingCommentsAttachments.Add(Customer);
                _db.SaveChanges();
                return Ok(Customer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("ListCustomerFilingCommentsAttachments")]
        public IActionResult ListCustomerFilingCommentsAttachments()
        {
            try
            {

                var CustomerFilingCommentsAttachments = _db.CustomerFilingCommentsAttachments.ToList();
                return Ok(CustomerFilingCommentsAttachments);


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ViewCustomerFilingCommentsAttachments/{AttachmentId:Int}")]
        public IActionResult ViewCustomerFilingCommentsAttachments(int AttachmentId)
        {
            try
            {

                var CustomerFilingCommentsAttachments = _db.CustomerFilingCommentsAttachments
                                       .FirstOrDefault(F => F.AttachmentId == AttachmentId);
                if (CustomerFilingCommentsAttachments != null)
                {
                    return Ok(CustomerFilingCommentsAttachments);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteCustomerFilingCommentsAttachments/{AttachmentId:Int}")]
        public IActionResult DeleteCustomerFilingCommentsAttachments(int AttachmentId)
        {
            try
            {

                var CustomerFilingCommentsAttachments = _db.CustomerFilingCommentsAttachments
                                                 .FirstOrDefault(a => a.AttachmentId == AttachmentId);

                if (CustomerFilingCommentsAttachments != null)
                {
                    _db.CustomerFilingCommentsAttachments.Remove(CustomerFilingCommentsAttachments);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }

        [HttpPut("UpdateCustomerFilingCommentsAttachments/{AttachmentId:int}")]
        public IActionResult UpdateCustomerFilingCommentsAttachments(int AttachmentId, [FromBody] CustomerFilingCommentsAttachments CustomerFilingCommentsAttachments)
        {
            try
            {

                var existingNotification = _db.CustomerFilingCommentsAttachments.
                                      FirstOrDefault(n => n.AttachmentId == AttachmentId);

                if (existingNotification != null)

                {
                    existingNotification.AttachmentPath = CustomerFilingCommentsAttachments.AttachmentPath;
                    existingNotification.CommentsId = CustomerFilingCommentsAttachments.CommentsId;
                    existingNotification.CreateDate = CustomerFilingCommentsAttachments.CreateDate;
                    existingNotification.CreateUser = CustomerFilingCommentsAttachments.CreateUser;
                    existingNotification.UpdatedDate = CustomerFilingCommentsAttachments.UpdatedDate;
                    existingNotification.UpdatedUser = CustomerFilingCommentsAttachments.UpdatedUser;


                    _db.SaveChanges();
                    return Ok(existingNotification);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

//==================================================================================================================

        [HttpPost("CreateCustomerFilingDraftAttachments")]
        public IActionResult CreateCustomerFilingDraftAttachments(CustomerFilingDraftAttachments Customer)
        {
            try
            {
                _db.CustomerFilingDraftAttachments.Add(Customer);
                _db.SaveChanges();
                return Ok(Customer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("ListCustomerFilingDraftAttachments")]
        public IActionResult ListCustomerFilingDraftAttachments()
        {
            try
            {

                var CustomerFilingDraftAttachments = _db.CustomerFilingDraftAttachments.ToList();
                return Ok(CustomerFilingDraftAttachments);


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ViewCustomerFilingDraftAttachments/{DraftId:Int}")]
        public IActionResult ViewCustomerFilingDraftAttachments(int DraftId)
        {
            try
            {

                var CustomerFilingDraftAttachments = _db.CustomerFilingDraftAttachments
                                       .FirstOrDefault(F => F.DraftId == DraftId);
                if (CustomerFilingDraftAttachments != null)
                {
                    return Ok(CustomerFilingDraftAttachments);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteCustomerFilingDraftAttachments/{DraftId:Int}")]
        public IActionResult DeleteCustomerFilingDraftAttachments(int DraftId)
        {
            try
            {

                var CustomerFilingDraftAttachments = _db.CustomerFilingDraftAttachments
                                                 .FirstOrDefault(a => a.DraftId == DraftId);

                if (CustomerFilingDraftAttachments != null)
                {
                    _db.CustomerFilingDraftAttachments.Remove(CustomerFilingDraftAttachments);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }

        [HttpPut("UpdateCustomerFilingDraftAttachments/{DraftId:int}")]
        public IActionResult UpdateCustomerFilingDraftAttachments(int DraftId, [FromBody] CustomerFilingDraftAttachments CustomerFilingDraftAttachments)
        {
            try
            {

                var existingNotification = _db.CustomerFilingDraftAttachments.
                                      FirstOrDefault(n => n.DraftId == DraftId);

                if (existingNotification != null)

                {
                    existingNotification.AttachmentId = CustomerFilingDraftAttachments.AttachmentId;
                    existingNotification.AttachmentPath = CustomerFilingDraftAttachments.AttachmentPath;
                    existingNotification.CreateDate = CustomerFilingDraftAttachments.CreateDate;
                    existingNotification.CreateUser = CustomerFilingDraftAttachments.CreateUser;
                    existingNotification.UpdatedDate = CustomerFilingDraftAttachments.UpdatedDate;
                    existingNotification.UpdatedUser = CustomerFilingDraftAttachments.UpdatedUser;


                    _db.SaveChanges();
                    return Ok(existingNotification);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
 //==================================================================================================================

        [HttpPost("CreateCustomerFilingDraftCommentsAttachments")]
        public IActionResult CreateCustomerFilingDraftCommentsAttachments(CustomerFilingDraftCommentsAttachments Customer)
        {
            try
            {
                _db.CustomerFilingDraftCommentsAttachments.Add(Customer);
                _db.SaveChanges();
                return Ok(Customer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("ListCustomerFilingDraftCommentsAttachments")]
        public IActionResult ListCustomerFilingDraftCommentsAttachments()
        {
            try
            {

                var CustomerFilingDraftCommentsAttachments = _db.CustomerFilingDraftCommentsAttachments.ToList();
                return Ok(CustomerFilingDraftCommentsAttachments);


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ViewCustomerFilingDraftCommentsAttachments/{AttachmentId:Int}")]
        public IActionResult ViewCustomerFilingDraftCommentsAttachments(int AttachmentId)
        {
            try
            {

                var CustomerFilingDraftCommentsAttachments = _db.CustomerFilingDraftCommentsAttachments
                                       .FirstOrDefault(F => F.AttachmentId == AttachmentId);
                if (CustomerFilingDraftCommentsAttachments != null)
                {
                    return Ok(CustomerFilingDraftCommentsAttachments);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteCustomerFilingDraftCommentsAttachments/{AttachmentId:Int}")]
        public IActionResult DeleteCustomerFilingDraftCommentsAttachments(int AttachmentId)
        {
            try
            {

                var CustomerFilingDraftCommentsAttachments = _db.CustomerFilingDraftCommentsAttachments
                                                 .FirstOrDefault(a => a.AttachmentId == AttachmentId);

                if (CustomerFilingDraftCommentsAttachments != null)
                {
                    _db.CustomerFilingDraftCommentsAttachments.Remove(CustomerFilingDraftCommentsAttachments);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }

        [HttpPut("UpdateCustomerFilingDraftCommentsAttachments/{AttachmentId:int}")]
        public IActionResult UpdateCustomerFilingDraftCommentsAttachments(int AttachmentId, [FromBody] CustomerFilingDraftCommentsAttachments CustomerFilingDraftCommentsAttachments)
        {
            try
            {

                var existingNotification = _db.CustomerFilingDraftCommentsAttachments.
                                      FirstOrDefault(n => n.AttachmentId == AttachmentId);

                if (existingNotification != null)

                {
                    existingNotification.AttachmentPath = CustomerFilingDraftCommentsAttachments.AttachmentPath;
                    existingNotification.CommentsId = CustomerFilingDraftCommentsAttachments.CommentsId;
                    existingNotification.CreateDate = CustomerFilingDraftCommentsAttachments.CreateDate;
                    existingNotification.CreateUser = CustomerFilingDraftCommentsAttachments.CreateUser;
                    existingNotification.UpdatedDate = CustomerFilingDraftCommentsAttachments.UpdatedDate;
                    existingNotification.UpdatedUser = CustomerFilingDraftCommentsAttachments.UpdatedUser;


                    _db.SaveChanges();
                    return Ok(existingNotification);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
 //================================================================================================================

        [HttpPost("CreateCustomerFilingTrackingAttachments")]
        public IActionResult CreateCustomerFilingTrackingAttachments(CustomerFilingTrackingAttachments Customer)
        {
            try
            {
                _db.CustomerFilingTrackingAttachments.Add(Customer);
                _db.SaveChanges();
                return Ok(Customer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("ListCustomerFilingTrackingAttachments")]
        public IActionResult ListCustomerFilingTrackingAttachments()
        {
            try
            {

                var CustomerFilingTrackingAttachments = _db.CustomerFilingTrackingAttachments.ToList();
                return Ok(CustomerFilingTrackingAttachments);


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ViewCustomerFilingTrackingAttachments/{FileTrackingId:Int}")]
        public IActionResult ViewCustomerFilingTrackingAttachments(int FileTrackingId)
        {
            try
            {

                var CustomerFilingTrackingAttachments = _db.CustomerFilingTrackingAttachments
                                       .FirstOrDefault(F => F.FileTrackingId == FileTrackingId);
                if (CustomerFilingTrackingAttachments != null)
                {
                    return Ok(CustomerFilingTrackingAttachments);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteCustomerFilingTrackingAttachments/{FileTrackingId:Int}")]
        public IActionResult DeleteCustomerFilingTrackingAttachments(int FileTrackingId)
        {
            try
            {

                var CustomerFilingTrackingAttachments = _db.CustomerFilingTrackingAttachments
                                                 .FirstOrDefault(a => a.FileTrackingId == FileTrackingId);

                if (CustomerFilingTrackingAttachments != null)
                {
                    _db.CustomerFilingTrackingAttachments.Remove(CustomerFilingTrackingAttachments);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }

        [HttpPut("UpdateCustomerFilingTrackingAttachments/{FileTrackingId:int}")]
        public IActionResult UpdateCustomerFilingTrackingAttachments(int FileTrackingId, [FromBody] CustomerFilingTrackingAttachments CustomerFilingTrackingAttachments)
        {
            try
            {

                var existingNotification = _db.CustomerFilingTrackingAttachments.
                                      FirstOrDefault(n => n.FileTrackingId == FileTrackingId);

                if (existingNotification != null)

                {
                    existingNotification.AttachmentId = CustomerFilingTrackingAttachments.AttachmentId;
                    existingNotification.AttachmentPath = CustomerFilingTrackingAttachments.AttachmentPath;
                    existingNotification.CreateDate = CustomerFilingTrackingAttachments.CreateDate;
                    existingNotification.CreateUser = CustomerFilingTrackingAttachments.CreateUser;
                    existingNotification.UpdatedDate = CustomerFilingTrackingAttachments.UpdatedDate;
                    existingNotification.UpdatedUser = CustomerFilingTrackingAttachments.UpdatedUser;


                    _db.SaveChanges();
                    return Ok(existingNotification);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
//=====================================================================================================================
        [HttpDelete("DeleteCustomerFilingTrackingNotification/{NotificationId:decimal}")]
        public IActionResult DeleteCustomerFilingTrackingNotification(decimal NotificationId)
        {
            try
            {
                //using (var Customer = new Model.TrackNowContext())
                {
                    var CustomerFilingTrackingNotification = _db.CustomerFilingTrackingNotifications
                                                .FirstOrDefault(n => n.NotificationId == NotificationId);

                    if (CustomerFilingTrackingNotification != null)
                    {
                        _db.CustomerFilingTrackingNotifications.Remove(CustomerFilingTrackingNotification);
                        _db.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {

                // return StatusCode(StatusCodes.Status500InternalServerError);

                return NotFound(ex.Message);
            }
        }


        [HttpPost("CreateCustomerFilingTrackingNotifications")]
        public IActionResult CreateCustomerFilingTrackingNotifications(CustomerFilingTrackingNotifications notification)
        {
            try
            {
                // using (var Customer = new Models.TrackNowContext())
                {
                    _db.CustomerFilingTrackingNotifications.Add(notification);
                    int i = _db.SaveChanges();
                    //return CreatedAtRoute("GetNotification", new { WorkflowId = notification.WorkflowId }, notification);
                    return Ok("number of row effected is " + i);
                }
            }
            catch (Exception ex)
            {

                // return StatusCode(StatusCodes.Status500InternalServerError);
                //Console.WriteLine(ex.Message);
                //return null;
                return NotFound(ex.Message);

            }
        }


        [HttpGet("ViewCustomerFilingTrackingNotification/{NotificationId:decimal}")]
        public IActionResult ViewCustomerFilingTrackingNotification(decimal NotificationId)
        {
            try
            {
                //using (var Customer = new Models.TrackNowContext())
                {
                    var CustomerFilingTrackingNotification = _db.CustomerFilingTrackingNotifications
                                                            .FirstOrDefault(n => n.NotificationId == NotificationId);

                    //var CustomerFilingTrackingNotification = from t in Customer.CustomerFilingTrackingNotifications
                    //                                         where t.NotificationId == NotificationId
                    //                                         select t;

                    if (CustomerFilingTrackingNotification != null)
                    {
                        return Ok(CustomerFilingTrackingNotification);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the view operation
                // return StatusCode(StatusCodes.Status500InternalServerError);
                return NotFound(ex.Message);
            }
        }



        [HttpGet("ListCustomerFilingTrackingNotifications")]
        public IActionResult ListCustomerFilingTrackingNotifications()
        {
            try
            {
                //using (var Customer = new Models.TrackNowContext())
                {
                    var CustomerFilingTrackingNotifications = _db.CustomerFilingTrackingNotifications.ToList();
                    return Ok(CustomerFilingTrackingNotifications);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the list operation
                //return StatusCode(StatusCodes.Status500InternalServerError);
                return NotFound(ex.Message);
            }
        }

        [HttpPut("UpdateCustomerFilingTrackingNotification/{NotificationId:decimal}")]
        public IActionResult UpdateCustomerFilingTrackingNotification(decimal NotificationId, [FromBody] CustomerFilingTrackingNotifications customerFilingTrackingNotification)
        {
            try
            {
                //  using (var Customer = new Models.TrackNowContext())
                {
                    var existingNotification = _db.CustomerFilingTrackingNotifications.
                                          FirstOrDefault(n => n.NotificationId == NotificationId);

                    if (existingNotification != null)

                    {
                        existingNotification.WorkflowId = customerFilingTrackingNotification.WorkflowId;
                        existingNotification.EmailFrom = customerFilingTrackingNotification.EmailFrom;
                        existingNotification.EmailTo = customerFilingTrackingNotification.EmailTo;
                        existingNotification.EmailCc = customerFilingTrackingNotification.EmailCc;
                        existingNotification.EmailSubject = customerFilingTrackingNotification.EmailSubject;
                        existingNotification.NotificationType = customerFilingTrackingNotification.NotificationType;
                        existingNotification.NotificationText = customerFilingTrackingNotification.NotificationText;
                        existingNotification.InformationRead = customerFilingTrackingNotification.InformationRead;
                        existingNotification.InformationDeleted = customerFilingTrackingNotification.InformationDeleted;
                        existingNotification.CreateDate = customerFilingTrackingNotification.CreateDate;
                        existingNotification.CreateUser = customerFilingTrackingNotification.CreateUser;



                        _db.SaveChanges();
                        return Ok(existingNotification);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the update operation
                // return StatusCode(StatusCodes.Status500InternalServerError);
                return NotFound(ex.Message);
            }


        }
 //=====================================================================================================

        [HttpPost("CreateCustomerApprovalStatus")]
        public IActionResult CreateCustomerApprovalStatus(CustomerApprovalStatus Approver)
        {
            try
            {
                _db.CustomerApprovalStatus.Add(Approver);
                _db.SaveChanges();
                return Ok(Approver);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ViewCustomerApprovalStatus/{CustomerApprovalId:int}")]
        public IActionResult ViewCustomerApprovalStatus(int CustomerApprovalId)
        {
            try
            {
                var CustomerApprovalStatus = _db.CustomerApprovalStatus
                                               .FirstOrDefault(n => n.CustomerApprovalId == CustomerApprovalId);

                if (CustomerApprovalStatus != null)
                {
                    return Ok(CustomerApprovalStatus);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ListCustomerApprovalStatus")]
        public IActionResult ListCustomerApprovalStatus()
        {
            try
            {
                var CustomerApprovalStatus = _db.CustomerApprovalStatus.ToList();
                return Ok(CustomerApprovalStatus);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("DeleteCustomerApprovalStatus/{CustomerApprovalId:int}")]
        public IActionResult DeleteCustomerApprovalStatus(int CustomerApprovalId)
        {
            try
            {

                var CustomerApprovalStatus = _db.CustomerApprovalStatus
                                            .FirstOrDefault(n => n.CustomerApprovalId == CustomerApprovalId);

                if (CustomerApprovalStatus != null)
                {
                    _db.CustomerApprovalStatus.Remove(CustomerApprovalStatus);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("UpdateCustomerApprovalStatus/{CustomerApprovalId:int}")]
        public IActionResult UpdateCustomerApprovalStatus(int CustomerApprovalId, [FromBody] CustomerApprovalStatus CustomerApprovalStatus)
        {
            try
            {

                var existingNotification = _db.CustomerApprovalStatus.
                                      FirstOrDefault(n => n.CustomerApprovalId == CustomerApprovalId);

                if (existingNotification != null)

                {
                    existingNotification.WorkflowId = CustomerApprovalStatus.WorkflowId;
                    existingNotification.ApproverName = CustomerApprovalStatus.ApproverName;
                    existingNotification.AlternateApprovers = CustomerApprovalStatus.AlternateApprovers;
                    existingNotification.Comments = CustomerApprovalStatus.Comments;
                    existingNotification.Attachments = CustomerApprovalStatus.Attachments;
                    existingNotification.Status = CustomerApprovalStatus.Status;
                    existingNotification.CreatedDate = CustomerApprovalStatus.CreatedDate;
                    existingNotification.CreatedUser = CustomerApprovalStatus.CreatedUser;
                    existingNotification.UpdateDate = CustomerApprovalStatus.UpdateDate;
                    existingNotification.UpdateUser = CustomerApprovalStatus.UpdateUser;
                    existingNotification.DoneBy = CustomerApprovalStatus.Comments;
                    existingNotification.DoneOn = CustomerApprovalStatus.DoneOn;

                    _db.SaveChanges();
                    return Ok(existingNotification);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
//=========================================================================================================================

       
        [HttpGet("ViewCustomerComments/{CommentsId:int}")]
        public IActionResult ViewCustomerComments(int CommentsId)
        {
            try
            {
                var CustomerComments = _db.CustomerComments
                                               .FirstOrDefault(n => n.CommentsId == CommentsId);

                if (CustomerComments != null)
                {
                    return Ok(CustomerComments);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("CustomerFilingWorkflowCommentsAttachmentsList")]
        public IActionResult CustomerFilingWorkflowCommentsAttachmentsList()
        {
            var res = _db.CustomerFilingWorkflowCommentsAttachments.ToList();
            return Ok(res);
        }

        [HttpPost("CustomerFilingWorkflowCommentsAttachmentsCreate")]
        public IActionResult CustomerFilingWorkflowCommentsAttachments([FromBody] CustomerFilingWorkflowCommentsAttachments item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            _db.CustomerFilingWorkflowCommentsAttachments.Add(item);
            _db.SaveChanges();
            return Ok(item);
        }
        [HttpGet("CustomerFilingWorkflowCommentsAttachmentsbyid/{AttachmentId:int}")]
        public ActionResult<CustomerFilingWorkflowCommentsAttachments> CustomerFilingWorkflowCommentsAttachmentsGetById(int AttachmentId)
        {
            var res = _db.CustomerFilingWorkflowCommentsAttachments.FirstOrDefault(p => p.AttachmentId == AttachmentId);
            if (res != null)
            {
                return Ok(res);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("CustomerFilingWorkflowCommentsAttachmentsupdate/{AttachmentId}")]
        public IActionResult CustomerFilingWorkflowCommentsAttachmentsUpdate(int AttachmentId, CustomerFilingWorkflowCommentsAttachments updatedCustomer)
        {
            var customer = _db.CustomerFilingWorkflowCommentsAttachments.FirstOrDefault(p => p.AttachmentId == AttachmentId);
            if (customer == null)
            {
                return NotFound();
            }
            customer.AttachmentPath = updatedCustomer.AttachmentPath;
            _db.SaveChanges();
            return Ok();
        }
        [HttpDelete("CustomerFilingWorkflowCommentsAttachmentsdelete/{AttachmentId:int}")]
        public IActionResult CustomerFilingWorkflowCommentsAttachmentsDelete(int AttachmentId)
        {
            var res = _db.CustomerFilingWorkflowCommentsAttachments.FirstOrDefault(t => t.AttachmentId == AttachmentId);
            if (res == null)
            {
                return NotFound();
            }
            _db.CustomerFilingWorkflowCommentsAttachments.Remove(res);
            _db.SaveChanges();
            return new NoContentResult();
        }


        [HttpGet("CustomerFilingMasterHistoryList")]
        public IActionResult CustomerFilingMasterHistoryList()
        {
            var res = _db.CustomerFilingMasterHistory.ToList();
            return Ok(res);
        }

        [HttpPost("CustomerFilingMasterHistoryCreate")]
        public IActionResult CustomerFilingMasterHistory([FromBody] CustomerFilingMasterHistory item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            _db.CustomerFilingMasterHistory.Add(item);
            _db.SaveChanges();
            return Ok(item);
        }
        [HttpGet("CustomerFilingMasterHistorybyid/{historyid:int}")]
        public ActionResult<CustomerFilingMasterHistory> CustomerFilingMasterHistoryGetById(int historyid)
        {
            var res = _db.CustomerFilingMasterHistory.FirstOrDefault(p => p.historyid == historyid);
            if (res != null)
            {
                return Ok(res);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("CustomerFilingMasterHistoryupdate/{historyid}")]
        public IActionResult CustomerFilingMasterHistoryUpdate(int historyid, CustomerFilingMasterHistory updatedCustomer)
        {
            var customer = _db.CustomerFilingMasterHistory.FirstOrDefault(p => p.historyid == historyid);
            if (customer == null)
            {
                return NotFound();
            }
            customer.UpdateUser = updatedCustomer.UpdateUser;
            _db.SaveChanges();
            return Ok();
        }
        [HttpDelete("CustomerFilingMasterHistorydelete/{historyid:int}")]
        public IActionResult CustomerFilingMasterHistoryDelete(int historyid)
        {
            var res = _db.CustomerFilingMasterHistory.FirstOrDefault(t => t.historyid == historyid);
            if (res == null)
            {
                return NotFound();
            }
            _db.CustomerFilingMasterHistory.Remove(res);
            _db.SaveChanges();
            return new NoContentResult();
        }

        [HttpGet("CustomerFilingMasterWorkflowLists")]
        public IActionResult CustomerFilingMasterWorkflowLists()
        {
            var res = _db.CustomerFilingMasterWorkflow.ToList();
            return Ok(res);
        }

        [HttpPost("CustomerFilingMasterWorkflowCreate")]
        public IActionResult CustomerFilingMasterWorkflow([FromBody] CustomerFilingMasterWorkflow item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            _db.CustomerFilingMasterWorkflow.Add(item);
            _db.SaveChanges();
            return Ok(item);
        }
        [HttpGet("CustomerFilingMasterWorkflowbyid/{WorkflowId:int}")]
        public ActionResult<CustomerFilingMasterWorkflow> CustomerFilingMasterWorkflowGetById(int WorkflowId)
        {
            var res = _db.CustomerFilingMasterWorkflow.FirstOrDefault(p => p.WorkflowId == WorkflowId);
            if (res != null)
            {
                return Ok(res);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("CustomerFilingMasterWorkflowupdate/{WorkflowId}")]
        public IActionResult CustomerFilingMasterWorkflowUpdate(int WorkflowId, CustomerFilingMasterWorkflow updatedCustomer)
        {
            var customer = _db.CustomerFilingMasterWorkflow.FirstOrDefault(p => p.WorkflowId == WorkflowId);
            if (customer == null)
            {
                return NotFound();
            }
            customer.UpdateUser = updatedCustomer.UpdateUser;
            _db.SaveChanges();
            return Ok();
        }
        [HttpDelete("CustomerFilingMasterWorkflowdelete/{WorkflowId:int}")]
        public IActionResult CustomerFilingMasterWorkflowDelete(int WorkflowId)
        {
            var res = _db.CustomerFilingMasterWorkflow.FirstOrDefault(t => t.WorkflowId == WorkflowId);
            if (res == null)
            {
                return NotFound();
            }
            _db.CustomerFilingMasterWorkflow.Remove(res);
            _db.SaveChanges();
            return new NoContentResult();
        }

        [HttpGet("ListCustomerComments")]
        public IActionResult ListCustomerComments()
        {
            try
            {
                var CustomerComments = _db.CustomerComments.ToList();
                return Ok(CustomerComments);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("DeleteCustomerComments/{CommentsId:int}")]
        public IActionResult DeleteCustomerComments(int CommentsId)
        {
            try
            {

                var CustomerComments = _db.CustomerComments
                                            .FirstOrDefault(n => n.CommentsId == CommentsId);

                if (CustomerComments != null)
                {
                    _db.CustomerComments.Remove(CustomerComments);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("UpdateCustomerComments/{CommentsId:int}")]
        public IActionResult UpdateCustomerComments(int CommentsId, [FromBody] CustomerComments CustomerComments)
        {
            try
            {

                var existingNotification = _db.CustomerComments.
                                      FirstOrDefault(n => n.CommentsId == CommentsId);

                if (existingNotification != null)

                {
                    existingNotification.CustomerId = CustomerComments.CustomerId;
                    existingNotification.CommentsText = CustomerComments.CommentsText;
                    existingNotification.InformationRead = CustomerComments.InformationRead;
                    existingNotification.InformationDeleted = CustomerComments.InformationDeleted;
                    existingNotification.CreateDate = CustomerComments.CreateDate;
                    existingNotification.CreateUser = CustomerComments.CreateUser;
                    existingNotification.UpdateDate = CustomerComments.UpdateDate;
                    existingNotification.UpdateUser = CustomerComments.UpdateUser;
                   

                    _db.SaveChanges();
                    return Ok(existingNotification);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
//=====================================================================================================================

        [HttpPost("CustomerFileTracking")]
        public IActionResult CreateCustomerFileTracking(CustomerFileTracking customer)
        {
            try
            {
                // Exclude the 'FileTrackingId' column when adding a new record to the database
                _db.CustomerFileTracking.Add(new CustomerFileTracking
                {
                    CustomerId = customer.CustomerId,
                    FilingId = customer.FilingId,
                    DueDate = customer.DueDate,
                    Status = customer.Status,
                    CreateDate = customer.CreateDate,
                    CreateUser = customer.CreateUser,
                    UpdateDate = customer.UpdateDate,
                    UpdateUser = customer.UpdateUser
                });

                _db.SaveChanges();

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("CustomerFileTracking")]
        public IActionResult ListCustomerFileTracking()
        {
            try
            {

                var CustomerFileTracking = _db.CustomerFileTracking.ToList();
                return Ok(CustomerFileTracking);


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("CustomerFileTracking/{FileTrackingId:Int}")]
        public IActionResult ViewCustomerFileTracking(int FileTrackingId)
        {
            try
            {

                var CustomerFileTracking = _db.CustomerFileTracking
                                       .FirstOrDefault(F => F.FileTrackingId == FileTrackingId);
                if (CustomerFileTracking != null)
                {
                    return Ok(CustomerFileTracking);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("CustomerFileTracking/{FileTrackingId:Int}")]
        public IActionResult DeleteCustomerFileTracking(int FileTrackingId)
        {
            try
            {

                var CustomerFileTracking = _db.CustomerFileTracking
                                                 .FirstOrDefault(a => a.FileTrackingId == FileTrackingId);

                if (CustomerFileTracking != null)
                {
                    _db.CustomerFileTracking.Remove(CustomerFileTracking);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }

        [HttpPut("CustomerFileTracking/{FileTrackingId:int}")]
        public IActionResult UpdateCustomerFileTracking(int FileTrackingId, [FromBody] CustomerFileTracking CustomerFileTracking)
        {
            try
            {

                var existingNotification = _db.CustomerFileTracking.
                                      FirstOrDefault(n => n.FileTrackingId == FileTrackingId);

                if (existingNotification != null)

                {
                    existingNotification.CustomerId = CustomerFileTracking.CustomerId;
                    existingNotification.FilingId = CustomerFileTracking.FilingId;
                    existingNotification.DueDate = CustomerFileTracking.DueDate;
                    existingNotification.Status = CustomerFileTracking.Status;
                    existingNotification.CreateDate = CustomerFileTracking.CreateDate;
                    existingNotification.CreateUser = CustomerFileTracking.CreateUser;
                    existingNotification.UpdateDate = CustomerFileTracking.UpdateDate;
                    existingNotification.UpdateUser = CustomerFileTracking.UpdateUser;


                    _db.SaveChanges();
                    return Ok(existingNotification);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
//===============================================================================================================

        [HttpPost("CustomerFilingMaster")]
        public IActionResult CreateCustomerFilingMaster(CustomerFilingMaster Customer)
        {
            try
            {
                _db.CustomerFilingMaster.Add(Customer);
                _db.SaveChanges();
                return Ok(Customer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("CustomerFilingMaster")]
        public IActionResult ListCustomerFilingMaster()
        {
            try
            {

                var CustomerFilingMaster = _db.CustomerFilingMaster.ToList();
                return Ok(CustomerFilingMaster);


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("CustomerFilingMaster/{CustomerId:Int}")]
        public IActionResult ViewCustomerFilingMaster(int CustomerId)
        {
            try
            {

                var CustomerFilingMaster = _db.CustomerFilingMaster
                                       .FirstOrDefault(F => F.CustomerId == CustomerId);
                if (CustomerFilingMaster != null)
                {
                    return Ok(CustomerFilingMaster);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("CustomerFilingMaster/{CustomerId:Int}")]
        public IActionResult DeleteCustomerFilingMaster(int CustomerId)
        {
            try
            {

                var CustomerFilingMaster = _db.CustomerFilingMaster
                                                 .FirstOrDefault(a => a.CustomerId == CustomerId);

                if (CustomerFilingMaster != null)
                {
                    _db.CustomerFilingMaster.Remove(CustomerFilingMaster);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }


        [HttpPut("CustomerFilingMaster/{Id:int}")]
        public IActionResult UpdateCustomerFilingMaster(int Id, [FromBody] CustomerFilingMaster CustomerFilingMaster)
        {
            try
            {

                var existingNotification = _db.CustomerFilingMaster.
                                      FirstOrDefault(n => n.Id == Id);

                if (existingNotification != null)

                {
                    existingNotification.CustomerId = CustomerFilingMaster.CustomerId;
                    existingNotification.FilingId = CustomerFilingMaster.FilingId;
                    existingNotification.Notes = CustomerFilingMaster.Notes;
                    existingNotification.CreateDate = CustomerFilingMaster.CreateDate;
                    existingNotification.CreateUser = CustomerFilingMaster.CreateUser;
                    existingNotification.UpdateDate = CustomerFilingMaster.UpdateDate;
                    existingNotification.UpdateUser = CustomerFilingMaster.UpdateUser;


                    _db.SaveChanges();
                    return Ok(existingNotification);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
//===============================================================================================================

        [HttpPost("CustomerFilingMasterDraft")]
        public IActionResult CreateCustomerFilingMasterDraft(CustomerFilingMasterDraft Customer)
        {
            try
            {
                _db.CustomerFilingMasterDraft.Add(Customer);
                _db.SaveChanges();
                return Ok(Customer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("CustomerFilingMasterDraft")]
        public IActionResult ListCustomerFilingMasterDraft()
        {
            try
            {

                var CustomerFilingMasterDraft = _db.CustomerFilingMasterDraft.ToList();
                return Ok(CustomerFilingMasterDraft);


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("CustomerFilingMasterDraft/{DraftId:Int}")]
        public IActionResult ViewCustomerFilingMasterDraft(int DraftId)
        {
            try
            {

                var CustomerFilingMasterDraft = _db.CustomerFilingMasterDraft
                                       .FirstOrDefault(F => F.DraftId == DraftId);
                if (CustomerFilingMasterDraft != null)
                {
                    return Ok(CustomerFilingMasterDraft);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("CustomerFilingMasterDraft/{DraftId:Int}")]
        public IActionResult DeleteCustomerFilingMasterDraft(int DraftId)
        {
            try
            {

                var CustomerFilingMasterDraft = _db.CustomerFilingMasterDraft
                                                 .FirstOrDefault(a => a.DraftId == DraftId);

                if (CustomerFilingMasterDraft != null)
                {
                    _db.CustomerFilingMasterDraft.Remove(CustomerFilingMasterDraft);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }

        [HttpPut("CustomerFilingMasterDraft/{DraftId:int}")]
        public IActionResult UpdateCustomerFilingMasterDraft(int DraftId, [FromBody] CustomerFilingMasterDraft CustomerFilingMasterDraft)
        {
            try
            {

                var existingNotification = _db.CustomerFilingMasterDraft.
                                      FirstOrDefault(n => n.DraftId == DraftId);

                if (existingNotification != null)

                {
                    existingNotification.CustomerId = CustomerFilingMasterDraft.CustomerId;
                    existingNotification.FilingId = CustomerFilingMasterDraft.FilingId;
                    existingNotification.Notes = CustomerFilingMasterDraft.Notes;
                    existingNotification.CreateDate = CustomerFilingMasterDraft.CreateDate;
                    existingNotification.CreateUser = CustomerFilingMasterDraft.CreateUser;
                    existingNotification.UpdateDate = CustomerFilingMasterDraft.UpdateDate;
                    existingNotification.UpdateUser = CustomerFilingMasterDraft.UpdateUser;
                    existingNotification.Status = CustomerFilingMasterDraft.Status;


                    _db.SaveChanges();
                    return Ok(existingNotification);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
 //===============================================================================================================

        [HttpPost("CustomerDraftBusinessCategory")]
        public IActionResult CreateCustomerDraftBusinessCategory(CustomerDraftBusinessCategory Customer)
        {
            try
            {
                _db.CustomerDraftBusinessCategory.Add(Customer);
                _db.SaveChanges();
                return Ok(Customer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("CustomerDraftBusinessCategory")]
        public IActionResult ListCustomerDraftBusinessCategory()
        {
            try
            {

                var CustomerDraftBusinessCategory = _db.CustomerDraftBusinessCategory.ToList();
                return Ok(CustomerDraftBusinessCategory);


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("CustomerDraftBusinessCategory/{Id:Int}")]
        public IActionResult ViewCustomerDraftBusinessCategory(int Id)
        {
            try
            {

                var CustomerDraftBusinessCategory = _db.CustomerDraftBusinessCategory
                                       .FirstOrDefault(F => F.Id == Id);
                if (CustomerDraftBusinessCategory != null)
                {
                    return Ok(CustomerDraftBusinessCategory);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("CustomerDraftBusinessCategory/{Id:Int}")]
        public IActionResult DeleteCustomerDraftBusinessCategory(int Id)
        {
            try
            {

                var CustomerDraftBusinessCategory = _db.CustomerDraftBusinessCategory
                                                 .FirstOrDefault(a => a.Id == Id);

                if (CustomerDraftBusinessCategory != null)
                {
                    _db.CustomerDraftBusinessCategory.Remove(CustomerDraftBusinessCategory);
                    _db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }

        [HttpPut("CustomerDraftBusinessCategory/{Id:int}")]
        public IActionResult UpdateCustomerDraftBusinessCategory(int Id, [FromBody] CustomerDraftBusinessCategory CustomerDraftBusinessCategory)
        {
            try
            {

                var existingNotification = _db.CustomerDraftBusinessCategory.
                                      FirstOrDefault(n => n.Id == Id);

                if (existingNotification != null)

                {
                    existingNotification.DraftId = CustomerDraftBusinessCategory.DraftId;
                    existingNotification.BusinessCategoryId = CustomerDraftBusinessCategory.BusinessCategoryId;
                    existingNotification.State = CustomerDraftBusinessCategory.State;
                   


                    _db.SaveChanges();
                    return Ok(existingNotification);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
