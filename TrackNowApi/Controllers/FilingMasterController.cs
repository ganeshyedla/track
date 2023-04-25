using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Net.Mail;
using System.Text.Json;
using TrackNowApi.Data;
using TrackNowApi.Model;

namespace TrackNowApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FilingMasterController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public FilingMasterController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpPost("CreateFilingMaster")]
        public IActionResult CreateFilingMaster(FilingMaster FilingMaster)
        {

            _db.Add(FilingMaster);
            _db.SaveChanges();

            return Ok(FilingMaster);
        }
        [HttpPost("CreateFilingApprovalStatus")]
        public IActionResult CreateFilingApprovalStatus(FilingApprovalStatus FilingApprovalStatus)
        {

            _db.Add(FilingApprovalStatus);
            _db.SaveChanges();

            return Ok(FilingApprovalStatus);
        }
        [HttpDelete("FilingApprovalStatusDelete{FilingApprovalId:Int}")]
        public void FilingApprovalStatusDelete(int FilingApprovalId)
        {
            FilingApprovalStatus FilingApprovalStatus = _db.FilingApprovalStatus.Where(d => d.FilingApprovalId == FilingApprovalId).First();
            _db.FilingApprovalStatus.Remove(FilingApprovalStatus);
            _db.SaveChanges();

        }
        [HttpPut("FilingApprovalStatusUpdate{FilingApprovalId:Int}")]
        public IActionResult FilingApprovalStatusUpdate(int FilingApprovalId, [FromBody] FilingApprovalStatus FilingApprovalStatus)
        {

            if (FilingApprovalStatus == null || FilingApprovalStatus.FilingApprovalId != FilingApprovalId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(FilingApprovalStatus);
            _db.SaveChanges();

            return Ok(FilingApprovalStatus);

        }
        [HttpGet("FilingApprovalStatusList")]
        public IActionResult FilingApprovalStatusList(int WorkflowId)
        {
            return Ok(from
                      f in _db.FilingApprovalStatus
                      where f.WorkflowId == WorkflowId
                      select new
                      {
                         WorkflowId= f.WorkflowId,
                         ApproverName = f.ApproverName,
                         Comments = f.Comments,
                         Status =f.Status,
                         DoneBy=f.DoneBy,
                         DoneOn=f.DoneOn,
                      });

        }


        [HttpGet("FilingBusinessCategoryList{FilingId:Int}")]
        public IActionResult FilingBusinessCategoryList(int FilingId)
        {
            FilingBusinessCategory FilingMaster = _db.FilingBusinessCategory.FirstOrDefault(x => x.FilingId== FilingId);

            return Ok(FilingMaster);

        }


        [HttpPost("CreateFilingBusinessCategory")]
        public IActionResult CreateFilingBusinessCategory([FromBody] FilingBusinessCategory []FilingBusinessCategory)
        {
            foreach (FilingBusinessCategory Bc in FilingBusinessCategory)
            {
                _db.Add(Bc);
            }

            _db.SaveChanges();
            return Ok(FilingBusinessCategory);

        }

        [HttpDelete("FilingBusinessCategory{id:Int}")]
        public void FilingBusinessCategoryDelete1(int id)
        {
            FilingBusinessCategory filingbusinesscategory;

            filingbusinesscategory = _db.FilingBusinessCategory.Where(d => d.Id == id).First();
            _db.FilingBusinessCategory.Remove(filingbusinesscategory);
            _db.SaveChanges();

        }




        //=====================================


        [HttpGet("FilingDraftBusinessCategoryList{DraftId:Int}")]
        public IActionResult FilingDraftBusinessCategoryList(int DraftId)
        {
            FilingDraftBusinessCategory res = _db.FilingDraftBusinessCategory.FirstOrDefault(x => x.DraftId == DraftId);

            return Ok(res);

        }




        [HttpPost("CreateFilingDraftBusinessCategory")]
        public IActionResult CreateFilingDraftBusinessCategory([FromBody] FilingDraftBusinessCategory  []Filingdraft)
        {
            foreach (FilingDraftBusinessCategory Bc in Filingdraft)
            {
                _db.Add(Bc);
            }
            
            _db.SaveChanges();

            return Ok(Filingdraft);
        }

       
        [HttpGet("FilingDraftBusinessCategoryGetById1/{DraftId:int}")]
        public ActionResult  FilingDraftBusinessCategoryGetById(int DraftId)
        {
            var res = _db.FilingDraftBusinessCategory.FirstOrDefault(p => p.DraftId== DraftId);

            return Ok(res);


        }

        [HttpGet("FilingDraftBusinessCategoryGetById/{id:int}")]
        public ActionResult  FilingDraftBusinessCategoryGetById1(int id)
        {
            var res = _db.FilingDraftBusinessCategory.FirstOrDefault(p => p.Id == id);

            return Ok(res);


        }

        [HttpPut("FilingDraftBusinessCategoryUpdate{id:Int}")]
        public IActionResult FilingDraftBusinessCategoryUpdate(int id, [FromBody] FilingDraftBusinessCategory Filingdraft)
        {

            if (Filingdraft == null || Filingdraft.Id != id)
            {
                return BadRequest(ModelState);
            }
            _db.Update(Filingdraft);
            _db.SaveChanges();

            return Ok(Filingdraft);

        }


        [HttpDelete("FilingDraftBusinessCategoryDelete1/{Id:Int}")]
        public void FilingDraftBusinessCategoryDelete(int Id)
        {
            FilingDraftBusinessCategory Filingdraft;

            Filingdraft = _db.FilingDraftBusinessCategory.Where(d => d.Id == Id).First();
            _db.FilingDraftBusinessCategory.Remove(Filingdraft);
            _db.SaveChanges();

        }

        [HttpDelete("FilingDraftBusinessCategoryDelete/{DraftId:Int}")]
        public void FilingDraftBusinessCategoryDelete1(int DraftId)
        {
            FilingDraftBusinessCategory Filingdraft;

            Filingdraft = _db.FilingDraftBusinessCategory.Where(d => d.DraftId == DraftId).First();
            _db.FilingDraftBusinessCategory.Remove(Filingdraft);
            _db.SaveChanges();

        }

        //=====================================


        [HttpPost("CreateFilingMasterHistory")]
        public IActionResult CreateFilingMasterHistory([FromBody] FilingMasterHistory filingMasterHistory)
        {

            _db.Add(filingMasterHistory);
            _db.SaveChanges();

            return Ok(filingMasterHistory);
        }

        [HttpGet("FilingMasterHistoryList{FilingId:Int}")]
        public IActionResult FilingMasterHistoryList(int FilingId)
        {
            FilingMasterHistory res = _db.FilingMasterHistory.FirstOrDefault(x => x.FilingId == FilingId);

            return Ok(res);

        }

        [HttpGet("FilingMasterHistoryGetById/{Historyid:int}")]
        public ActionResult<FilingMasterHistory> FilingMasterHistoryGetById(int Historyid)
        {
            var res = _db.FilingMasterHistory.FirstOrDefault(p => p.Historyid == Historyid);

            return Ok(res);


        }

        [HttpPut("FilingMasterHistoryUpdate{Historyid:Int}")]
        public IActionResult FilingMasterHistoryUpdate(int Historyid, [FromBody] FilingMasterHistory FilingMaster)
        {

            if (FilingMaster == null || FilingMaster.Historyid != Historyid)
            {
                return BadRequest(ModelState);
            }
            _db.Update(FilingMaster);
            _db.SaveChanges();

            return Ok(FilingMaster);

        }


        [HttpDelete("FilingMasterHistoryDelete{Historyid:Int}")]
        public void FilingMasterHistoryDelete(int Historyid)
        {
            FilingMasterHistory FilingMaster;

            FilingMaster = _db.FilingMasterHistory.Where(d => d.Historyid == Historyid).First();
            _db.FilingMasterHistory.Remove(FilingMaster);
            _db.SaveChanges();

        }

        //====================================
       
        [HttpPut("FilingMasterApprove{WorkflowId:Int}")]
        public IActionResult FilingMasterApprove(ulong WorkflowId, ulong Userid, ulong DraftId )
        {
            FilingMasterWorkflow FilingMasterWorkflow =  (from w in _db.FilingMasterWorkflow
                                                          where w.WorkflowId == WorkflowId
                                                          select w).FirstOrDefault();

            FilingMasterDraft FilingMasterDraft = (from f in _db.FilingMasterDraft
                                                   where f.DraftId == DraftId
                                                   select f).FirstOrDefault();

            if (FilingMasterWorkflow == null || FilingMasterDraft == null)
            {
                return BadRequest(ModelState);
            }

            FilingMasterWorkflow.WorkflowStatus = "Approved";
            FilingMasterWorkflow.UpdateDate = DateTime.Now;
            FilingMasterWorkflow.UpdateUser = Userid.ToString();
            _db.FilingMasterWorkflow.Attach(FilingMasterWorkflow);
            _db.Entry(FilingMasterWorkflow).Property(x => x.WorkflowStatus).IsModified = true;
            _db.Entry(FilingMasterWorkflow).Property(x => x.UpdateDate).IsModified = true;
            _db.Entry(FilingMasterWorkflow).Property(x => x.UpdateUser).IsModified = true;

            FilingMasterDraft.ChangesInprogress = false;
            FilingMasterDraft.UpdateDate = DateTime.Now;
            FilingMasterDraft.UpdateUser = Userid.ToString();
            _db.FilingMasterDraft.Attach(FilingMasterDraft);
            _db.Entry(FilingMasterDraft).Property(x => x.ChangesInprogress).IsModified = true;
            _db.Entry(FilingMasterDraft).Property(x => x.UpdateDate).IsModified = true;
            _db.Entry(FilingMasterDraft).Property(x => x.UpdateUser).IsModified = true;


            decimal FilingId = 0;
            if (FilingMasterDraft != null)
            {
                
                var DraftBusinessCategoryInfo = (from f in _db.FilingDraftBusinessCategory
                                                 where f.DraftId == DraftId
                                                 select f).ToList();

                var FilingBusinessCategoryInfo = (from f in _db.FilingMasterDraft
                                                  join o in _db.FilingBusinessCategory on f.FilingId equals o.FilingId
                                                  where f.DraftId == DraftId
                                                  select o);

                foreach (FilingBusinessCategory Bc in FilingBusinessCategoryInfo)
                {
                    _db.FilingBusinessCategory.Remove((FilingBusinessCategory)Bc);
                }


                if (FilingMasterDraft.BusinessOperation.Contains("edit"))
                {
                    var rowsToUpdate = _db.FilingMaster.AsEnumerable().Where(r => r.FilingId == FilingMasterDraft.FilingId).FirstOrDefault();
                    if (rowsToUpdate != null)
                    {
                        rowsToUpdate.FilingName = FilingMasterDraft.FilingName;
                        rowsToUpdate.FilingDescription = FilingMasterDraft.FilingDescription;
                        rowsToUpdate.FilingFrequency = FilingMasterDraft.FilingFrequency;
                        rowsToUpdate.StateInfo = FilingMasterDraft.StateInfo;
                        rowsToUpdate.RuleInfo = FilingMasterDraft.RuleInfo;
                        rowsToUpdate.Required = FilingMasterDraft.Required;
                        rowsToUpdate.Jsidept = FilingMasterDraft.Jsidept;
                        rowsToUpdate.JsicontactName = FilingMasterDraft.JsicontactName;
                        rowsToUpdate.JsicontactEmail = FilingMasterDraft.JsicontactEmail;
                        rowsToUpdate.JSIContactNumber = FilingMasterDraft.JSIContactNumber;
                        rowsToUpdate.UpdateDate = FilingMasterDraft.UpdateDate;
                        rowsToUpdate.UpdateUser = FilingMasterDraft.UpdateUser;
                        rowsToUpdate.Juristiction = FilingMasterDraft.Juristiction;
                        rowsToUpdate.Notes = FilingMasterDraft.Notes;
                        rowsToUpdate.ChangesInprogress = false;
                        rowsToUpdate.DueDayofFrequency = FilingMasterDraft.DueDayofFrequency;
                        _db.FilingMaster.Update(rowsToUpdate);
                        FilingId = (decimal) FilingMasterDraft.FilingId;
                    }
                    
                    
                }
                else if (FilingMasterDraft.BusinessOperation.Contains("add"))
                {
                    FilingMaster FilingMasterData = new FilingMaster
                    {
                        FilingName = FilingMasterDraft.FilingName,
                        FilingDescription = FilingMasterDraft.FilingDescription,
                        FilingFrequency = FilingMasterDraft.FilingFrequency,
                        StateInfo = FilingMasterDraft.StateInfo,
                        Required = FilingMasterDraft.Required,
                        RuleInfo = FilingMasterDraft.RuleInfo,
                        Jsidept = FilingMasterDraft.Jsidept,
                        JsicontactName = FilingMasterDraft.JsicontactName,
                        JsicontactEmail = FilingMasterDraft.JsicontactEmail,
                        JSIContactNumber = FilingMasterDraft.JSIContactNumber,
                        UpdateDate = FilingMasterDraft.UpdateDate,
                        UpdateUser = FilingMasterDraft.UpdateUser,
                        Juristiction = FilingMasterDraft.Juristiction,
                        DueDayofFrequency = FilingMasterDraft.DueDayofFrequency,
                        Notes = FilingMasterDraft.Notes,
                        ChangesInprogress = false
                };
                    _db.FilingMaster.Add(FilingMasterData);
                    _db.SaveChanges();
                    FilingId = _db.FilingMaster.Max(u => (decimal)u.FilingId);

                }
                else if (FilingMasterDraft.BusinessOperation.Contains("delete"))
                {
                    var rowsToDelete = _db.FilingMaster.AsEnumerable().Where(r => r.FilingId == FilingMasterDraft.FilingId).FirstOrDefault();
                    if (rowsToDelete != null)
                    {
                        _db.FilingMaster.Remove(rowsToDelete);
                    }
                }
                

                if (FilingMasterDraft.BusinessOperation.Contains("add") || FilingMasterDraft.BusinessOperation.Contains("edit"))
                {
                    foreach (FilingDraftBusinessCategory Bc in DraftBusinessCategoryInfo)
                    {
                        _db.FilingBusinessCategory.Add(new FilingBusinessCategory
                        {
                            FilingId = FilingId,
                            BusinessCategoryId = Bc.BusinessCategoryId,
                            State = Bc.State
                        });
                        
                    }
                    foreach(FilingMasterDraftAttachments da in (_db.FilingMasterDraftAttachments.Where(u=>u.DraftId== FilingMasterDraft.DraftId)))
                    {
                        _db.FilingMasterAttachments.Add(new FilingMasterAttachments
                        {
                            FilingId = FilingId,
                            AttachmentPath = da.AttachmentPath,
                            CreateDate = DateTime.Now
                        }) ;

                    }
                }
                _db.FilingMasterWorkflowNotifications.Add(new FilingMasterWorkflowNotifications
                {
                    WorkflowId = FilingMasterWorkflow.WorkflowId,
                    NotifiedUserId = FilingMasterWorkflow.CurrentApproverId,
                    NotificationType = "Notification",
                    NotificationSubject = "Approval Informaton",
                    NotificationText = "Changes in FilingMaster has been approved",
                    InformationRead = false,
                    InformationDeleted = false,
                    CreateDate = DateTime.Now,
                    CreateUser = "System"
                });
            }
            _db.SaveChanges();
            return Ok();
        }

        [HttpGet("FilingMasterWorkflowNotificationsList")]
        public APIStatusJSON FilingMasterWorkflowNotificationsList()
        {
            try
            {
                var FilingMasterWorkflowNotifications = _db.FilingMasterWorkflowNotifications.ToList();

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterWorkflowNotifications, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        [HttpGet("FilingMasterWorkflowNotificationsList{ReceipientId}")]
        public APIStatusJSON FilingMasterWorkflowNotificationsListbyReceipientId(int ReceipientId)
        {
            try
            {
                var FilingMasterWorkflowNotifications = _db.FilingMasterWorkflowNotifications.Where(u => u.NotifiedUserId == ReceipientId).ToList();

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterWorkflowNotifications, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }


        [HttpGet("FilingMasterWorkflowNotificationsListByWorkflowid")]
        public APIStatusJSON FilingMasterWorkflowNotificationsList(int workflowid)
        {
            try
            {
                var FilingMasterWorkflowNotifications = _db.FilingMasterWorkflowNotifications.Where(u => u.WorkflowId == workflowid).ToList();

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterWorkflowNotifications, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpPost("FilingMasterWorkflowNotifications")]
        public APIStatusJSON FilingMasterWorkflowNotifications([FromBody] FilingMasterWorkflowNotifications item)
        {
            try
            {
                _db.FilingMasterWorkflowNotifications.Add(item);
                _db.SaveChanges();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(item, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpPut("FilingMasterReject{WorkflowId:Int}")]
        public IActionResult FilingMasterReject(ulong WorkflowId, ulong Userid, ulong DraftId)
        {

            FilingMasterDraft FilingMasterDraft = (from f in _db.FilingMasterDraft
                                                   where f.DraftId == DraftId
                                                   select f).FirstOrDefault();

            FilingMasterWorkflow FilingMasterWorkflow = (from w in _db.FilingMasterWorkflow
                                                         where w.WorkflowId == WorkflowId
                                                         select w).FirstOrDefault();
            if (FilingMasterWorkflow == null)
            {
                return BadRequest(ModelState);
            }
         
            FilingMasterWorkflow.WorkflowStatus = "Rejected";
            FilingMasterWorkflow.UpdateDate = DateTime.Now;
            FilingMasterWorkflow.UpdateUser = Userid.ToString();
            _db.FilingMasterWorkflow.Attach(FilingMasterWorkflow);
            _db.Entry(FilingMasterWorkflow).Property(x => x.WorkflowStatus).IsModified = true;
            _db.Entry(FilingMasterWorkflow).Property(x => x.UpdateDate).IsModified = true;
            _db.Entry(FilingMasterWorkflow).Property(x => x.UpdateUser).IsModified = true;


            FilingMasterDraft.ChangesInprogress = false;
            FilingMasterDraft.UpdateDate = DateTime.Now;
            FilingMasterDraft.Status = FilingMasterWorkflow.WorkflowStatus;
            FilingMasterDraft.UpdateUser = Userid.ToString();
            _db.FilingMasterDraft.Attach(FilingMasterDraft);
            _db.Entry(FilingMasterDraft).Property(x => x.ChangesInprogress).IsModified = true;
            _db.Entry(FilingMasterDraft).Property(x => x.UpdateDate).IsModified = true;
            _db.Entry(FilingMasterDraft).Property(x => x.UpdateUser).IsModified = true;

            var rowsToUpdate = _db.FilingMaster.AsEnumerable().Where(r => r.FilingId == FilingMasterDraft.FilingId).FirstOrDefault();
            if (rowsToUpdate != null)
                rowsToUpdate.ChangesInprogress = false;

            _db.FilingMasterWorkflowNotifications.Add(new FilingMasterWorkflowNotifications
            {
                WorkflowId = FilingMasterWorkflow.WorkflowId,
                NotifiedUserId = FilingMasterWorkflow.CurrentApproverId,
                NotificationType = "Notification",
                NotificationSubject = "Reject Notification",
                NotificationText = "Changes in FilingMaster has been Rejected",
                InformationRead = false,
                InformationDeleted = false,
                CreateDate = DateTime.Now,
                CreateUser = "System"
            });


            _db.SaveChanges();
            return Ok();
        }
        [HttpGet("FilingMasterList")]
        public IActionResult FilingMasterList()
        {
            return Ok((from o in _db.FilingMaster
                       select new
                       {
                           FilingId = o.FilingId,
                           FilingName=o.FilingName,
                           FilingDescription = o.FilingDescription,
                           FilingFrequency = o.FilingFrequency,
                           StateInfo = o.StateInfo,
                           RuleInfo = o.RuleInfo,
                           Required = o.Required,
                           BusinessCategory = (from i in _db.BusinessCategoryMaster
                                               join j in _db.FilingBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                               where j.FilingId == o.FilingId
                                               select new { i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                           Jsidept = o.Jsidept,
                           JsicontactName = o.JsicontactName,
                           JsicontactEmail = o.JsicontactEmail,
                           Juristiction = o.Juristiction,
                           Notes = o.Notes,
                           CreateDate = o.CreateDate,
                           CreateUser = o.CreateUser,
                           UpdateDate = o.UpdateDate,
                           UpdateUser = o.UpdateUser,
                           ChangesInprogress = o.ChangesInprogress,
                           DueDayofFrequency = o.DueDayofFrequency
                       }
                       ));

        }
        [HttpGet("FilingMasterList{FilingId:Int}")]
        public IActionResult FilingMasterList(int FilingId)
        {
            return Ok((from o in _db.FilingMaster
                       where o.FilingId== FilingId
                       select new
                       {
                           FilingId = o.FilingId,
                           FilingName = o.FilingName,
                           FilingDescription = o.FilingDescription,
                           FilingFrequency = o.FilingFrequency,
                           StateInfo = o.StateInfo,
                           RuleInfo = o.RuleInfo,
                           Required = o.Required,
                           BusinessCategory = (from i in _db.BusinessCategoryMaster
                                               join j in _db.FilingBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                               where j.FilingId == o.FilingId
                                               select new { i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                           Jsidept = o.Jsidept,
                           JsicontactName = o.JsicontactName,
                           JsicontactEmail = o.JsicontactEmail,
                           Juristiction = o.Juristiction,
                           Notes = o.Notes,
                           CreateDate = o.CreateDate,
                           CreateUser = o.CreateUser,
                           UpdateDate = o.UpdateDate,
                           UpdateUser = o.UpdateUser,
                           ChangesInprogress = o.ChangesInprogress,
                           DueDayofFrequency = o.DueDayofFrequency
                       }
                       ));

        }
        [HttpPut("FilingMasterUpdate{FilingId:Int}")]
        public IActionResult FilingMasterUpdate(int FilingId, [FromBody] FilingMaster FilingMaster)
        {

            if (FilingMaster == null || FilingMaster.FilingId != FilingId)
            {
                return BadRequest(ModelState);
            }

            FilingMasterHistory FilingMasterHistory = new FilingMasterHistory();
            FilingMasterHistory.FilingId = FilingMaster.FilingId;
            FilingMasterHistory.FilingName = FilingMaster.FilingName;
            FilingMasterHistory.FilingDescription = FilingMaster.FilingDescription;
            FilingMasterHistory.FilingFrequency = FilingMaster.FilingFrequency;
            FilingMasterHistory.StateInfo = FilingMaster.StateInfo;
            FilingMasterHistory.RuleInfo = FilingMaster.RuleInfo;
            FilingMasterHistory.Required= FilingMaster.Required;
            FilingMasterHistory.Jsidept= FilingMaster.Jsidept;
            FilingMasterHistory.JsicontactName = FilingMaster.JsicontactName;
            FilingMasterHistory.JsicontactEmail = FilingMaster.JsicontactEmail;
            FilingMasterHistory.JSIContactNumber = FilingMaster.JSIContactNumber;
            FilingMasterHistory.CreateDate = FilingMaster.CreateDate;
            FilingMasterHistory.CreateUser = FilingMaster.CreateUser;
            FilingMasterHistory.UpdateDate = FilingMaster.UpdateDate;
            FilingMasterHistory.UpdateUser = FilingMaster.UpdateUser;
            FilingMasterHistory.Juristiction = FilingMaster.Juristiction;
            FilingMasterHistory.Notes = FilingMaster.Notes;
            FilingMasterHistory.DueDayofFrequency = FilingMaster.DueDayofFrequency;
            FilingMasterHistory.ChangesInprogress = FilingMaster.ChangesInprogress;
            FilingMasterHistory.DueDayofFrequency = FilingMaster.DueDayofFrequency;
            FilingMasterHistory.Dboperation = "Update FilingMaster ";
            FilingMasterHistory.Source = "FilingMasterUpdate API";

            _db.Update(FilingMaster);
            CreateFilingMasterHistory(FilingMasterHistory);
            _db.SaveChanges();

            return Ok(FilingMaster);

        }
        [HttpDelete("FilingMasterDelete{FilingId:Int}")]
        public IActionResult FilingMasterDelete(ulong FilingId)
        {
            

            var CustomerFilingDraft = _db.CustomerFilingMasterDraft.Where(d => d.FilingId == FilingId);
            var CustomerFilingMaster = _db.CustomerFilingMaster.Where(d => d.FilingId == FilingId);

            if (CustomerFilingDraft.ToList().Count==0 && CustomerFilingMaster.ToList().Count == 0)
            {
                FilingMaster FilingMaster = _db.FilingMaster.Where(d => d.FilingId == FilingId).FirstOrDefault();

                if (FilingMaster !=null)
                {
                    FilingMasterHistory filingMasterHistory = new FilingMasterHistory
                    {
                        FilingId = FilingMaster.FilingId,
                        FilingName = FilingMaster.FilingName,
                        FilingDescription = FilingMaster.FilingDescription,
                        FilingFrequency = FilingMaster.FilingFrequency,
                        StateInfo = FilingMaster.StateInfo,
                        RuleInfo = FilingMaster.RuleInfo,
                        Required = FilingMaster.Required,
                        Jsidept = FilingMaster.Jsidept,
                        JsicontactName = FilingMaster.JsicontactName,
                        JsicontactEmail = FilingMaster.JsicontactEmail,
                        JSIContactNumber = FilingMaster.JSIContactNumber,
                        CreateDate = FilingMaster.CreateDate,
                        CreateUser = FilingMaster.CreateUser,
                        UpdateDate = FilingMaster.UpdateDate,
                        UpdateUser = FilingMaster.UpdateUser,
                        Juristiction = FilingMaster.Juristiction,
                        Notes = FilingMaster.Notes,
                        ChangesInprogress = FilingMaster.ChangesInprogress,
                        Dboperation = "Delete FilingMaster",
                        Source = "FilingMasterDelete API",
                        DueDayofFrequency = FilingMaster.DueDayofFrequency
                    };

                    _db.FilingMasterHistory.Add(filingMasterHistory);
                    _db.FilingMaster.Remove(FilingMaster);
                    _db.SaveChanges();
                }
                else
                {
                    return NotFound("FilingID Not Found");
                }

            }
            else
            {   
                return NotFound("FilingID Used in CustomerFilingDraft or CustomerFilingMaster");
            }
            return Ok();
        }
       
        [HttpPost("CreateDraftFilingMaster")]
        public APIStatusJSON CreateDraftFilingMaster([FromBody] FilingMasterDraft FilingMasterDraft)
        {
            try {
                
                FilingMasterDraft.ChangesInprogress = true;
                
                _db.Add(FilingMasterDraft);
                _db.SaveChanges();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterDraft, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch(Exception ex ) { return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message }; }
        }
        [HttpGet("DraftFilingMasterList")]
        public IActionResult DraftFilingMasterList()
        {
            List<FilingMasterDraft> FilingMasterDraft = new List<FilingMasterDraft>();
            FilingMasterDraft = _db.FilingMasterDraft.ToList();

            return Ok(FilingMasterDraft);

        }
        [HttpGet("DraftFilingMasterList{DraftId:Int}")]
        public IActionResult DraftFilingMasterList(int DraftId)
        {
            FilingMasterDraft FilingMasterDraft = _db.FilingMasterDraft.FirstOrDefault(x => x.DraftId == DraftId);

            return Ok(FilingMasterDraft);

        }
        [HttpPut("DraftFilingMasterUpdate{DraftId:Int}")]
        public IActionResult DraftFilingMasterUpdate(int DraftId, [FromBody] FilingMasterDraft FilingMasterDraft)
        {

            if (FilingMasterDraft == null || FilingMasterDraft.DraftId != DraftId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(FilingMasterDraft);
            _db.SaveChanges();

            return Ok(FilingMasterDraft);

        }
        [HttpDelete("DraftFilingMasterDelete{DraftId:Int}")]
        public void DraftFilingMasterDelete(int DraftId)
        {
            FilingMasterDraft FilingMasterDraft;

            FilingMasterDraft = _db.FilingMasterDraft.Where(d => d.DraftId == DraftId).First();
            _db.FilingMasterDraft.Remove(FilingMasterDraft);
            _db.SaveChanges();

        }
        [HttpPost("CreateFilingMasterWorkflow")]
        public APIStatusJSON CreateFilingMasterWorkflow([FromBody] FilingMasterWorkflow FilingMasterWorkflow)
        {
            try
            {
                FilingMasterDraft FilingMasterDraft = _db.FilingMasterDraft
                                            .Where(d => d.DraftId == FilingMasterWorkflow.DraftId).First();

                FilingMasterWorkflow.WorkflowStatus = "Pending";

                string Juristiction = FilingMasterDraft.Juristiction == null ? "State" : FilingMasterDraft.Juristiction;

                if (Juristiction.Contains("State"))
                {
                    FilingMasterWorkflow.CurrentApproverId = (from a in _db.Approvers
                                                              join c in _db.ApproverConfiguration on a.ApproverGroupId equals c.ApproverGroupId
                                                              join f in _db.FilingMasterDraft on a.State equals f.StateInfo
                                                              where a.FilingType.Equals("MasterFiling") && a.Isdefault == true && a.Juristiction == "State"
                                                              select a.ApproverId).FirstOrDefault();
                }
                else if (Juristiction.Contains("Federal"))
                {
                    FilingMasterWorkflow.CurrentApproverId = (from a in _db.Approvers
                                                              join c in _db.ApproverConfiguration on a.ApproverGroupId equals c.ApproverGroupId
                                                              where a.FilingType.Equals("MasterFiling") && a.Isdefault == true && a.Juristiction == "Federal"
                                                              select a.ApproverId).FirstOrDefault();
                }

                if (FilingMasterWorkflow.CurrentApproverId == 0)
                {
                    FilingMasterWorkflow.CurrentApproverId = (from u in _db.Users
                                                              join ur in _db.UsersRoles on u.UserId equals ur.UserId
                                                              join r in _db.Roles on ur.RoleId equals r.RoleId
                                                              where r.RoleName.Contains("Admin")
                                                              select u.UserId).FirstOrDefault();
                    //return new APIStatus { Status = "Failure", ErrorCode = 1, ErrorMessage = "Approver Not Configured" };
                }
                
                _db.Add(FilingMasterWorkflow);
                _db.SaveChanges();
                _db.FilingMasterWorkflowNotifications.Add(new FilingMasterWorkflowNotifications
                {
                    WorkflowId = FilingMasterWorkflow.WorkflowId,
                    NotifiedUserId = FilingMasterWorkflow.WorkflowInitiatorId,
                    NotificationType = "Notification",
                    NotificationSubject = "Request of Approval",
                    NotificationText = "Review And approve my request Workflow",
                    InformationRead = false,
                    InformationDeleted = false,
                    CreateDate = DateTime.Now,
                    CreateUser = "System"
                });
                _db.SaveChanges();

                
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterWorkflow, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        [HttpGet("FilingMasterWorkflowList")]
        public IActionResult FilingMasterWorkflowList()
        {
            return Ok((from o in _db.FilingMasterDraft
                       join c in _db.FilingMasterWorkflow on o.DraftId equals c.DraftId
                       join s in _db.Users on c.CurrentApproverId equals s.UserId
                       where c.WorkflowStatus == "Pending"
                       select new
                       {
                           WorkflowId = c.WorkflowId,
                           DraftId = c.DraftId,
                           FilingId = o.FilingId,
                           FilingName = o.FilingName,
                           FilingDescription = o.FilingDescription,
                           FilingFrequency = o.FilingFrequency,
                           DueDayofFrequency = o.DueDayofFrequency,
                           StateInfo = o.StateInfo,
                           RuleInfo = o.RuleInfo,
                           Required = o.Required,
                           BusinessCategory = (from i in _db.BusinessCategoryMaster
                                               join j in _db.FilingDraftBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                               where j.DraftId == o.DraftId
                                               select new { i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                           Jsidept = o.Jsidept,
                           JsicontactName = o.JsicontactName,
                           JsicontactEmail = o.JsicontactEmail,
                           Juristiction = o.Juristiction,
                           Notes = o.Notes,
                           CreateDate = o.CreateDate,
                           CreateUser = o.CreateUser,
                           UpdateDate = o.UpdateDate,
                           UpdateUser = o.UpdateUser,
                           WorkflowStatus = c.WorkflowStatus,
                           ChangesInprogress = o.ChangesInprogress,
                           ApproverName = s.UserName,
                           ApproverId = c.CurrentApproverId,
                           BusinessOperation = o.BusinessOperation
                       }
                      ));

        }
        [HttpGet("FilingMasterWorkflowListByApprover{UserId:Int}")]
        public IActionResult FilingMasterWorkflowListByApprover(long UserId)
        {
            return Ok((from o in _db.FilingMasterDraft
                       join c in _db.FilingMasterWorkflow on o.DraftId equals c.DraftId
                       join s in _db.Approvers on c.CurrentApproverId equals s.ApproverId
                       where s.ApproverId== UserId && c.WorkflowStatus!= "Pending"
                       select new
                       {
                           WorkflowId = c.WorkflowId,
                           DraftId = c.DraftId,
                           FilingId = o.FilingId,
                           FilingName = o.FilingName,
                           FilingDescription = o.FilingDescription,
                           FilingFrequency = o.FilingFrequency,
                           DueDayofFrequency = o.DueDayofFrequency,
                           StateInfo = o.StateInfo,
                           RuleInfo = o.RuleInfo,
                           Required = o.Required,
                           BusinessCategory = (from i in _db.BusinessCategoryMaster
                                               join j in _db.FilingBusinessCategory on i.BusinessCategoryId equals j.BusinessCategoryId
                                               where j.FilingId == o.FilingId
                                               select new { i.BusinessCategoryId, i.BusinessCategoryName }).ToList(),
                           Jsidept = o.Jsidept,
                           JsicontactName = o.JsicontactName,
                           JsicontactEmail = o.JsicontactEmail,
                           Juristiction = o.Juristiction,
                           Notes = o.Notes,
                           CreateDate = o.CreateDate,
                           CreateUser = o.CreateUser,
                           UpdateDate = o.UpdateDate,
                           UpdateUser = o.UpdateUser,
                           WorkflowStatus = c.WorkflowStatus,
                           ChangesInprogress = o.ChangesInprogress,
                           ApproverName = s.ApproverName,
                           ApproverId = s.ApproverId,
                           BusinessOperation = o.BusinessOperation
                       }
                       ));

        }
        [HttpGet("FilingMasterWorkflowList/{WorkflowId:Int}")]
        public IActionResult FilingMasterWorkflowList(int WorkflowId)
        {
            try
            {
                var FilingMasterWorkflow =(from f in _db.FilingMasterWorkflow
                           join c in _db.FilingMasterDraft on f.DraftId equals c.DraftId
                              where f.WorkflowId == WorkflowId
                              select new
                           {
                               WorkflowId = f.WorkflowId,
                               WorkflowInitiatorId = f.WorkflowInitiatorId,
                               CurrentApproverId = f.CurrentApproverId,
                               DraftId = f.DraftId,
                               WorkflowStatus = f.WorkflowStatus,
                               FilingId = c.FilingId,
                               FilingName = c.FilingName,
                               CreateDate = f.CreateDate,
                               CreateUser = f.CreateUser,
                               UpdateDate = f.UpdateDate,
                               UpdateUser = f.UpdateUser,
                           }
                    );

                if (FilingMasterWorkflow.Count() == 0) // if no records found
                {
                    return NotFound("No Workflow records found.");
                }


                return Ok( FilingMasterWorkflow );
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            //FilingMasterWorkflow FilingMasterWorkflow = _db.FilingMasterWorkflow.FirstOrDefault(x => x.WorkflowId == WorkflowId);

            //return Ok(FilingMasterWorkflow);

        }
        [HttpPut("FilingMasterWorkflowUpdate{WorkflowId:Int}")]
        public IActionResult FilingMasterWorkflowUpdate(int WorkflowId, [FromBody] FilingMasterWorkflow FilingMasterWorkflow)
        {

            if (FilingMasterWorkflow == null || FilingMasterWorkflow.WorkflowId != WorkflowId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(FilingMasterWorkflow);
            _db.SaveChanges();

            return Ok(FilingMasterWorkflow);

        }
        [HttpDelete("FilingMasterWorkflowDelete{WorkflowId:Int}")]
        public void FilingMasterWorkflowDelete(int WorkflowId)
        {
            FilingMasterWorkflow FilingMasterWorkflow;

            FilingMasterWorkflow = _db.FilingMasterWorkflow.Where(d => d.WorkflowId == WorkflowId).First();
            _db.FilingMasterWorkflow.Remove(FilingMasterWorkflow);
            _db.SaveChanges();

        }

        [HttpPost("CreateWorkflowTracking")]
        public IActionResult CreateWorkflowTracking([FromBody] WorkflowTracking WorkflowTracking)
        {

            _db.Add(WorkflowTracking);
            _db.SaveChanges();

            return Ok(WorkflowTracking);
        }
        [HttpGet("WorkflowTrackingList")]
        public IActionResult WorkflowTrackingList()
        {
            List<WorkflowTracking> WorkflowTracking = new List<WorkflowTracking>();
            WorkflowTracking = _db.WorkflowTracking.ToList();

            return Ok(WorkflowTracking);

        }
        [HttpGet("WorkflowTrackingList{WorkflowTrackId:Int}")]
        public IActionResult WorkflowTrackingList(int WorkflowTrackId)
        {
            WorkflowTracking WorkflowTracking = _db.WorkflowTracking.FirstOrDefault(x => x.WorkflowTrackId == WorkflowTrackId);

            return Ok(WorkflowTracking);

        }
        [HttpPut("WorkflowTrackingUpdate{WorkflowTrackId:Int}")]
        public IActionResult WorkflowTrackingUpdate(int WorkflowTrackId, [FromBody] WorkflowTracking WorkflowTracking)
        {

            if (WorkflowTracking == null || WorkflowTracking.WorkflowTrackId != WorkflowTrackId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(WorkflowTracking);
            _db.SaveChanges();

            return Ok(WorkflowTracking);

        }
        [HttpDelete("WorkflowTrackingrDelete{WorkflowTrackId:Int}")]
        public void WorkflowTrackingrDelete(int WorkflowTrackId)
        {
            WorkflowTracking WorkflowTracking;

            WorkflowTracking = _db.WorkflowTracking.Where(d => d.WorkflowTrackId == WorkflowTrackId).First();
            _db.WorkflowTracking.Remove(WorkflowTracking);
            _db.SaveChanges();

        }

        [HttpPost("CreateSystemFilingFollowup")]
        public IActionResult CreateSystemFilingFollowup([FromBody] SystemFilingFollowup SystemFilingFollowup)
        {

            _db.Add(SystemFilingFollowup);
            _db.SaveChanges();

            return Ok(SystemFilingFollowup);
        }
        [HttpGet("SystemFilingFollowupList")]
        public IActionResult SystemFilingFollowupList()
        {
            List<SystemFilingFollowup> SystemFilingFollowup = new List<SystemFilingFollowup>();
            SystemFilingFollowup = _db.SystemFilingFollowup.ToList();

            return Ok(SystemFilingFollowup);

        }
        [HttpGet("SystemFilingFollowupList{FileTrackingId:Int}")]
        public IActionResult SystemFilingFollowupList(int FileTrackingId)
        {
            SystemFilingFollowup SystemFilingFollowup = _db.SystemFilingFollowup.FirstOrDefault(x => x.FileTrackingId == FileTrackingId);

            return Ok(SystemFilingFollowup);

        }
        [HttpPut("SystemFilingFollowupUpdate{FileTrackingId:Int}")]
        public IActionResult SystemFilingFollowupUpdate(int FileTrackingId, [FromBody] SystemFilingFollowup SystemFilingFollowup)
        {

            if (SystemFilingFollowup == null || SystemFilingFollowup.FileTrackingId != FileTrackingId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(SystemFilingFollowup);
            _db.SaveChanges();

            return Ok(SystemFilingFollowup);

        }
        [HttpDelete("SystemFilingFollowuprDelete{FileTrackingId:Int}")]
        public void SystemFilingFollowuprDelete(int FileTrackingId)
        {
            SystemFilingFollowup SystemFilingFollowup;

            SystemFilingFollowup = _db.SystemFilingFollowup.Where(d => d.FileTrackingId == FileTrackingId).First();
            _db.SystemFilingFollowup.Remove(SystemFilingFollowup);
            _db.SaveChanges();

        }

        [HttpPut("FilingMasterCommentsUpdate{CommentsId:Int}")]
        public IActionResult FilingMasterCommentsUpdate(int CommentsId, [FromBody] FilingMasterComments FilingMasterComments)
        {
            if (FilingMasterComments == null || FilingMasterComments.CommentsId != CommentsId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(FilingMasterComments);
            _db.SaveChanges();
            return Ok(FilingMasterComments);
        }
        [HttpGet("FilingMasterAttachmentsList")]
        public ActionResult FilingMasterAttachmentsList()
        {            
            return Ok(_db.FilingMasterAttachments);
         }

       
       
        [HttpPut("FilingMasterDraftCommentsUpdate{CommentsId:Int}")]
        public IActionResult FilingMasterDraftCommentsUpdate(int CommentsId, [FromBody] FilingMasterDraftComments FilingMasterDraftComments)
        {
            if (FilingMasterDraftComments == null || FilingMasterDraftComments.CommentsId != CommentsId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(FilingMasterDraftComments);
            _db.SaveChanges();
            return Ok(FilingMasterDraftComments);
        }
        [HttpPut("FilingMasterWorkflowCommentsUpdate{CommentsId:Int}")]
        public IActionResult FilingMasterWorkflowCommentsUpdate(int CommentsId, [FromBody] FilingMasterWorkflowComments FilingMasterWorkflowComments)
        {
            if (FilingMasterWorkflowComments == null || FilingMasterWorkflowComments.CommentsId != CommentsId)
            {
                return BadRequest(ModelState);
            }
            _db.Update(FilingMasterWorkflowComments);
            _db.SaveChanges();
            return Ok(FilingMasterWorkflowComments);
        }

        [HttpGet("FilingMasterComments{FilingId:Int}")]
        public IActionResult FilingMasterComments(int FilingId)
        {
            return Ok((from o in _db.FilingMasterComments
                       where o.FilingId == FilingId
                       select new
                       {
                           CommentsId = o.CommentsId,
                           FilingId=o.FilingId,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser

                       }));

        }
        [HttpGet("FilingMasterWorkflowComments{WorkflowId:Int}")]
        public IActionResult FilingMasterWorkflowComments(int WorkflowId)
        {
            return Ok((from o in _db.FilingMasterWorkflowComments
                       where o.WorkflowId == WorkflowId
                       select new
                       {
                           CommentsId = o.CommentsId,
                           WorkflowId = o.WorkflowId,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }
        [HttpGet("FilingMasterDraftComments{DraftId:Int}")]
        public IActionResult FilingMasterDraftComments(int DraftId)
        {
            return Ok((from o in _db.FilingMasterDraftComments
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

        [HttpGet("FilingMasterCommentsbyId{CommentsId:Int}")]
        public IActionResult FilingMasterCommentsbyId(int CommentsId)
        {
            return Ok((from o in _db.FilingMasterComments
                       where o.CommentsId == CommentsId
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
        [HttpGet("FilingMasterWorkflowCommentsbyId{CommentsId:Int}")]
        public IActionResult FilingMasterWorkflowCommentsbyId(int CommentsId)
        {
            return Ok((from o in _db.FilingMasterWorkflowComments
                       where o.CommentsId == CommentsId
                       select new
                       {
                           CommentsId = o.CommentsId,
                           WorkflowId = o.WorkflowId,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }
        [HttpGet("FilingMasterDraftCommentsbyId{CommentsId:Int}")]
        public IActionResult FilingMasterDraftCommentsbyId(int CommentsId)
        {
            return Ok((from o in _db.FilingMasterDraftComments
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

        [HttpDelete("FilingMasterCommentssDelete{CommentsId:Int}")]
        public void FilingMasterCommentssDelete(int CommentsId)
        {
            FilingMasterComments FilingMasterComments;

            FilingMasterComments = _db.FilingMasterComments.Where(d => d.CommentsId == CommentsId).First();
            _db.FilingMasterComments.Remove(FilingMasterComments);
            _db.SaveChanges();

        }
        [HttpDelete("CustomerFilingCommentsDelete{CommentId:Int}")]
        public void CustomerFilingCommentsDelete(int CommentsId)
        {
            FilingMasterDraftComments FilingMasterDraftComments;

            FilingMasterDraftComments = _db.FilingMasterDraftComments.Where(d => d.CommentsId == CommentsId).First();
            _db.FilingMasterDraftComments.Remove(FilingMasterDraftComments);
            _db.SaveChanges();

        }
        [HttpDelete("FilingMasterWorkflowCommentsDelete{CommentId:Int}")]
        public void FilingMasterWorkflowCommentsDelete(int CommentsId)
        {
            FilingMasterWorkflowComments FilingMasterWorkflowComments;

            FilingMasterWorkflowComments = _db.FilingMasterWorkflowComments.Where(d => d.CommentsId == CommentsId).First();
            _db.FilingMasterWorkflowComments.Remove(FilingMasterWorkflowComments);
            _db.SaveChanges();

        }

        [HttpPost("CreateFilingMasterComments")]
        public IActionResult CreateFilingMasterComments([FromBody] FilingMasterComments FilingMasterComments)
        {

            _db.Add(FilingMasterComments);
            _db.SaveChanges();

            return Ok(FilingMasterComments);
        }
        [HttpPost("CreateFilingMasterDraftComments")]
        public IActionResult CreateFilingMasterDraftComments([FromBody] FilingMasterDraftComments FilingMasterDraftComments)
        {

            _db.Add(FilingMasterDraftComments);
            _db.SaveChanges();

            return Ok(FilingMasterDraftComments);
        }
        [HttpPost("CreateFilingMasterWorkflowComments")]
        public IActionResult CreateFilingMasterWorkflowComments([FromBody] FilingMasterWorkflowComments FilingMasterWorkflowComments)
        {

            _db.Add(FilingMasterWorkflowComments);
            _db.SaveChanges();

            return Ok(FilingMasterWorkflowComments);
        }
        // ========================================================================================
        [HttpPost("CreateFilingMasterDraftAttachments")]
        public APIStatusJSON CreateFilingMasterDraftAttachments(FilingMasterDraftAttachments Attachment)
        {
            try
            {
                _db.FilingMasterDraftAttachments.Add(Attachment);
                _db.SaveChanges();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(Attachment, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpGet("ViewFilingMasterDraftAttachments/{AttachmentId:decimal}")]
        public APIStatusJSON ViewFilingMasterDraftAttachments(decimal AttachmentId)
        {
            try
            {
                var FilingMasterDraftAttachments = _db.FilingMasterDraftAttachments
                                               .FirstOrDefault(n => n.AttachmentId == AttachmentId);

                if (FilingMasterDraftAttachments != null)
                {
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterDraftAttachments, new JsonSerializerOptions
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

        [HttpGet("FilingMasterDraftAttachments/{DraftId:decimal}")]
        public APIStatusJSON ListFilingMasterDraftAttachments(decimal DraftId)
        {
            try
            {
                var FilingMasterDraftAttachments = (from fd in _db.FilingMasterDraft
                                                    join fda in _db.FilingMasterDraftAttachments on fd.DraftId equals fda.DraftId
                                                    where fd.DraftId == DraftId
                                                    select new
                                                    {
                                                        DraftId = fd.DraftId,
                                                        AttachmentId = fda.AttachmentId,
                                                        AttachmentPath = fda.AttachmentPath,
                                                        CreateDate = fda.CreateDate,
                                                        CreateUser = fda.CreateUser,
                                                        UpdatedDate = fda.UpdatedDate,
                                                        UpdatedUser = fda.UpdatedUser
                                                    });

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterDraftAttachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpDelete("DeleteFilingMasterDraftAttachments/{AttachmentId:decimal}")]
        public APIStatusJSON DeleteFilingMasterDraftAttachments(decimal AttachmentId)
        {

            try
            {

                var FilingMasterDraftAttachments = _db.FilingMasterDraftAttachments
                                            .FirstOrDefault(n => n.AttachmentId == AttachmentId);

                if (FilingMasterDraftAttachments != null)
                {
                    _db.FilingMasterDraftAttachments.Remove(FilingMasterDraftAttachments);
                    _db.SaveChanges();
                    return new APIStatusJSON { Status = "Success" };
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


        [HttpPut("UpdateFilingMasterDraftAttachments/{AttachmentId:decimal}")]
        public APIStatusJSON UpdateFilingMasterDraftAttachments(decimal AttachmentId, [FromBody] FilingMasterDraftAttachments FilingMasterDraftAttachments)
        {
            try
            {

                var existingFiling = _db.FilingMasterDraftAttachments.
                                      FirstOrDefault(n => n.AttachmentId == AttachmentId);

                if (existingFiling != null)

                {
                    existingFiling.AttachmentPath = FilingMasterDraftAttachments.AttachmentPath;
                    existingFiling.DraftId = FilingMasterDraftAttachments.DraftId;
                    existingFiling.CreateDate = FilingMasterDraftAttachments.CreateDate;
                    existingFiling.CreateUser = FilingMasterDraftAttachments.CreateUser;
                    existingFiling.UpdatedDate = FilingMasterDraftAttachments.UpdatedDate;
                    existingFiling.UpdatedUser = FilingMasterDraftAttachments.UpdatedUser;

                    _db.SaveChanges();
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterDraftAttachments, new JsonSerializerOptions
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

//==============================================================================================================================

        [HttpPost("CreateFilingMasterDraftCommentsAttachments")]
        public IActionResult CreateFilingMasterDraftCommentsAttachments(FilingMasterDraftCommentsAttachments Attachment)
        {
            try
            {
                _db.FilingMasterDraftCommentsAttachments.Add(Attachment);
                _db.SaveChanges();
                return Ok(Attachment);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ViewFilingMasterDraftCommentsAttachments/{AttachmentId:int}")]
        public IActionResult ViewFilingMasterDraftCommentsAttachments(int AttachmentId)
        {
            try
            {
                var FilingMasterDraftCommentsAttachments = _db.FilingMasterDraftCommentsAttachments
                                               .FirstOrDefault(n => n.AttachmentId == AttachmentId);

                if (FilingMasterDraftCommentsAttachments != null)
                {
                    return Ok(FilingMasterDraftCommentsAttachments);
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

        [HttpGet("ListFilingMasterDraftCommentsAttachments")]
        public IActionResult ListFilingMasterDraftCommentsAttachments()
        {
            try
            {
                var FilingMasterDraftCommentsAttachments = _db.FilingMasterDraftCommentsAttachments.ToList();
                return Ok(FilingMasterDraftCommentsAttachments);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpDelete("DeleteFilingMasterDraftCommentsAttachments/{AttachmentId:int}")]
        public IActionResult DeleteFilingMasterDraftCommentsAttachments(int AttachmentId)
        {

            try
            {

                var FilingMasterDraftCommentsAttachments = _db.FilingMasterDraftCommentsAttachments
                                            .FirstOrDefault(n => n.AttachmentId == AttachmentId);

                if (FilingMasterDraftCommentsAttachments != null)
                {
                    _db.FilingMasterDraftCommentsAttachments.Remove(FilingMasterDraftCommentsAttachments);
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

        [HttpPut("UpdateFilingMasterDraftCommentsAttachments/{AttachmentId:int}")]
        public IActionResult UpdateFilingMasterDraftCommentsAttachments(int AttachmentId, [FromBody] FilingMasterDraftCommentsAttachments FilingMasterDraftCommentsAttachments)
        {
            try
            {

                var existingNotification = _db.FilingMasterDraftCommentsAttachments.
                                      FirstOrDefault(n => n.AttachmentId == AttachmentId);


                if (existingNotification != null)
                {
                    existingNotification.AttachmentPath = FilingMasterDraftCommentsAttachments.AttachmentPath;
                    existingNotification.CommentsId = FilingMasterDraftCommentsAttachments.CommentsId;
                    existingNotification.CreateDate = FilingMasterDraftCommentsAttachments.CreateDate;
                    existingNotification.CreateUser = FilingMasterDraftCommentsAttachments.CreateUser;
                    existingNotification.UpdatedDate = FilingMasterDraftCommentsAttachments.UpdatedDate;
                    existingNotification.UpdatedUser = FilingMasterDraftCommentsAttachments.UpdatedUser;

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

        //=================================================================================================


        [HttpPost("CreateFilingMasterWorkflowCommentsAttachments")]
        public IActionResult CreateFilingMasterWorkflowCommentsAttachments(FilingMasterWorkflowCommentsAttachments Attachment)
        {
            try
            {
                _db.FilingMasterWorkflowCommentsAttachments.Add(Attachment);
                _db.SaveChanges();
                return Ok(Attachment);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ViewFilingMasterWorkflowCommentsAttachments/{AttachmentId:int}")]
        public IActionResult ViewFilingMasterWorkflowCommentsAttachments(int AttachmentId)
        {
            try
            {
                var FilingMasterWorkflowCommentsAttachments = _db.FilingMasterWorkflowCommentsAttachments
                                               .FirstOrDefault(n => n.AttachmentId == AttachmentId);

                if (FilingMasterWorkflowCommentsAttachments != null)
                {
                    return Ok(FilingMasterWorkflowCommentsAttachments);
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

        [HttpGet("ListFilingMasterWorkflowCommentsAttachments")]
        public IActionResult ListFilingMasterWorkflowCommentsAttachments()
        {
            try
            {
                var FilingMasterWorkflowCommentsAttachments = _db.FilingMasterWorkflowCommentsAttachments.ToList();
                return Ok(FilingMasterWorkflowCommentsAttachments);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpDelete("DeleteFilingMasterWorkflowCommentsAttachments/{AttachmentId:int}")]
        public IActionResult DeleteFilingMasterWorkflowCommentsAttachments(int AttachmentId)
        {

            try
            {

                var FilingMasterWorkflowCommentsAttachments = _db.FilingMasterWorkflowCommentsAttachments
                                            .FirstOrDefault(n => n.AttachmentId == AttachmentId);

                if (FilingMasterWorkflowCommentsAttachments != null)
                {
                    _db.FilingMasterWorkflowCommentsAttachments.Remove(FilingMasterWorkflowCommentsAttachments);
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

        [HttpPut("UpdateFilingMasterWorkflowCommentsAttachments/{AttachmentId:int}")]
        public IActionResult UpdateFilingMasterWorkflowCommentsAttachments(int AttachmentId, [FromBody] FilingMasterWorkflowCommentsAttachments FilingMasterWorkflowCommentsAttachments)
        {
            try
            {

                var existingNotification = _db.FilingMasterWorkflowCommentsAttachments.
                                      FirstOrDefault(n => n.AttachmentId == AttachmentId);

                if (existingNotification != null)

                {
                    existingNotification.AttachmentPath = FilingMasterWorkflowCommentsAttachments.AttachmentPath;
                    existingNotification.CommentsId = FilingMasterWorkflowCommentsAttachments.CommentsId;
                    existingNotification.CreateDate = FilingMasterWorkflowCommentsAttachments.CreateDate;
                    existingNotification.CreateUser = FilingMasterWorkflowCommentsAttachments.CreateUser;
                    existingNotification.UpdatedDate = FilingMasterWorkflowCommentsAttachments.UpdatedDate;
                    existingNotification.UpdatedUser = FilingMasterWorkflowCommentsAttachments.UpdatedUser;

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
//==============================================================================
        [HttpPost("CreateMasterFilingAttachments")]
        public IActionResult CreateMasterFilingAttachments(MasterFilingAttachments Followup)
        {
            try
            {
                _db.MasterFilingAttachments.Add(Followup);
               _db.SaveChanges();
                return Ok(Followup);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ViewMasterFilingAttachments/{FollowupId:int}")]
        public IActionResult ViewMasterFilingAttachments(int FollowupId)
        {
            try
            {
                var MasterFilingAttachments = _db.MasterFilingAttachments
                                               .FirstOrDefault(n => n.FollowupId == FollowupId);

                if (MasterFilingAttachments != null)
                {
                    return Ok(MasterFilingAttachments);
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

        [HttpGet("ListMasterFilingAttachments")]
        public IActionResult ListMasterFilingAttachments()
        {
            try
            {
                var MasterFilingAttachments = _db.MasterFilingAttachments.ToList();
                return Ok(MasterFilingAttachments);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("DeleteMasterFilingAttachments/{AttachmentId:int}")]
        public IActionResult DeleteMasterFilingAttachments(int AttachmentId)
        {
            try
            {

                var MasterFilingAttachments = _db.MasterFilingAttachments
                                            .FirstOrDefault(n => n.AttachmentId == AttachmentId);

                if (MasterFilingAttachments != null)
                {
                    _db.MasterFilingAttachments.Remove(MasterFilingAttachments);
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
        [HttpPut("UpdateMasterFilingAttachments/{FollowupId:int}")]
        public IActionResult UpdateMasterFilingAttachments(int FollowupId, [FromBody] MasterFilingAttachments MasterFilingAttachments)
        {
            try
            {

                var existingNotification = _db.MasterFilingAttachments.
                                      FirstOrDefault(n => n.FollowupId == FollowupId);

                if (existingNotification != null)

                {
                    existingNotification.AttachmentId = MasterFilingAttachments.AttachmentId;

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

//=====================================================================================


        [HttpPost("createFilingMasterWorkflowNotifications")]
        public APIStatusJSON createFilingMasterWorkflowNotifications(FilingMasterWorkflowNotifications workflow)
        {
            try
            {
                _db.FilingMasterWorkflowNotifications.Add(workflow);
                _db.SaveChanges();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(workflow, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }

        }

        [HttpGet("ListFilingMasterWorkflowNotifications")]
        public APIStatusJSON ListFilingMasterWorkflowNotifications()
        {
            try
            {
                var FilingMasterWorkflowNotifications = _db.FilingMasterWorkflowNotifications.ToList();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterWorkflowNotifications, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpGet("ViewFilingMasterWorkflowNotifications/{NotificationId:Int}")]
        public APIStatusJSON ViewFilingMasterWorkflowNotifications(int NotificationId)
        {
            try
            {
                var FilingMasterWorkflowNotifications = _db.FilingMasterWorkflowNotifications
                                       .FirstOrDefault(F => F.NotificationId == NotificationId);
                if (FilingMasterWorkflowNotifications != null)
                {

                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterWorkflowNotifications, new JsonSerializerOptions
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

        [HttpDelete("DeleteFilingMasterWorkflowNotifications/{NotificationId:Int}")]
        public APIStatusJSON DeleteFilingMasterWorkflowNotifications(int NotificationId)
        {
            try
            {

                var FilingMasterWorkflowNotifications = _db.FilingMasterWorkflowNotifications
                                                 .FirstOrDefault(a => a.NotificationId == NotificationId);

                if (FilingMasterWorkflowNotifications != null)
                {
                    _db.FilingMasterWorkflowNotifications.Remove(FilingMasterWorkflowNotifications);
                    _db.SaveChanges();
                    return new APIStatusJSON { Status = "Success" };
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

        [HttpPut("UpdateFilingMasterWorkflowNotifications/{NotificationId:int}")]
        public APIStatusJSON UpdateFilingMasterWorkflowNotifications(int NotificationId, [FromBody] FilingMasterWorkflowNotifications FilingMasterWorkflowNotifications)
        {
            try
            {

                var existingNotification = _db.FilingMasterWorkflowNotifications.
                                      FirstOrDefault(n => n.NotificationId == NotificationId);

                if (existingNotification != null)

                {
                    existingNotification.WorkflowId = FilingMasterWorkflowNotifications.WorkflowId;
                    existingNotification.NotificationFrom = FilingMasterWorkflowNotifications.NotificationFrom;
                    existingNotification.NotificationTo = FilingMasterWorkflowNotifications.NotificationTo;
                    existingNotification.NotificationCC = FilingMasterWorkflowNotifications.NotificationCC;
                    existingNotification.NotificationSubject = FilingMasterWorkflowNotifications.NotificationSubject;
                    existingNotification.NotificationType = FilingMasterWorkflowNotifications.NotificationType;
                    existingNotification.NotificationText = FilingMasterWorkflowNotifications.NotificationText;
                    existingNotification.InformationRead = FilingMasterWorkflowNotifications.InformationRead;
                    existingNotification.InformationDeleted = FilingMasterWorkflowNotifications.InformationDeleted;
                    existingNotification.CreateDate = FilingMasterWorkflowNotifications.CreateDate;
                    existingNotification.CreateUser = FilingMasterWorkflowNotifications.CreateUser;
                   

                    _db.SaveChanges();
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterWorkflowNotifications, new JsonSerializerOptions
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
//========================================================================================================================================

        [HttpPost("FilingMasterAttachmentsCreate")]
        public APIStatusJSON FilingMasterAttachmentsCreate([FromBody] FilingMasterAttachments item)
        {
            try
            {
                if (item == null)
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "FilingAttachment Not Found" };
                }

                _db.FilingMasterAttachments.Add(item);
                _db.SaveChanges();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(item, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }


        }

        [HttpGet("FilingMasterAttachments/{FilingId:decimal}")]
        public APIStatusJSON ListFilingMasterAttachments(decimal FilingId)
        {
            try
            {
                var FilingMasterAttachments = (from fm in _db.FilingMaster
                                               join fa in _db.FilingMasterAttachments on fm.FilingId equals fa.FilingId
                                               where fm.FilingId == FilingId
                                               select new
                                               {
                                                   FilingId = fm.FilingId,
                                                   FilingName = fm.FilingName,
                                                   AttachmentId = fa.AttachmentId,
                                                   AttachmentPath = fa.AttachmentPath,
                                                   CreateDate = fa.CreateDate,
                                                   CreateUser = fa.CreateUser,
                                                   UpdatedDate = fa.UpdatedDate,
                                                   UpdatedUser = fa.UpdatedUser
                                               });

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterAttachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }


        [HttpGet("FilingMasterAttachmentsGetById/{AttachmentId:int}")]
        public APIStatusJSON FilingMasterAttachmentsGetById(int AttachmentId)
        {
            try
            {
                var FilingMasterAttachments = (from fm in _db.FilingMaster
                                               join fa in _db.FilingMasterAttachments on fm.FilingId equals fa.FilingId
                                               where fa.AttachmentId == AttachmentId
                                               select new
                                               {
                                                   FilingId = fm.FilingId,
                                                   FilingName = fm.FilingName,
                                                   AttachmentId = fa.AttachmentId,
                                                   AttachmentPath = fa.AttachmentPath,
                                                   CreateDate = fa.CreateDate,
                                                   CreateUser = fa.CreateUser,
                                                   UpdatedDate = fa.UpdatedDate,
                                                   UpdatedUser = fa.UpdatedUser
                                               });
            
                if (FilingMasterAttachments != null)
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterAttachments, new JsonSerializerOptions
                        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                    };
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "FilingAttachment Not Found" };
                }

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }


        [HttpPut("FilingMasterAttachmentsupdate{AttachmentId}")]
        public APIStatusJSON FilingMasterAttachmentsupdate([FromBody] FilingMasterAttachments FilingMasterAttachments)
        {
            try
            {
                var existingFiling = _db.FilingMasterAttachments.FirstOrDefault(p => p.AttachmentId == FilingMasterAttachments.AttachmentId);

                if (existingFiling != null)
                {
                    existingFiling.AttachmentPath = FilingMasterAttachments.AttachmentPath;
                    existingFiling.AttachmentId = FilingMasterAttachments.AttachmentId;
                    existingFiling.FilingId = FilingMasterAttachments.FilingId;
                    existingFiling.UpdatedDate = FilingMasterAttachments.UpdatedDate;
                    existingFiling.UpdatedUser = FilingMasterAttachments.UpdatedUser;
                    existingFiling.CreateDate = FilingMasterAttachments.CreateDate;
                    existingFiling.CreateUser = FilingMasterAttachments.CreateUser;


                    _db.SaveChanges();
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterAttachments, new JsonSerializerOptions
                        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                    };

                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "FilingAttachment Not Found" };
                }
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }



        [HttpDelete("(FilingMasterAttachmentsdelete{AttachmentId:int}")]
        public APIStatusJSON FilingMasterAttachmentsdelete(int AttachmentId)
        {
            try
            {
                var res = _db.FilingMasterAttachments.FirstOrDefault(t => t.AttachmentId == AttachmentId);
                if (res == null)
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Attachment Not Found" };
                }

                _db.FilingMasterAttachments.Remove(res);
                _db.SaveChanges();
                return new APIStatusJSON { Status = "Success" };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        [HttpGet("FilingMasterWorkflowAttachments/{WorkflowId:decimal}")]
        public APIStatusJSON ListFilingMasterWorkflowAttachments(decimal WorkflowId)
        {
            try
            {
                var FilingMasterWorkflowAttachments = (from fw in _db.FilingMasterWorkflow
                                                       join fwa in _db.FilingMasterWorkflowAttachments on fw.WorkflowId equals fwa.WorkFlowId
                                                       where fw.WorkflowId == WorkflowId
                                                       select new
                                                       {
                                                           WorkflowId = fw.WorkflowId,
                                                           WorkflowStatus = fw.WorkflowStatus,
                                                           AttachmentId = fwa.AttachmentId,
                                                           AttachmentPath = fwa.AttachmentPath,
                                                           CreateDate = fwa.CreateDate,
                                                           CreateUser = fwa.CreateUser,
                                                           UpdatedDate = fwa.UpdatedDate,
                                                           UpdatedUser = fwa.UpdatedUser
                                                       });

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterWorkflowAttachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        //================================================================================================================
        [HttpGet("FilingMasterWorkflowCommentsAttachments/{WorkflowId:decimal}/{CommentId:decimal}")]
        public APIStatusJSON ListFilingMasterWorkflowCommentsAttachments(decimal WorkflowId, decimal CommentId)
        {
            try
            {
                var FilingMasterWorkflowCommentsAttachments = (from fmw in _db.FilingMasterWorkflow
                                                               join fmc in _db.FilingMasterComments on fmw.CurrentApproverId equals fmc.CommentsId
                                                               join fmca in _db.FilingMasterWorkflowCommentsAttachments on fmc.CommentsId equals fmca.CommentsId
                                                               where fmw.WorkflowId == WorkflowId && fmc.CommentsId == CommentId
                                                               select new
                                                               {
                                                                   WorkflowId = fmw.WorkflowId,
                                                                   WorkflowStatus = fmw.WorkflowStatus,
                                                                   AttachmentId = fmca.AttachmentId,
                                                                   AttachmentPath = fmca.AttachmentPath,
                                                                   CommentsId = fmca.CommentsId,
                                                                   CreateDate = fmca.CreateDate,
                                                                   CreateUser = fmca.CreateUser,
                                                                   UpdatedDate = fmca.UpdatedDate,
                                                                   UpdatedUser = fmca.UpdatedUser
                                                               });

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterWorkflowCommentsAttachments, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        [HttpGet("FilingMasterCommentsAttachments/{FilingId:decimal}/{CommentId:decimal}")]
        public APIStatusJSON ListFilingMasterCommentsAttachments(decimal FilingId, decimal CommentId)
        {
            try
            {
                var FilingMasterCommentsAttachments = (from f in _db.FilingMaster
                                                       join fmc in _db.FilingMasterComments on f.FilingId equals fmc.FilingId
                                                       join fmca in _db.FilingMasterCommentsAttachments on fmc.CommentsId equals fmca.CommentsId
                                                       where f.FilingId == FilingId && fmc.CommentsId == CommentId
                                                       select new
                                                       {
                                                           FilingId = f.FilingId,
                                                           FilingName = f.FilingName,
                                                           CommentsId = fmc.CommentsId,
                                                           CommentsText = fmc.CommentsText,
                                                           AttachmentId = fmca.AttachmentId,
                                                           AttachmentPath = fmca.AttachmentPath,
                                                           CreateDate = fmca.CreateDate,
                                                           CreateUser = fmca.CreateUser,
                                                           UpdatedDate = fmca.UpdatedDate,
                                                           UpdatedUser = fmca.UpdatedUser
                                                       });

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(FilingMasterCommentsAttachments, new JsonSerializerOptions
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
