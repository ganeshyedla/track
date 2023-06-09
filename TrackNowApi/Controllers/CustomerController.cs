﻿using Azure;
using Azure.Communication.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Data;
using System.Net.Mail;
using System.Text.Json;
using TrackNowApi.Data;
using TrackNowApi.Model;
using static Azure.Core.HttpHeader;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace TrackNowApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        private readonly string Email_connectionString;

        public CustomerController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            Email_connectionString = configuration.GetConnectionString("COMMUNICATION_SERVICES_CONNECTION_STRING");
        }
        
        [HttpGet("GetConfigValue")]
        public string GetConfigValue(string ConfigItem)
        {
            return ((from var in _db.AppConfiguration where var.ConfigItem == ConfigItem && var.CustomerId == null select var.ConfigItemValue).FirstOrDefault());
        }
        [HttpGet("GetConfigValuebyCustmer")]
        public string GetConfigValuebyCustmer(string ConfigItem, decimal CustomerId)
        {
            return ((from var in _db.AppConfiguration where var.ConfigItem == ConfigItem && var.CustomerId == CustomerId select var.ConfigItemValue).FirstOrDefault());
        }
        [HttpGet("SendMail")]
        public APIStatusJSON SendMail(EmailNotification EmailObj)
        {
            try
            {

                EmailClient emailClient = new EmailClient(Email_connectionString);

                var sender = "DoNotReply@9ff9dc46-4bab-4c9d-b883-e79af66841e3.azurecomm.net";

                var emailRecipients = new EmailRecipients(new List<EmailAddress> { new EmailAddress(EmailObj.EmailTo) },
                                                            EmailObj.EmailCC != null ? new List<EmailAddress> { new EmailAddress(EmailObj.EmailCC) } : null,
                                                            EmailObj.EmailBCC != null ? new List<EmailAddress> { new EmailAddress(EmailObj.EmailBCC) } : null);

                // Define the email details
                var subject = EmailObj.EmailSubject;
                var emailContent = new EmailContent(subject)
                {
                    Html = EmailObj.EmailMessage
                };

                var emailMessage = new EmailMessage(sender, emailRecipients, emailContent);
                //emailMessage.Importance = EmailImportance.Low;

                EmailSendOperation sendEmailResult = emailClient.Send(WaitUntil.Completed, emailMessage);

                //EmailSendResult sendStatus = sendEmailResult.GetRawResponse()

                return new APIStatusJSON
                {
                    Status = "Success",
                };
            }
            catch (Exception ex) { return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message }; }

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
                                               select new {j.Id, j.State, i.BusinessCategoryId,i.BusinessCategoryName }).ToList(),
                           Address = o.Address,
                           TaxNumber = o.TaxNumber,
                           Phone = o.Phone,
                           Email = o.Email,
                           ZipCode = o.ZipCode,
                           Juristiction = o.Juristiction,
                           Notes= o.Notes,
                           City= o.City,
                           State= o.State,
                           ParentCustomer =   (from i in _db.Customer
                                                where i.CustomerId == o.ParentCustomerId
                                                select new { i.CustomerId, i.CustomerName }).FirstOrDefault(),
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
                                               select new { j.Id, j.State, i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                           Address = o.Address,
                           TaxNumber = o.TaxNumber,
                           Phone = o.Phone,
                           Email = o.Email,
                           ZipCode = o.ZipCode,
                           Juristiction = o.Juristiction,
                           Notes = o.Notes,
                           City = o.City,
                           State = o.State,
                           ParentCustomer = (from i in _db.Customer
                                             where i.CustomerId == o.ParentCustomerId
                                             select new { i.CustomerId, i.CustomerName }).FirstOrDefault(),
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
                                               select new { j.Id, j.State,  i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                           Address = o.Address,
                           TaxNumber = o.TaxNumber,
                           Phone = o.Phone,
                           Email = o.Email,
                           ZipCode = o.ZipCode,
                           Juristiction = o.Juristiction,
                           Notes = o.Notes,
                           City = o.City,
                           State = o.State,
                           ParentCustomer = (from i in _db.Customer
                                             where i.CustomerId == o.ParentCustomerId
                                             select new { i.CustomerId, i.CustomerName }).FirstOrDefault(),
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
                           Id = cb.Id, CustomerId=c.CustomerId, CustomerName=c.CustomerName,
                           BusinessCategoryId = i.BusinessCategoryId,
                           BusinessCategoryName = i.BusinessCategoryName,
                           BusinessCategoryDescription = i.BusinessCategoryDescription,
                           State = cb.State
                       })); ;

        }
        
        [HttpDelete("CustomerBusinessCategory/{Id:Int}")]
        public APIStatusJSON CustomerBusinessCategory(int Id)
        {
            try { 
                    CustomerBusinessCategory CustomerBusinessCategory;

                    CustomerBusinessCategory = _db.CustomerBusinessCategory.Where(d => d.Id == Id).FirstOrDefault();

                    if (CustomerBusinessCategory == null)
                    {
                        return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Businesscategory not found" };
                    }
                    else {

                        var AnyExistFilings = (from c in _db.CustomerBusinessCategory
                                               join cf in _db.CustomerFilingMaster on c.CustomerId equals cf.CustomerId
                                               join fb in _db.FilingBusinessCategory on cf.FilingId equals fb.FilingId
                                               where c.BusinessCategoryId == fb.BusinessCategoryId && c.Id == Id
                                               select c).FirstOrDefault();

                        if (AnyExistFilings != null )
                        {
                            return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Businesscategory used in some filings" };
                        }

                        _db.CustomerBusinessCategory.Remove(CustomerBusinessCategory);
                        _db.SaveChanges();
                        return new APIStatusJSON
                        {   Status = "Success"};
                    }
                }
                catch (Exception ex)
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
                }
        }

        [HttpGet("CustomerBusinessCategoryList{CustomerId:Int}")]
        public IActionResult CustomerBusinessCategoryList(int CustomerId)
        {
            return Ok((from c in _db.Customer
                       join cb in _db.CustomerBusinessCategory on c.CustomerId equals cb.CustomerId
                       join i in _db.BusinessCategoryMaster on cb.BusinessCategoryId equals i.BusinessCategoryId
                       where c.CustomerId== CustomerId
                       select new
                       {   Id=cb.Id,
                           CustomerId = c.CustomerId,
                           CustomerName = c.CustomerName,
                           BusinessCategoryId = i.BusinessCategoryId,
                           BusinessCategoryName = i.BusinessCategoryName,
                           BusinessCategoryDescription = i.BusinessCategoryDescription,
                           State = cb.State
                       })); ;

        }
        [HttpGet("CustomerFederalBusinessCategoryList{CustomerId:Int}")]
        public IActionResult CustomerFederalBusinessCategoryList(int CustomerId)
        {
            return Ok((from c in _db.Customer
                       join cb in _db.CustomerBusinessCategory on c.CustomerId equals cb.CustomerId
                       join i in _db.BusinessCategoryMaster on cb.BusinessCategoryId equals i.BusinessCategoryId
                       where c.CustomerId == CustomerId && cb.State == null
                       select new
                       {
                           Id = cb.Id,
                           CustomerId = c.CustomerId,
                           CustomerName = c.CustomerName,
                           BusinessCategoryId = i.BusinessCategoryId,
                           BusinessCategoryName = i.BusinessCategoryName,
                           BusinessCategoryDescription = i.BusinessCategoryDescription,
                           State = cb.State
                       })); ;

        }
        [HttpGet("CustomerStateBusinessCategoryList{CustomerId:Int}")]
        public IActionResult CustomerStateBusinessCategoryList(int CustomerId)
        {
            return Ok((from c in _db.Customer
                       join cb in _db.CustomerBusinessCategory on c.CustomerId equals cb.CustomerId
                       join i in _db.BusinessCategoryMaster on cb.BusinessCategoryId equals i.BusinessCategoryId
                       where c.CustomerId == CustomerId && cb.State != null
                       select new
                       {
                           Id = cb.Id,
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

        [HttpDelete("CustomerDelete/{CustomerId:Int}")]
        public IActionResult CustomerDelete(int CustomerId)
        {
            try
            {
                // Find the customer record to be deleted
                Customer TmpCustomer = _db.Customer.Where(d => d.CustomerId == CustomerId).FirstOrDefault();

                if (TmpCustomer != null)
                {
                    // Check if the customer has any child customers
                    Customer ParentCustomer = _db.Customer.Where(d => d.ParentCustomerId == TmpCustomer.CustomerId).FirstOrDefault();

                    if (ParentCustomer == null)
                    {
                        // Add a record to the CustomerHistory table
                        CustomerHistory customerHistory = new CustomerHistory
                        {
                            Title = TmpCustomer.Title,
                            CustomerId = TmpCustomer.CustomerId,
                            CustomerName = TmpCustomer.CustomerName,
                            Address = TmpCustomer.Address,
                            City = TmpCustomer.City,
                            State = TmpCustomer.State,
                            LocationCode = TmpCustomer.ZipCode,
                            TaxNumber = TmpCustomer.TaxNumber,
                            Phone = TmpCustomer.Phone,
                            Email = TmpCustomer.Email,
                            CreateDate = TmpCustomer.CreateDate,
                            CreateUser = TmpCustomer.CreateUser,
                            UpdateDate = TmpCustomer.UpdateDate,
                            UpdateUser = TmpCustomer.UpdateUser,
                            Juristiction = TmpCustomer.Juristiction,
                            Notes = TmpCustomer.Notes,
                            JSI_POC = TmpCustomer.JSI_POC,
                            Customer_POC = TmpCustomer.Customer_POC,
                            Dboperation = "Delete Customer",
                            Source = "CustomerDelete API"
                        };
                        _db.CustomerHistory.Add(customerHistory);

                        // Delete the customer record from the Customer table
                        _db.Customer.Remove(TmpCustomer);
                        _db.SaveChanges();
                    }
                    else
                    {
                        return NotFound("This Parent Customer has child customers, could not delete it");
                    }
                }
                else
                {
                    return NotFound("No Customer found");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting customer: " + ex.Message);
            }
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
            try
            {
                _db.CustomerHistory.Add(CustomerHistory);
                _db.SaveChanges();
                return Ok(CustomerHistory);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpGet("ViewCustomerHistory/{CustomerId:int}")]
        public IActionResult ViewCustomerHistory(int CustomerId)
        {
            try
            {
                var CustomerHistory = _db.CustomerHistory
                                               .FirstOrDefault(n => n.CustomerId == CustomerId);

                if (CustomerHistory != null)
                {
                    return Ok(CustomerHistory);
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

        [HttpGet("ListCustomerHistory")]
        public IActionResult ListCustomerHistory()
        {
            try
            {
                var CustomerHistory = _db.CustomerHistory.ToList();
                return Ok(CustomerHistory);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpDelete("DeleteCustomerHistory/{CustomerId:int}")]
        public IActionResult DeleteCustomerHistory(int CustomerId)
        {

            try
            {

                var CustomerHistory = _db.CustomerHistory
                                            .FirstOrDefault(n => n.CustomerId == CustomerId);

                if (CustomerHistory != null)
                {
                    _db.CustomerHistory.Remove(CustomerHistory);
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
        [HttpPut("UpdateCustomerHistory/{CustomerId:int}")]
        public IActionResult UpdateCustomerHistory(int CustomerId, [FromBody] CustomerHistory CustomerHistory)
        {
            try
            {

                var existingNotification = _db.CustomerHistory.
                                      FirstOrDefault(n => n.CustomerId == CustomerId);


                if (existingNotification != null)
                {
                    existingNotification.Title = CustomerHistory.Title;
                    existingNotification.historyid = CustomerHistory.historyid;
                    existingNotification.BusinessCategoryId = CustomerHistory.BusinessCategoryId;
                    existingNotification.CustomerName = CustomerHistory.CustomerName;
                    existingNotification.Address = CustomerHistory.Address;
                    existingNotification.City = CustomerHistory.City;
                    existingNotification.State = CustomerHistory.State;
                    existingNotification.LocationCode = CustomerHistory.LocationCode;
                    existingNotification.TaxNumber = CustomerHistory.TaxNumber;
                    existingNotification.Phone = CustomerHistory.Phone;
                    existingNotification.Email = CustomerHistory.Email;
                    existingNotification.CreateDate = CustomerHistory.CreateDate;
                    existingNotification.CreateUser = CustomerHistory.CreateUser;
                    existingNotification.UpdateDate = CustomerHistory.UpdateDate;
                    existingNotification.UpdateUser = CustomerHistory.UpdateUser;
                    existingNotification.Juristiction = CustomerHistory.Juristiction;
                    existingNotification.Dboperation = CustomerHistory.Dboperation;
                    existingNotification.Source = CustomerHistory.Source;
                    existingNotification.Notes = CustomerHistory.Notes;
                    existingNotification.JSI_POC = CustomerHistory.JSI_POC;
                    existingNotification.Customer_POC = CustomerHistory.Customer_POC;

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
                          CustomerName = c.CustomerName,
                          FilingId = i.FilingId,
                          FilingName = b.FilingName,
                          FilingDescription = b.FilingDescription,
                          FilingFrequency = b.FilingFrequency,
                          DueDayofFrequency = b.DueDayofFrequency,
                          StateInfo = b.StateInfo,
                          RuleInfo = b.RuleInfo,
                          Juristiction = b.Juristiction,
                          Required = b.Required,
                          Jsidept = b.Jsidept,
                          JsicontactName = b.JsicontactName,
                          JsiContactEmail = b.JsicontactEmail,
                          BusinessCategory = (from i in _db.BusinessCategoryMaster
                                             join j in _db.FilingBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                             where j.FilingId == b.FilingId
                                             select new { i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                                              });

        }
        [HttpGet("BusinessBasedFilingMasterList")]
        public IActionResult BusinessBasedFilingMasterList(int CustomerId)
        {
            return Ok((from a in (from
                      c in _db.Customer
                      join cb in _db.CustomerBusinessCategory on c.CustomerId equals cb.CustomerId
                      join b in _db.BusinessCategoryMaster on cb.BusinessCategoryId equals b.BusinessCategoryId
                      join fm in _db.FilingBusinessCategory on b.BusinessCategoryId equals fm.BusinessCategoryId
                      join f in _db.FilingMaster on fm.FilingId equals f.FilingId
                      where cb.CustomerId == CustomerId && 
                            ((f.StateInfo==cb.State && f.Juristiction == "State") || (cb.State == null && f.Juristiction== "Federal"))
                      select new
                      {
                          CustomerId = c.CustomerId,
                          CustomerName = c.CustomerName,
                          FilingId = f.FilingId,
                          FilingName = f.FilingName,
                          FilingDescription = f.FilingDescription,
                          FilingFrequency = f.FilingFrequency,
                          DueDayofFrequency = f.DueDayofFrequency,
                          StateInfo = f.StateInfo,
                          RuleInfo = f.RuleInfo,
                          Juristiction = f.Juristiction,
                          Required = f.Required,
                          Jsidept = f.Jsidept,
                          JsicontactName = f.JsicontactName,
                          JsiContactEmail = f.JsicontactEmail
                      }).Distinct().Where(x => !_db.CustomerFilingMaster.Any(c => c.FilingId == x.FilingId && c.CustomerId == x.CustomerId) &&
                                      !_db.CustomerFilingMasterDraft.Any(c => c.FilingId == x.FilingId && c.CustomerId == x.CustomerId && c.Status == "Pending")   
                                )
                        select new
                         {
                             CustomerId = a.CustomerId,
                             CustomerName = a.CustomerName,
                             FilingId = a.FilingId,
                             FilingName = a.FilingName,
                             FilingDescription = a.FilingDescription,
                             FilingFrequency = a.FilingFrequency,
                             DueDayofFrequency = a.DueDayofFrequency,
                             StateInfo = a.StateInfo,
                             RuleInfo = a.RuleInfo,
                             Juristiction = a.Juristiction,
                             Required = a.Required,
                             Jsidept = a.Jsidept,
                             JsicontactName = a.JsicontactName,
                             JsiContactEmail = a.JsiContactEmail,
                             BusinessCategory = (from i in _db.BusinessCategoryMaster
                                                 join j in _db.FilingBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                                 where j.FilingId == a.FilingId
                                                 select new { i.BusinessCategoryId, i.BusinessCategoryName }).ToList()
                         }
                        ));
        }
        [HttpPut("CustomerFilingMasterReject{WorkflowId:Int}")]
        public IActionResult CustomerFilingMasterReject(int WorkflowId, string Userid)
        {

            {
                string StoredProc = "exec CustomerFilingMasterDraftApproveReject " +
                        "@WorkflowId = " + WorkflowId.ToString() + "," +
                        "@LoginUserdID = " + Userid.ToString() + "," +
                        "@ApprovedOrRejected= 'Rejected'";

                //return await _context.output.ToListAsync();
                _db.Database.ExecuteSqlRaw(StoredProc);

                CustomerFilingMasterWorkflow CustomerFilingMasterWorkflow = (from w in _db.CustomerFilingMasterWorkflow
                                                                             where w.WorkflowId == WorkflowId
                                                                             select w).FirstOrDefault();

                CustomerFilingMasterDraft CustomerFilingMasterDraft = (from d in _db.CustomerFilingMasterDraft
                                                                       where d.DraftId == CustomerFilingMasterWorkflow.DraftId
                                                                       select d).FirstOrDefault();

                FilingMaster FilingMaster = _db.FilingMaster.Where(d => d.FilingId == CustomerFilingMasterDraft.FilingId).FirstOrDefault();

                Users RequestorDetails = (from u in _db.Users where u.UserId == CustomerFilingMasterWorkflow.WorkflowInitiatorId select u).FirstOrDefault();

                EmailNotification Mail = new EmailNotification();
                Mail.EmailTo = RequestorDetails.LoginId;
                Mail.EmailSubject = GetConfigValue("Mail_ClientFilingRejectResponse_Subject").Replace("{{FilingName}}", FilingMaster.FilingName);
                Mail.EmailMessage = GetConfigValue("Mail_ClientFilingRejectResponse_Message").Replace("{{FilingName}}", FilingMaster.FilingName)
                    .Replace("{\r\n}", Environment.NewLine).Replace("{{Requestor}}", RequestorDetails.UserName);

                _db.FilingMasterWorkflowNotifications.Add(new FilingMasterWorkflowNotifications
                {
                    WorkflowId = CustomerFilingMasterWorkflow.WorkflowId,
                    NotifiedUserId = CustomerFilingMasterWorkflow.WorkflowInitiatorId,
                    NotificationFrom = "DoNotReply@9ff9dc46-4bab-4c9d-b883-e79af66841e3.azurecomm.net",
                    NotificationTo = RequestorDetails.LoginId,
                    NotificationType = "Email",
                    NotificationSubject = Mail.EmailSubject,
                    NotificationText = Mail.EmailMessage,
                    InformationRead = false,
                    InformationDeleted = false,
                    CreateDate = DateTime.Now,
                    CreateUser = "System"
                });

                APIStatusJSON MailResult = SendMail(Mail);


                return Ok();

            }
        }
        [HttpPut("CustomerFilingMasterApprove{WorkflowId:Int}")]
        public IActionResult CustomerFilingMasterApprove(ulong WorkflowId, string Userid)
        {

            string StoredProc = "exec CustomerFilingMasterDraftApproveReject " +
                    "@WorkflowId = " + WorkflowId.ToString() + "," +
                    "@LoginUserdID = " + Userid.ToString() + "," +
                    "@ApprovedOrRejected= 'Approved'";

            //return await _context.output.ToListAsync();
            _db.Database.ExecuteSqlRaw(StoredProc);

            CustomerFilingMasterWorkflow CustomerFilingMasterWorkflow = (from w in _db.CustomerFilingMasterWorkflow
                                                                         where w.WorkflowId == WorkflowId
                                                                         select w).FirstOrDefault();

            CustomerFilingMasterDraft CustomerFilingMasterDraft = (from d in _db.CustomerFilingMasterDraft
                                                                   where d.DraftId == CustomerFilingMasterWorkflow.DraftId
                                                                   select d).FirstOrDefault();

            FilingMaster FilingMaster = _db.FilingMaster.Where(d => d.FilingId == CustomerFilingMasterDraft.FilingId).FirstOrDefault();

            Users RequestorDetails = (from u in _db.Users where u.UserId == CustomerFilingMasterWorkflow.WorkflowInitiatorId select u).FirstOrDefault();

            EmailNotification Mail = new EmailNotification();
            Mail.EmailTo = RequestorDetails.LoginId;
            Mail.EmailSubject = GetConfigValue("Mail_ClientFilingApproveResponse_Subject").Replace("{{FilingName}}", FilingMaster.FilingName);
            Mail.EmailMessage = GetConfigValue("Mail_ClientFilingApproveResponse_Message").Replace("{{FilingName}}", FilingMaster.FilingName)
                .Replace("{\r\n}", Environment.NewLine).Replace("{{Requestor}}", RequestorDetails.UserName);

            _db.FilingMasterWorkflowNotifications.Add(new FilingMasterWorkflowNotifications
            {
                WorkflowId = CustomerFilingMasterWorkflow.WorkflowId,
                NotifiedUserId = CustomerFilingMasterWorkflow.WorkflowInitiatorId,
                NotificationFrom = "DoNotReply@9ff9dc46-4bab-4c9d-b883-e79af66841e3.azurecomm.net",
                NotificationTo = RequestorDetails.LoginId,
                NotificationType = "Email",
                NotificationSubject = Mail.EmailSubject,
                NotificationText = Mail.EmailMessage,
                InformationRead = false,
                InformationDeleted = false,
                CreateDate = DateTime.Now,
                CreateUser = "System"
            });

            APIStatusJSON MailResult = SendMail(Mail);
            return Ok();

        }

        [HttpGet("CustomerFilingMasterWorkflowList")]
        public IActionResult CustomerFilingMasterWorkflowList()
        {
            return Ok(from o in _db.CustomerFilingMasterDraft
                       join c in _db.Customer on o.CustomerId equals c.CustomerId
                       join w in _db.CustomerFilingMasterWorkflow on o.DraftId equals w.DraftId
                       join f in _db.FilingMaster on o.FilingId equals f.FilingId
                       join s in _db.Users on w.CurrentApproverId equals s.UserId into Appr
                            from m in Appr.DefaultIfEmpty()
                      where w.WorkflowStatus =="Pending"
                      select new
                       {
                           WorkflowId = w.WorkflowId,
                           DraftId = w.DraftId,
                           CustomerId = c.CustomerId,
                           CustomerName = c.CustomerName,
                           FilingId = f.FilingId,
                           FilingName = f.FilingName,
                           FilingDescription = f.FilingDescription,
                           FilingFrequency = f.FilingFrequency,
                           DueDayofFrequency = f.DueDayofFrequency,
                           StateInfo = f.StateInfo,
                           RuleInfo = f.RuleInfo,
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
                           CreateDate = w.CreateDate,
                           CreateUser = w.CreateUser,
                           UpdateDate = w.UpdateDate,
                           UpdateUser = w.UpdateUser,
                           WorkflowStatus = w.WorkflowStatus,
                           ChangesInprogress = f.ChangesInprogress,
                           ApproverName = m.UserName,
                           ApproverId = w.CurrentApproverId,
                           BusinessOperation = o.BusinessOperation,
                           WorkflowNotes = w.Notes
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
                       join s in _db.Users on w.CurrentApproverId equals s.UserId
                       where s.UserId == UserId && w.WorkflowStatus != "Pending"
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
                           RuleInfo = f.RuleInfo,
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
                           CreateDate = w.CreateDate,
                           CreateUser = w.CreateUser,
                           UpdateDate = w.UpdateDate,
                           UpdateUser =w.UpdateUser,
                           WorkflowStatus = w.WorkflowStatus,
                           ChangesInprogress = f.ChangesInprogress,
                           ApproverName = s.UserName,
                           ApproverId = w.CurrentApproverId,
                           BusinessOperation = o.BusinessOperation,
                           WorkflowNotes = w.Notes
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
        [HttpGet("CustomerFilingWorkflowNotificationsList{ReceipientId}")]
        public APIStatusJSON CustomerFilingWorkflowNotificationsListbyReceipientId(int ReceipientId)
        {
            try
            {
                var CustomerFilingWorkflowNotifications = _db.CustomerFilingWorkflowNotifications.Where(x=>x.NotifiedUserId== ReceipientId).ToList();

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingWorkflowNotifications, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        [HttpGet("CustomerFilingWorkflowNotificationsList")]
        public APIStatusJSON CustomerFilingWorkflowNotificationsList()
        {
            try { 
                var CustomerFilingWorkflowNotifications = _db.CustomerFilingWorkflowNotifications.ToList();

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingWorkflowNotifications, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
               
            }
            catch(Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        [HttpGet("ViewCustomerFilingWorkflowNotifications/{NotificationId}")]
        public APIStatusJSON ViewCustomerFilingWorkflowNotifications(int NotificationId)
        {
            try
            {
                var CustomerFilingWorkflowNotifications = _db.CustomerFilingWorkflowNotifications.Where(u=>u.NotificationId==NotificationId).ToList();

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingWorkflowNotifications, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        [HttpDelete("DeleteCustomerFilingWorkflowNotifications/{NotificationId}")]
        public APIStatusJSON DeleteCustomerFilingWorkflowNotifications(int NotificationId)
        {
            try
            {
                CustomerFilingWorkflowNotifications CustomerFilingWorkflowNotifications = 
                    _db.CustomerFilingWorkflowNotifications.Where(u => u.NotificationId == NotificationId).FirstOrDefault();

                _db.CustomerFilingWorkflowNotifications.Remove(CustomerFilingWorkflowNotifications);
                _db.SaveChanges();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingWorkflowNotifications, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
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
        public APIStatusJSON CustomerFilingWorkflowNotifications([FromBody] CustomerFilingWorkflowNotifications item)
        {
            try { 
                _db.CustomerFilingWorkflowNotifications.Add(item);
                _db.SaveChanges();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(item, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch ( Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
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
        [HttpGet("CustomerFilingCommentsByCustomerIdFilingId{FilingId:Int}")]
        public APIStatusJSON CustomerFilingCommentsByCustomerIdFilingId(int CustomerId, int FilingId)
        {
            try {
                var CustomerFilingComments =  from o in _db.CustomerFilingComments
                           where o.FilingId == FilingId && o.CustomerId == CustomerId
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
                           };
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingComments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex) {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
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

        [HttpGet("ViewCustomerFilingAttachments/{AttachmentId:Int}")]
        public APIStatusJSON ViewCustomerFilingAttachments(int AttachmentId)
        {
            try
            {

                var CustomerFilingAttachments = _db.CustomerFilingAttachments
                                       .FirstOrDefault(F => F.AttachmentId == AttachmentId);
                if (CustomerFilingAttachments != null)
                {
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingAttachments, new JsonSerializerOptions
                        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                    };
                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage= "Attachment Not found" };
                }


            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpDelete("DeleteCustomerFilingAttachments/{AttachmentId:Int}")]
        public APIStatusJSON DeleteCustomerFilingAttachments(int AttachmentId)
        {
            try
            {

                var CustomerFilingAttachments = _db.CustomerFilingAttachments
                                                 .FirstOrDefault(a => a.AttachmentId == AttachmentId);

                if (CustomerFilingAttachments != null)
                {
                    _db.CustomerFilingAttachments.Remove(CustomerFilingAttachments);
                    _db.SaveChanges();
                    return new APIStatusJSON
                    {
                        Status = "Success"
                    };
                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Attachment Not found" };
                }
            }
            catch (Exception ex)
            {

                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }

        }

        [HttpPut("UpdateCustomerFilingAttachments")]
        public APIStatusJSON UpdateCustomerFilingAttachments( [FromBody] CustomerFilingAttachments CustomerFilingAttachments)
        {
            try
            {

                var existingNotification = _db.CustomerFilingAttachments.
                                      FirstOrDefault(n => n.AttachmentId == CustomerFilingAttachments.AttachmentId);

                if (existingNotification != null)

                {
                    existingNotification.AttachmentPath = CustomerFilingAttachments.AttachmentPath;
                    existingNotification.FilingId = CustomerFilingAttachments.FilingId;
                    existingNotification.CustomerId = CustomerFilingAttachments.CustomerId;
                    existingNotification.CreateDate = CustomerFilingAttachments.CreateDate;
                    existingNotification.CreateUser = CustomerFilingAttachments.CreateUser;
                    existingNotification.UpdatedDate = CustomerFilingAttachments.UpdatedDate;
                    existingNotification.UpdatedUser = CustomerFilingAttachments.UpdatedUser;

                    _db.SaveChanges();
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingAttachments, new JsonSerializerOptions
                        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                    };
                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Attachment Not found" };
                }

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
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

        [HttpGet("ListCustomerFilingCommentsAttachments/{CommentsId}")]
        public IActionResult ListCustomerFilingCommentsAttachments(int CommentsId)
        {
            try
            {

                var CustomerFilingCommentsAttachments = _db.CustomerFilingCommentsAttachments.Where(u => u.CommentsId == CommentsId).ToList();
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
        public APIStatusJSON CreateCustomerFilingTrackingAttachments(CustomerFilingTrackingAttachments Customer)
        {
            try
            {
                _db.CustomerFilingTrackingAttachments.Add(Customer);
                _db.SaveChanges();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(Customer, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }

        }

        [HttpGet("ListCustomerFilingTrackingAttachments")]
        public APIStatusJSON ListCustomerFilingTrackingAttachments()
        {
            try
            {

                 var CustomerFilingTrackingAttachments = (
                 from o in _db.CustomerFilingTrackingAttachments
                 join ct in _db.CustomerFileTracking on o.FileTrackingId equals ct.FileTrackingId
                 select new
                 {
                     FileTrackingId = ct.FileTrackingId,
                     FileTrackingStatus = ct.Status,
                     FileTrackingDueDate = ct.DueDate,
                     AttachmentId = o.AttachmentId,
                     AttachmentPath = o.AttachmentPath,
                     CreateDate = o.CreateDate,
                     CreateUser = o.CreateUser,
                     UpdatedDate = o.UpdatedDate,
                     UpdatedUser = o.UpdatedUser,
                     FileTrackingCreateDate = ct.CreateDate,
                     FileTrackingCreateUser = ct.CreateUser,
                     FileTrackingUpdateDate = ct.UpdateDate,
                     FileTrackingUpdateUser = ct.UpdateUser
                 });
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingTrackingAttachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpGet("ViewCustomerFilingTrackingAttachments/{FileTrackingId:Int}")]
        public APIStatusJSON ViewCustomerFilingTrackingAttachments(int FileTrackingId)
        {
            try
            {
                var CustomerFilingTrackingAttachments = (
                 from o in _db.CustomerFilingTrackingAttachments
                 join ct in _db.CustomerFileTracking on o.FileTrackingId equals ct.FileTrackingId
                 where ct.FileTrackingId == FileTrackingId
                 select new
                 {
                     FileTrackingId = ct.FileTrackingId,
                     FileTrackingStatus = ct.Status,
                     FileTrackingDueDate = ct.DueDate,
                     AttachmentId = o.AttachmentId,
                     AttachmentPath = o.AttachmentPath,
                     CreateDate = o.CreateDate,
                     CreateUser = o.CreateUser,
                     UpdatedDate = o.UpdatedDate,
                     UpdatedUser = o.UpdatedUser,
                     FileTrackingCreateDate = ct.CreateDate,
                     FileTrackingCreateUser = ct.CreateUser,
                     FileTrackingUpdateDate = ct.UpdateDate,
                     FileTrackingUpdateUser = ct.UpdateUser
                 });
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingTrackingAttachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
          }

        [HttpDelete("DeleteCustomerFilingTrackingAttachments/{AttachmentId:Int}")]
        public APIStatusJSON DeleteCustomerFilingTrackingAttachments(int AttachmentId)
        {
            try
            {

                var CustomerFilingTrackingAttachments = _db.CustomerFilingTrackingAttachments
                                                 .FirstOrDefault(a => a.AttachmentId == AttachmentId);

                if (CustomerFilingTrackingAttachments != null)
                {
                    _db.CustomerFilingTrackingAttachments.Remove(CustomerFilingTrackingAttachments);
                    _db.SaveChanges();
                    return new APIStatusJSON
                    {   Status = "Success"};

                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Attachment Not Found" };
                }
            }
            catch (Exception ex)
            {

                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }

        }

        [HttpPut("UpdateCustomerFilingTrackingAttachments/{AttachmentId:int}")]
        public APIStatusJSON UpdateCustomerFilingTrackingAttachments( [FromBody] CustomerFilingTrackingAttachments CustomerFilingTrackingAttachments)
        {
            try
            {

                var existingNotification = _db.CustomerFilingTrackingAttachments.
                                      FirstOrDefault(n => n.AttachmentId == CustomerFilingTrackingAttachments.AttachmentId);

                if (existingNotification != null)

                {
                    existingNotification.AttachmentPath = CustomerFilingTrackingAttachments.AttachmentPath;
                    existingNotification.CreateDate = CustomerFilingTrackingAttachments.CreateDate;
                    existingNotification.CreateUser = CustomerFilingTrackingAttachments.CreateUser;
                    existingNotification.UpdatedDate = CustomerFilingTrackingAttachments.UpdatedDate;
                    existingNotification.UpdatedUser = CustomerFilingTrackingAttachments.UpdatedUser;
                    _db.SaveChanges();
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingTrackingAttachments, new JsonSerializerOptions
                        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                    };

                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Attachment Not Found" };
                }
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
//=====================================================================================================================
        [HttpDelete("DeleteCustomerFilingTrackingNotification/{NotificationId:decimal}")]
        public APIStatusJSON DeleteCustomerFilingTrackingNotification(decimal NotificationId)
        {
            try
            {
                {
                    var CustomerFilingTrackingNotification = _db.CustomerFilingTrackingNotifications
                                                .FirstOrDefault(n => n.NotificationId == NotificationId);

                    if (CustomerFilingTrackingNotification != null)
                    {
                        _db.CustomerFilingTrackingNotifications.Remove(CustomerFilingTrackingNotification);
                        _db.SaveChanges();
                        return new APIStatusJSON { Status = "Success"};
                    }
                    else
                    {
                        return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Notification Not Found"};
                    }
                }
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }


        [HttpPost("CreateCustomerFilingTrackingNotifications")]
        public APIStatusJSON CreateCustomerFilingTrackingNotifications(CustomerFilingTrackingNotifications notification)
        {
            try
            {
                _db.CustomerFilingTrackingNotifications.Add(notification);
                _db.SaveChanges();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(notification, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
                
            }
        }


        [HttpGet("ViewCustomerFilingTrackingNotification/{NotificationId:decimal}")]
        public APIStatusJSON ViewCustomerFilingTrackingNotification(decimal NotificationId)
        {
            try
            {
                var CustomerFilingTrackingNotification = _db.CustomerFilingTrackingNotifications
                                                        .FirstOrDefault(n => n.NotificationId == NotificationId);

                if (CustomerFilingTrackingNotification != null)
                {
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingTrackingNotification, new JsonSerializerOptions
                        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                    };
                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Notification Not Found" };
                }
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }



        [HttpGet("ListCustomerFilingTrackingNotifications")]
        public APIStatusJSON ListCustomerFilingTrackingNotifications()
        {
            try
            {
                var CustomerFilingTrackingNotifications = _db.CustomerFilingTrackingNotifications.ToList();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingTrackingNotifications, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpGet("ListCustomerFilingTrackingNotifications{ReceipientId}")]
        public APIStatusJSON ListCustomerFilingTrackingNotificationsbyReceipientId(int ReceipientId)
        {
            try
            {
                var CustomerFilingTrackingNotifications = _db.CustomerFilingTrackingNotifications.Where(x => x.NotifiedUserId == ReceipientId).ToList();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingTrackingNotifications, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        [HttpPut("UpdateCustomerFilingTrackingNotification/{NotificationId:decimal}")]
        public APIStatusJSON UpdateCustomerFilingTrackingNotification(decimal NotificationId, [FromBody] CustomerFilingTrackingNotifications customerFilingTrackingNotification)
        {
            try
            {
                var existingNotification = _db.CustomerFilingTrackingNotifications.
                                        FirstOrDefault(n => n.NotificationId == NotificationId);

                if (existingNotification != null)

                {
                    existingNotification.FileTrackingId = customerFilingTrackingNotification.FileTrackingId;
                    existingNotification.NotificationFrom = customerFilingTrackingNotification.NotificationFrom;
                    existingNotification.NotificationTo = customerFilingTrackingNotification.NotificationTo;
                    existingNotification.NotificationCC = customerFilingTrackingNotification.NotificationCC;
                    existingNotification.NotificationSubject = customerFilingTrackingNotification.NotificationSubject;
                    existingNotification.NotificationType = customerFilingTrackingNotification.NotificationType;
                    existingNotification.NotificationText = customerFilingTrackingNotification.NotificationText;
                    existingNotification.InformationRead = customerFilingTrackingNotification.InformationRead;
                    existingNotification.InformationDeleted = customerFilingTrackingNotification.InformationDeleted;
                    existingNotification.CreateDate = customerFilingTrackingNotification.CreateDate;
                    existingNotification.CreateUser = customerFilingTrackingNotification.CreateUser;

                    _db.SaveChanges();
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(existingNotification, new JsonSerializerOptions
                        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                    };

                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Notification Not Found" };

                }
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
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


        [HttpGet("CustomerFilingMasterHistoryList/{CustomerId:int}/{FilingId:int}")]
        public IActionResult CustomerFilingMasterHistoryList(int CustomerId, int FilingId)
        {
            var CustomerFilingMasterHistory = (from cfh in _db.CustomerFilingMasterHistory
                       join c in _db.Customer on cfh.CustomerId equals c.CustomerId
                       join cf in _db.CustomerFilingMaster on cfh.CustomerId equals cf.CustomerId
                       where c.CustomerId == CustomerId && cf.FilingId == FilingId
                       select new
                       {
                           historyid = cfh.historyid,
                           CustomerId = c.CustomerId,
                           FilingId = cf.FilingId,
                           Notes = cfh.Notes,
                           Dboperations = cfh.Dboperations,
                           Source = cfh.Source,
                           CreateDate = cfh.CreateDate,
                           CreateUser = cfh.CreateUser,
                           UpdateDate = cfh.UpdateDate,
                           UpdateUser = cfh.UpdateUser
                       });

            return Ok(CustomerFilingMasterHistory);
        }

        [HttpPost("CustomerFilingMasterHistoryCreate")]
        public IActionResult CustomerFilingMasterHistoryCreate([FromBody] CustomerFilingMasterHistory item)
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

        //[HttpGet("CustomerFilingMasterWorkflowLists")]
        //public IActionResult CustomerFilingMasterWorkflowLists()
        //{
        //    var res = _db.CustomerFilingMasterWorkflow.ToList();
        //    return Ok(res);
        //}

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
            try
            {
                var CustomerFilingMasterWorkflow = (from o in _db.CustomerFilingMasterDraft
                           join c in _db.Customer on o.CustomerId equals c.CustomerId
                           join w in _db.CustomerFilingMasterWorkflow on o.DraftId equals w.DraftId
                           join f in _db.FilingMaster on o.FilingId equals f.FilingId
                           where w.WorkflowId == WorkflowId
                           select new
                           {
                               WorkflowId = w.WorkflowId,
                               WorkflowInitiatorId = w.WorkflowInitiatorId,
                               CurrentApproverId = w.CurrentApproverId,
                               DraftId = w.DraftId,
                               WorkflowStatus = w.WorkflowStatus,
                               FilingId = o.FilingId,
                               CustomerId = o.CustomerId,
                               FilingName = f.FilingName,
                               CustomerName = c.CustomerName,
                               CreateDate = w.CreateDate,
                               CreateUser = w.CreateUser,
                               UpdateDate = w.UpdateDate,
                               UpdateUser = w.UpdateUser,
                           }).ToList();


                        if (CustomerFilingMasterWorkflow.Count == 0) // if no records found
                        {
                            return NotFound("No Workflow records found.");
                        }

                return Ok(CustomerFilingMasterWorkflow);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            //    var res = _db.CustomerFilingMasterWorkflow.FirstOrDefault(p => p.WorkflowId == WorkflowId);
            //    if (res != null)
            //    {
            //        return Ok(res);
            //    }
            //    else
            //    {
            //        return NotFound();
            //    }
        }
        [HttpPut("CustomerFilingMasterWorkflowupdate/{WorkflowId}")]
        public IActionResult CustomerFilingMasterWorkflowUpdate(int WorkflowId, CustomerFilingMasterWorkflow updatedCustomer)
        {
            var customer = _db.CustomerFilingMasterWorkflow.FirstOrDefault(p => p.WorkflowId == WorkflowId);
            if (customer == null)
            {
                return NotFound();
            }

            _db.Entry(customer).CurrentValues.SetValues(updatedCustomer);
           // _db.Update(updatedCustomer);
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
        public APIStatusJSON CreateCustomerFileTracking(CustomerFileTracking customer)
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
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(customer, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpGet("CustomerFileTrackingbyCustomerId")]
        public APIStatusJSON CustomerFileTrackingbyCustomerId(ulong CustomerId)
        {
            try
            {
                var Filetrackinglist = (
                from c in _db.CustomerFileTracking
                join f in _db.FilingMaster on c.FilingId equals f.FilingId
                join o in _db.Customer on c.CustomerId equals o.CustomerId
                where o.CustomerId == CustomerId
                select new
                {
                    FileTrackingId = c.FileTrackingId,
                    CustomerId = c.CustomerId,
                    FilingId = c.FilingId,
                    DueDate = c.DueDate,
                    Status = c.Status,
                    CreateDate = c.CreateDate,
                    CreateUser = c.CreateUser,
                    UpdateDate = c.UpdateDate,
                    UpdateUser = c.UpdateUser,
                    CustomerName = o.CustomerName,
                    FilingName = f.FilingName,
                    FilingDescription = f.FilingDescription,
                    FilingFrequency = f.FilingFrequency,
                    FilingJuristiction = f.Juristiction,
                    FilingRequired = f.Required,
                    FilingStateInfo = f.StateInfo,
                    RuleInfo = f.RuleInfo,
                    BusinessCategory = (from i in _db.BusinessCategoryMaster
                                        join j in _db.FilingBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                        where j.FilingId == f.FilingId
                                        select new { j.Id, j.State, i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                    Jsidept = f.Jsidept,
                    JsicontactName = f.JsicontactName,
                    JsicontactEmail = f.JsicontactEmail
                }
                );
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(Filetrackinglist, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpGet("CustomerFileTracking")]
        public APIStatusJSON ListCustomerFileTracking()
        {
            try
            {
                var Filetrackinglist = (
                from c in _db.CustomerFileTracking
                join f in _db.FilingMaster on c.FilingId equals f.FilingId
                join o in _db.Customer on c.CustomerId equals o.CustomerId
                select new
                {
                    FileTrackingId = c.FileTrackingId,
                    CustomerId = c.CustomerId,
                    FilingId = c.FilingId,
                    DueDate = c.DueDate,
                    Status = c.Status,
                    CreateDate = c.CreateDate,
                    CreateUser = c.CreateUser,
                    UpdateDate = c.UpdateDate,
                    UpdateUser = c.UpdateUser,
                    CustomerName = o.CustomerName,
                    FilingName = f.FilingName,
                    FilingDescription = f.FilingDescription,
                    FilingFrequency = f.FilingFrequency,
                    FilingJuristiction = f.Juristiction,
                    FilingRequired = f.Required,
                    FilingStateInfo = f.StateInfo,
                    RuleInfo = f.RuleInfo,
                    BusinessCategory = (from i in _db.BusinessCategoryMaster
                                        join j in _db.FilingBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                        where j.FilingId == f.FilingId
                                        select new { j.Id, j.State, i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                    Jsidept = f.Jsidept,
                    JsicontactName = f.JsicontactName,
                    JsicontactEmail = f.JsicontactEmail
                }
                );
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(Filetrackinglist, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpGet("CustomerFileTracking/{FileTrackingId:Int}")]
        public APIStatusJSON ViewCustomerFileTracking(int FileTrackingId)
        {
            try
            {

                var CustomerFileTracking = (
                               from c in _db.CustomerFileTracking
                               join f in _db.FilingMaster on c.FilingId equals f.FilingId
                               join o in _db.Customer on c.CustomerId equals o.CustomerId
                               where c.FileTrackingId == FileTrackingId
                               select new
                               {
                                   FileTrackingId = c.FileTrackingId,
                                   CustomerId = c.CustomerId,
                                   CustomerName = o.CustomerName,
                                   FilingId = c.FilingId,
                                   FilingName = f.FilingName,
                                   FilingFrequency = f.FilingFrequency,
                                   DueDate = c.DueDate,
                                   Status = c.Status,
                                   CreateDate = c.CreateDate,
                                   CreateUser = c.CreateUser,
                                   UpdateDate = c.UpdateDate,
                                   UpdateUser = c.UpdateUser,

                               });

                if (CustomerFileTracking == null)
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "File Tracking Not Found" };

                }
                else
                {
   
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFileTracking, new JsonSerializerOptions
                        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }

            //   var CustomerFileTracking = _db.CustomerFileTracking
            //                       .FirstOrDefault(F => F.FileTrackingId == FileTrackingId);
            //if (CustomerFileTracking != null)
            //{

            //    return new APIStatusJSON
            //    {
            //        Status = "Success",
            //        Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFileTracking, new JsonSerializerOptions
            //        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
            //    };

            //}

        }

        [HttpDelete("CustomerFileTracking/{FileTrackingId:Int}")]
        public APIStatusJSON DeleteCustomerFileTracking(int FileTrackingId)
        {
            try
            {

                var CustomerFileTracking = _db.CustomerFileTracking
                                                 .FirstOrDefault(a => a.FileTrackingId == FileTrackingId);

                if (CustomerFileTracking != null)
                {
                    _db.CustomerFileTracking.Remove(CustomerFileTracking);
                    _db.SaveChanges();
                    return new APIStatusJSON { Status = "Success" };

                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "File Tracking Not Found" };
                }
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }

        }

        [HttpPut("CustomerFileTracking/{FileTrackingId:int}")]
        public APIStatusJSON UpdateCustomerFileTracking(int FileTrackingId, [FromBody] CustomerFileTracking CustomerFileTracking)
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
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFileTracking, new JsonSerializerOptions
                        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                    };
                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Notification Not Found" };
                }

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
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
                var CustomerFilingMaster = _db.CustomerFilingMaster.FirstOrDefault(a => a.CustomerId == CustomerId);

                if (CustomerFilingMaster != null)
                {
                    // Add a record to CustomerFilingMasterHistory for auditing purposes
                    var CustomerFilingMasterHistory = new CustomerFilingMasterHistory
                    {
                        //historyid = CustomerFilingMaster.Id,
                        CustomerId = CustomerFilingMaster.CustomerId,
                        FilingId = CustomerFilingMaster.FilingId,
                        Notes = CustomerFilingMaster.Notes,
                        Dboperations = "Delete CustomerFilingMaster",
                        Source = "DeleteCustomerFilingMaster API",
                        CreateDate = CustomerFilingMaster.CreateDate,
                        CreateUser = CustomerFilingMaster.CreateUser,
                        UpdateDate = CustomerFilingMaster.UpdateDate,
                        UpdateUser = CustomerFilingMaster.UpdateUser
                    };
                    _db.CustomerFilingMasterHistory.Add(CustomerFilingMasterHistory);

                    // Delete the record from CustomerFilingMaster
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
                if (CustomerFilingMaster == null || CustomerFilingMaster.Id != Id)
                {
                    return BadRequest(ModelState);
                }

                CustomerFilingMasterHistory CustomerFilingMasterHistory = new CustomerFilingMasterHistory();
               // CustomerFilingMasterHistory.historyid = Id;
                CustomerFilingMasterHistory.CustomerId = CustomerFilingMaster.CustomerId;
                CustomerFilingMasterHistory.FilingId = CustomerFilingMaster.FilingId;
                CustomerFilingMasterHistory.Notes = CustomerFilingMaster.Notes;
                CustomerFilingMasterHistory.UpdateDate = CustomerFilingMaster.UpdateDate;
                CustomerFilingMasterHistory.UpdateUser = CustomerFilingMaster.UpdateUser;
                CustomerFilingMasterHistory.CreateDate = CustomerFilingMaster.CreateDate;
                CustomerFilingMasterHistory.CreateUser = CustomerFilingMaster.CreateUser;
                CustomerFilingMasterHistory.Dboperations = "Update CustomerFilingMaster";
                CustomerFilingMasterHistory.Source = "UpdateCustomerFilingMaster API";

                _db.Update(CustomerFilingMaster);
                CustomerFilingMasterHistoryCreate(CustomerFilingMasterHistory);
                _db.SaveChanges();

                return Ok(CustomerFilingMaster);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("CustomerFilingTrackerGenerator")]
        public APIStatusJSON CustomerFilingTrackerGenerator(string Frequency)
        {
            try
            {
                var FilingTracker = _db.CustomerFileTracking.FromSqlRaw("dbo.CustomerFileTrackGenerator {0}", Frequency).ToList();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingTracker, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON
                {
                    Status = "Failure",
                    ErrorCode = 1,
                    ErrorMessage = ex.Message
                };

            }
        }
        [HttpGet("CustomerFilingTrackerNotifications")]
        public APIStatusJSON CustomerFilingTrackerNotifications()
        {
            try
            {
                var FileTracker = _db.CustomerFileTracking.FromSqlRaw("dbo.CustomerFileTrackMailNotifications").ToList();


                //var FileTracker = (from i in _db.CustomerFileTracking
                //                   join j in _db.FilingMaster on i.FilingId equals j.FilingId
                //                   where ((DateTime)i.DueDate).AddDays((double)j.DueDayofFrequency) == DateTime.Today
                //                   select i).ToList();

                foreach (CustomerFileTracking ft in FileTracker)
                {

                    Customer Customer = _db.Customer.Where(d => d.CustomerId == ft.CustomerId).FirstOrDefault();

                    FilingMaster FilingMaster = _db.FilingMaster.Where(d => d.FilingId == ft.FilingId).FirstOrDefault();

                    Users ReceiverDetails = (from u in _db.Users where u.CustomerId == Customer.CustomerId select u).FirstOrDefault();

                    EmailNotification Mail = new EmailNotification();
                    Mail.EmailTo = ReceiverDetails.LoginId;

                    if (GetConfigValuebyCustmer("Mail_FilingTrackerReminder_Subject", Convert.ToDecimal(ft.CustomerId)) != null)
                        Mail.EmailSubject = GetConfigValuebyCustmer("Mail_FilingTrackerReminder_Subject", Convert.ToDecimal(ft.CustomerId));
                    else
                        Mail.EmailSubject = GetConfigValue("Mail_FilingTrackerReminder_Subject");

                    if (GetConfigValuebyCustmer("Mail_FilingTrackerReminder_Subject", Convert.ToDecimal(ft.CustomerId)) != null)
                        Mail.EmailMessage = GetConfigValuebyCustmer("Mail_FilingTrackerReminder_Message", Convert.ToDecimal(ft.CustomerId));
                    else
                        Mail.EmailMessage = GetConfigValue("Mail_FilingTrackerReminder_Message");

                    Mail.EmailSubject = Mail.EmailSubject.Replace("{{FilingName}}", FilingMaster.FilingName);
                    Mail.EmailMessage = Mail.EmailMessage.Replace("{{FilingName}}", FilingMaster.FilingName)
                        .Replace("{{Customer}}", ReceiverDetails.UserName).Replace("{{DueDate}}", ft.DueDate.ToString());

                    _db.CustomerFilingTrackingNotifications.Add(new CustomerFilingTrackingNotifications
                    {
                        NotifiedUserId = ReceiverDetails.UserId,
                        NotificationFrom = "DoNotReply@9ff9dc46-4bab-4c9d-b883-e79af66841e3.azurecomm.net",
                        NotificationTo = ReceiverDetails.LoginId,
                        NotificationType = "Email",
                        NotificationSubject = Mail.EmailSubject,
                        NotificationText = Mail.EmailMessage,
                        InformationRead = false,
                        InformationDeleted = false,
                        CreateDate = DateTime.Now,
                        CreateUser = "System"
                    });

                    APIStatusJSON MailResult = SendMail(Mail);
                }
                return new APIStatusJSON
                {
                    Status = "Success"
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };

            }
        }
        [HttpPost("CustomerFilingMasterDraft")]
        public APIStatusJSON CreateCustomerFilingMasterDraft(CustomerFilingMasterDraft[] Customer, string @LoggedInUser)
        {
            try { 
                var opt = new JsonSerializerOptions() { WriteIndented = true };
                string Customerrecords = JsonSerializer.Serialize<CustomerFilingMasterDraft[]>(Customer, opt);

                var CustomerUpdate = _db.CustomerFilingMasterDraft.FromSqlRaw("dbo.CustomerFilingMasterDraftUpdate {0},{1}", Customerrecords, @LoggedInUser).ToList();

                foreach (CustomerFilingMasterDraft Cfm in CustomerUpdate)
                {

                    FilingMaster FilingMaster = _db.FilingMaster.Where(d => d.FilingId == Cfm.FilingId).FirstOrDefault();

                    CustomerFilingMasterWorkflow CustomerFilingMasterWorkflow = (from w in _db.CustomerFilingMasterWorkflow
                                                                                 where w.DraftId == Cfm.DraftId
                                                                                 select w).FirstOrDefault();

                    Users RequestorDetails = (from u in _db.Users where u.UserId == CustomerFilingMasterWorkflow.WorkflowInitiatorId select u).FirstOrDefault();
                    Users ApproverDetails = (from u in _db.Users where u.UserId == CustomerFilingMasterWorkflow.CurrentApproverId select u).FirstOrDefault();

                    EmailNotification Mail = new EmailNotification();
                    Mail.EmailTo = RequestorDetails.LoginId;
                    Mail.EmailSubject = GetConfigValue("Mail_ClientFilingApproveRequest_Subject").Replace("{{FilingName}}", FilingMaster.FilingName);
                    Mail.EmailMessage = GetConfigValue("Mail_ClientFilingApproveRequest_Message").Replace("{{FilingName}}", FilingMaster.FilingName)
                        .Replace("{\r\n}", Environment.NewLine).Replace("{{Requestor}}", RequestorDetails.UserName).Replace("{{ApproverName}}", ApproverDetails.UserName);

                    _db.FilingMasterWorkflowNotifications.Add(new FilingMasterWorkflowNotifications
                    {
                        WorkflowId = CustomerFilingMasterWorkflow.WorkflowId,
                        NotifiedUserId = CustomerFilingMasterWorkflow.WorkflowInitiatorId,
                        NotificationFrom = "DoNotReply@9ff9dc46-4bab-4c9d-b883-e79af66841e3.azurecomm.net",
                        NotificationTo = RequestorDetails.LoginId,
                        NotificationType = "Email",
                        NotificationSubject = Mail.EmailSubject,
                        NotificationText = Mail.EmailMessage,
                        InformationRead = false,
                        InformationDeleted = false,
                        CreateDate = DateTime.Now,
                        CreateUser = "System"
                    });

                    APIStatusJSON MailResult = SendMail(Mail);
                }
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerUpdate, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
             {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };

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
        [HttpGet("customerfilingmasterworkflowattachments/{workflowid:decimal}")]
        public APIStatusJSON listcustomerfilingmasterworkflowattachments(decimal workflowid)
        {
            try
            {
                var customerfilingmasterworkflowattachments = (from cfw in _db.CustomerFilingMasterWorkflow
                                                               join cfwa in _db.CustomerFilingMasterWorkflowAttachments on cfw.WorkflowId equals cfwa.WorkFlowId
                                                               where cfw.WorkflowId == workflowid
                                                               select new
                                                               {
                                                                   workflowid = cfw.WorkflowId,
                                                                   WorkflowStatus = cfw.WorkflowStatus,
                                                                   attachmentid = cfwa.AttachmentId,
                                                                   attachmentpath = cfwa.AttachmentPath,
                                                                   createdate = cfwa.CreateDate,
                                                                   createuser = cfwa.CreateUser,
                                                                   updateddate = cfwa.UpdatedDate,
                                                                   updateduser = cfwa.UpdatedUser
                                                               });

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(customerfilingmasterworkflowattachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
                ;
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }

        }

        [HttpGet("CustomerFilingWorkflowCommentsAttachments/{WorkflowId:decimal}/{CommentsId:decimal}")]
        public APIStatusJSON ListCustomerFilingWorkflowCommentsAttachments(decimal WorkflowId, decimal CommentsId)
        {
            try
            {
                var CustomerFilingWorkflowCommentsAttachments = (from cfw in _db.CustomerFilingMasterWorkflow
                                                                 join cc in _db.CustomerComments on cfw.WorkflowId equals cc.CustomerId
                                                                 join cfwa in _db.CustomerFilingWorkflowCommentsAttachments on cc.CommentsId equals cfwa.CommentsId
                                                                 where cfw.WorkflowId == WorkflowId && cc.CommentsId == CommentsId
                                                                 select new
                                                                 {
                                                                     WorkflowId = cfw.WorkflowId,
                                                                     WorkflowStatus = cfw.WorkflowStatus,
                                                                     CommentsId = cc.CommentsId,
                                                                     AttachmentId = cfwa.AttachmentId,
                                                                     AttachmentPath = cfwa.AttachmentPath,
                                                                     CreateDate = cfwa.CreateDate,
                                                                     CreateUser = cfwa.CreateUser,
                                                                     UpdatedDate = cfwa.UpdatedDate,
                                                                     UpdatedUser = cfwa.UpdatedUser
                                                                 });

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingWorkflowCommentsAttachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        
        [HttpGet("CustomerAttachments/{CustomerId:decimal}")]
        public APIStatusJSON ListCustomerAttachments(decimal CustomerId)
        {
            try
            {
                var CustomerAttachments = from c in _db.Customer
                                          join ca in _db.CustomerAttachments on c.CustomerId equals ca.CustomerId
                                          where c.CustomerId == CustomerId
                                          select new
                                          {
                                              CustomerId = c.CustomerId,
                                              CustomerName = c.CustomerName,
                                              AttachmentId = ca.AttachmentId,
                                              AttachmentPath = ca.AttachmentPath,
                                              CreateDate = ca.CreateDate,
                                              CreateUser = ca.CreateUser,
                                              UpdatedDate = ca.UpdatedDate,
                                              UpdatedUser = ca.UpdatedUser
                                          };
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerAttachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
                ;
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }

        }

        [HttpGet("ListCustomerFilingCommentsAttachments/{customerId}/{filingId}")]
        public APIStatusJSON ListCustomerFilingCommentsAttachments(decimal customerId, decimal filingId, decimal? commentsId = null)
        {
            try
            {
                var res = from c in _db.Customer
                          join cf in _db.CustomerFilingMaster on c.CustomerId equals cf.CustomerId
                          join cc in _db.CustomerFilingComments on cf.CustomerId equals cc.CustomerId
                          join f in _db.FilingMaster on cf.FilingId equals f.FilingId
                          join ccc in _db.CustomerFilingCommentsAttachments on cc.CommentsId equals ccc.CommentsId into ccfa
                          from cfa in ccfa.DefaultIfEmpty()
                          where c.CustomerId == customerId && cf.FilingId == filingId && cc.CommentsId == (commentsId.HasValue ? commentsId : cc.CommentsId)

                          select new
                          {
                              CustomerId = c.CustomerId,
                              CustomerName = c.CustomerName,
                              FilingId = cf.FilingId,
                              FilingName = f.FilingName,
                              CommentsId = cc.CommentsId,
                              CommentsText = cc.CommentsText,
                              AttachmentId = cfa.AttachmentId == null ? 0 : cfa.AttachmentId,
                              AttachmentPath = cfa.AttachmentPath,
                              CreateDate = cfa.CreateDate,
                              CreateUser = cfa.CreateUser,
                              UpdatedDate = cfa.UpdatedDate,
                              UpdatedUser = cfa.UpdatedUser
                          };

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(res, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
//===========================================================================================================
        [HttpGet("CustomerFilingDraftAttachments/{DraftId:decimal}")]
        public APIStatusJSON ListCustomerFilingDraftAttachments(decimal DraftId)
        {
            try
            {
                var CustomerFilingDraftAttachments = (from d in _db.CustomerFilingMasterDraft
                                                      join a in _db.CustomerFilingDraftAttachments on d.DraftId equals a.DraftId
                                                      where d.DraftId == DraftId
                                                      select new
                                                      {
                                                          DraftId = d.DraftId,
                                                          AttachmentId = a.AttachmentId,
                                                          AttachmentPath = a.AttachmentPath,
                                                          CreateDate = a.CreateDate,
                                                          CreateUser = a.CreateUser,
                                                          UpdatedDate = a.UpdatedDate,
                                                          UpdatedUser = a.UpdatedUser
                                                      });

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingDraftAttachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
                ;
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        //==========================================================================================================

        [HttpGet("CustomerFilingDraftCommentsAttachments/{DraftId:decimal}/{CommentsId:decimal}")]
        public APIStatusJSON ListDraftCommentsAttachments(decimal DraftId, decimal CommentsId)
        {
            try
            {
                var CustomerFilingDraftCommentsAttachments = (from d in _db.CustomerFilingMasterDraft
                                                              join c in _db.CustomerComments on d.CustomerId equals c.CustomerId
                                                              join a in _db.CustomerFilingDraftCommentsAttachments on c.CommentsId equals a.CommentsId
                                                              where d.DraftId == DraftId && c.CommentsId == CommentsId
                                                              select new
                                                              {
                                                                  DraftId = d.DraftId,
                                                                  AttachmentId = a.AttachmentId,
                                                                  AttachmentPath = a.AttachmentPath,
                                                                  CommentsId = a.CommentsId,
                                                                  CommentsText = c.CommentsText,
                                                                  CreateDate = a.CreateDate,
                                                                  CreateUser = a.CreateUser,
                                                                  UpdatedDate = a.UpdatedDate,
                                                                  UpdatedUser = a.UpdatedUser
                                                              });

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingDraftCommentsAttachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }


        //===========================================================================================================

        [HttpGet("CustomerFilingTrackingAttachments")]
        public APIStatusJSON ListCustomerFilingTrackingAttachments(decimal customerId, decimal? fileTrackingId)
        {
            try
            {
                var CustomerFilingTrackingAttachments = from c in _db.Customer
                                                        join ft in _db.CustomerFileTracking on c.CustomerId equals ft.CustomerId
                                                        join fa in _db.CustomerFilingTrackingAttachments on ft.FileTrackingId equals fa.FileTrackingId
                                                        where c.CustomerId == customerId && (!fileTrackingId.HasValue || ft.FileTrackingId == fileTrackingId)
                                                        select new
                                                        {
                                                            CustomerId = c.CustomerId,
                                                            CustomerName = c.CustomerName,
                                                            FileTrackingId = ft.FileTrackingId,
                                                            AttachmentId = fa.AttachmentId,
                                                            AttachmentPath = fa.AttachmentPath,
                                                            CreateDate = fa.CreateDate,
                                                            CreateUser = fa.CreateUser,
                                                            UpdatedDate = fa.UpdatedDate,
                                                            UpdatedUser = fa.UpdatedUser

                                                        };
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingTrackingAttachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
                ;
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        //=============================================================================================================
        [HttpGet("CustomerFilingTrackingCommentsAttachments")]
        public APIStatusJSON ListCustomerFilingTrackingCommentsAttachments(decimal customerId, decimal? fileTrackingId, decimal? commentsId)
        {
            try
            {
                var CustomerFilingTrackingCommentsAttachments = from c in _db.Customer
                                                                join ft in _db.CustomerFileTracking on c.CustomerId equals ft.CustomerId
                                                                join fc in _db.CustomerComments on c.CustomerId equals fc.CustomerId into ftc
                                                                from fc in ftc.DefaultIfEmpty()
                                                                join fa in _db.CustomerFilingTrackingCommentsAttachments on fc.CommentsId equals fa.CommentsId into fcaf
                                                                from fa in fcaf.DefaultIfEmpty()
                                                                where c.CustomerId == customerId && (!fileTrackingId.HasValue || ft.FileTrackingId == fileTrackingId) && (!commentsId.HasValue || fc.CommentsId == commentsId)
                                                                select new
                                                                {
                                                                    CustomerId = c.CustomerId,
                                                                    CustomerName = c.CustomerName,
                                                                    FileTrackingId = ft.FileTrackingId,
                                                                    CommentsId = fc.CommentsId,
                                                                    CommentsText = fc.CommentsText,
                                                                    CreateDate = fa.CreateDate,
                                                                    CreateUser = fa.CreateUser,
                                                                    UpdatedDate = fa.UpdatedDate,
                                                                    UpdatedUser = fa.UpdatedUser,
                                                                    AttachmentId = fa.AttachmentId,
                                                                    AttachmentPath = fa.AttachmentPath
                                                                };

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(CustomerFilingTrackingCommentsAttachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

    }
}
