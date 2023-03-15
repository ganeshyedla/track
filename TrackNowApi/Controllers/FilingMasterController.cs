using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using TrackNowApi.Data;
using TrackNowApi.Model;

namespace TrackNowApi.Controllers
{
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
        [HttpDelete("FilingApprovalStatusDelete{FilingApprovalID:Int}")]
        public void FilingApprovalStatusDelete(int FilingApprovalID)
        {
            FilingApprovalStatus FilingApprovalStatus = _db.FilingApprovalStatus.Where(d => d.FilingApprovalID == FilingApprovalID).First();
            _db.FilingApprovalStatus.Remove(FilingApprovalStatus);
            _db.SaveChanges();

        }
        [HttpPut("FilingApprovalStatusUpdate{FilingApprovalID:Int}")]
        public IActionResult FilingApprovalStatusUpdate(int FilingApprovalID, [FromBody] FilingApprovalStatus FilingApprovalStatus)
        {

            if (FilingApprovalStatus == null || FilingApprovalStatus.FilingApprovalID != FilingApprovalID)
            {
                return BadRequest(ModelState);
            }
            _db.Update(FilingApprovalStatus);
            _db.SaveChanges();

            return Ok(FilingApprovalStatus);

        }
        [HttpGet("FilingApprovalStatusList")]
        public IActionResult FilingApprovalStatusList(int WorkflowID)
        {
            return Ok(from
                      f in _db.FilingApprovalStatus
                      where f.WorkflowID == WorkflowID
                      select new
                      {
                         WorkflowID= f.WorkflowID,
                         ApproverName = f.ApproverName,
                         Comments = f.Comments,
                         Status =f.Status,
                         DoneBy=f.DoneBy,
                         DoneOn=f.DoneOn,
                      });

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
        [HttpPut("FilingMasterApprove{WorkflowId:Int}")]
        public IActionResult FilingMasterApprove(int WorkflowId, string Userid, [FromBody] FilingMasterDraft FilingMasterDraft)
        {

            FilingMasterWorkflow FilingMasterWorkflow =  (from w in _db.FilingMasterWorkflow
                                                          where w.WorkflowId == WorkflowId
                                                          select w).FirstOrDefault();
            if (FilingMasterWorkflow == null)
            {
                return BadRequest(ModelState);
            }
            FilingMasterWorkflow.WorkflowStatus = "Approved";
            FilingMasterWorkflow.UpdateDate = DateTime.Now;
            FilingMasterWorkflow.UpdateUser = Userid;
            _db.FilingMasterWorkflow.Attach(FilingMasterWorkflow);
            _db.Entry(FilingMasterWorkflow).Property(x => x.WorkflowStatus).IsModified = true;
            _db.Entry(FilingMasterWorkflow).Property(x => x.UpdateDate).IsModified = true;
            _db.Entry(FilingMasterWorkflow).Property(x => x.UpdateUser).IsModified = true;

            FilingMaster FilingMaster = new FilingMaster();

            var rowsToUpdate =     _db.FilingMaster.AsEnumerable().Where(r => r.FilingId == FilingMasterDraft.FilingId).FirstOrDefault();
            if(rowsToUpdate!=null)
            {
                rowsToUpdate.FilingName= FilingMasterDraft.FilingName;
                rowsToUpdate.FilingDescription = FilingMasterDraft.FilingDescription;
                rowsToUpdate.FilingFrequency = FilingMasterDraft.FilingFrequency;
                rowsToUpdate.StateInfo = FilingMasterDraft.StateInfo;
                rowsToUpdate.Required = FilingMasterDraft.Required;
                rowsToUpdate.Jsidept = FilingMasterDraft.Jsidept;
                rowsToUpdate.JsicontactName = FilingMasterDraft.JsicontactName;
                rowsToUpdate.JsicontactEmail = FilingMasterDraft.JsicontactEmail;
                rowsToUpdate.UpdateDate = FilingMasterDraft.UpdateDate;
                rowsToUpdate.UpdateUser = FilingMasterDraft.UpdateUser;
                rowsToUpdate.Juristiction = FilingMasterDraft.Juristiction;
                rowsToUpdate.Notes = FilingMasterDraft.Notes;
                rowsToUpdate.ChangesInprogress = false;
                }
            if(FilingMasterDraft!=null)
                FilingMasterDraft.ChangesInprogress = false;
            _db.SaveChanges();
            return Ok();
        }
        [HttpPut("FilingMasterReject{WorkflowId:Int}")]
        public IActionResult FilingMasterReject(int WorkflowId, string Userid, [FromBody] FilingMasterDraft FilingMasterDraft)
        {

            FilingMasterWorkflow FilingMasterWorkflow = (from w in _db.FilingMasterWorkflow
                                                         where w.WorkflowId == WorkflowId
                                                         select w).FirstOrDefault();
            if (FilingMasterWorkflow == null)
            {
                return BadRequest(ModelState);
            }
         
            FilingMasterWorkflow.WorkflowStatus = "Rejected";
            FilingMasterWorkflow.UpdateDate = DateTime.Now;
            FilingMasterWorkflow.UpdateUser = Userid;
            _db.FilingMasterWorkflow.Attach(FilingMasterWorkflow);
            _db.Entry(FilingMasterWorkflow).Property(x => x.WorkflowStatus).IsModified = true;
            _db.Entry(FilingMasterWorkflow).Property(x => x.UpdateDate).IsModified = true;
            _db.Entry(FilingMasterWorkflow).Property(x => x.UpdateUser).IsModified = true;

            var rowsToUpdate = _db.FilingMaster.AsEnumerable().Where(r => r.FilingId == FilingMasterDraft.FilingId).FirstOrDefault();
            if (rowsToUpdate != null)
                rowsToUpdate.ChangesInprogress = false;
            if(FilingMasterDraft !=null)
                FilingMasterDraft.ChangesInprogress = false;
            
            _db.SaveChanges();
            return Ok();
        }
        [HttpGet("FilingMasterList")]
        public IActionResult FilingMasterList()
        {
            return Ok((from o in _db.FilingMaster
                       select new
                       {
                           FilingID = o.FilingId,
                           FilingName=o.FilingName,
                           FilingDescription = o.FilingDescription,
                           FilingFrequency = o.FilingFrequency,
                           StateInfo = o.StateInfo,
                           RuleInf = o.RuleInfo,
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
                           ChangesInprogress = o.ChangesInprogress
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
                           FilingID = o.FilingId,
                           FilingName = o.FilingName,
                           FilingDescription = o.FilingDescription,
                           FilingFrequency = o.FilingFrequency,
                           StateInfo = o.StateInfo,
                           RuleInf = o.RuleInfo,
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
                           ChangesInprogress = o.ChangesInprogress
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
            _db.Update(FilingMaster);
            _db.SaveChanges();

            return Ok(FilingMaster);

        }
        [HttpDelete("FilingMasterDelete{FilingId:Int}")]
        public void FilingMasterDelete(int FilingId)
        {
            FilingMaster FilingMaster;

            FilingMaster = _db.FilingMaster.Where(d => d.FilingId == FilingId).First();
            _db.FilingMaster.Remove(FilingMaster);
            _db.SaveChanges();

        }
        [HttpPost("CreateFilingMasterHistory")]
        public IActionResult CreateFilingMasterHistory([FromBody] FilingMasterHistory FilingMasterHistory)
        {

            _db.Add(FilingMasterHistory);
            _db.SaveChanges();

            return Ok(FilingMasterHistory);
        }
        [HttpPost("CreateDraftFilingMaster")]
        public IActionResult CreateDraftFilingMaster([FromBody] FilingMasterDraft FilingMasterDraft)
        {

            _db.Add(FilingMasterDraft);
            _db.SaveChanges();

            return Ok(FilingMasterDraft);
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
        public IActionResult CreateFilingMasterWorkflow([FromBody] FilingMasterWorkflow FilingMasterWorkflow)
        {
            FilingMasterWorkflow.CurrentApproverID = (from a in _db.Approvers
                                                      join c in _db.ApproverConfiguration on a.ApproverGroupID equals c.ApproverGroupID
                                                      join f in _db.FilingMaster on a.State equals f.StateInfo
                                                      where c.FilingType != null && c.FilingType.Equals("MasterFiling") && a.Isdefault == true
                                                      select a.ApproverID).FirstOrDefault();
            _db.Add(FilingMasterWorkflow);
            _db.SaveChanges();

            return Ok(FilingMasterWorkflow);
        }
        [HttpGet("FilingMasterWorkflowList")]
        public IActionResult FilingMasterWorkflowList()
        {
            return Ok((from o in _db.FilingMasterDraft
                       join c in _db.FilingMasterWorkflow on o.DraftId equals c.DraftId
                       join s in _db.Approvers on c.CurrentApproverID equals s.ApproverID
                       
                       select new
                       {
                           WorkflowId = c.WorkflowId,
                           DraftId = c.DraftId,
                           FilingID = o.FilingId,
                           FilingName = o.FilingName,
                           FilingDescription = o.FilingDescription,
                           FilingFrequency = o.FilingFrequency,
                           StateInfo = o.StateInfo,
                           RuleInf = o.RuleInfo,
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
                           ApproverName = s.ApproverName
                       }
                      ));

        }
        [HttpGet("FilingMasterWorkflowListByApprover{UserID:Int}")]
        public IActionResult FilingMasterWorkflowListByApprover(long UserID)
        {
            return Ok((from o in _db.FilingMasterDraft
                       join c in _db.FilingMasterWorkflow on o.DraftId equals c.DraftId
                       join s in _db.Approvers on c.CurrentApproverID equals s.ApproverID
                       where s.ApproverID== UserID && o.ChangesInprogress==true
                       select new
                       {
                           WorkflowId = c.WorkflowId,
                           DraftId = c.DraftId,
                           FilingID = o.FilingId,
                           FilingName = o.FilingName,
                           FilingDescription = o.FilingDescription,
                           FilingFrequency = o.FilingFrequency,
                           StateInfo = o.StateInfo,
                           RuleInf = o.RuleInfo,
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
                           ApproverName = s.ApproverName
                       }
                       ));

        }
        [HttpGet("FilingMasterWorkflowList{WorkflowId:Int}")]
        public IActionResult FilingMasterWorkflowList(int WorkflowId)
        {
            FilingMasterWorkflow FilingMasterWorkflow = _db.FilingMasterWorkflow.FirstOrDefault(x => x.WorkflowId == WorkflowId);

            return Ok(FilingMasterWorkflow);

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

        [HttpPut("FilingMasterCommentsUpdate{CommentsID:Int}")]
        public IActionResult FilingMasterCommentsUpdate(int CommentsID, [FromBody] FilingMasterComments FilingMasterComments)
        {
            if (FilingMasterComments == null || FilingMasterComments.CommentsID != CommentsID)
            {
                return BadRequest(ModelState);
            }
            _db.Update(FilingMasterComments);
            _db.SaveChanges();
            return Ok(FilingMasterComments);
        }
        [HttpPut("FilingMasterDraftCommentsUpdate{CommentsID:Int}")]
        public IActionResult FilingMasterDraftCommentsUpdate(int CommentsID, [FromBody] FilingMasterDraftComments FilingMasterDraftComments)
        {
            if (FilingMasterDraftComments == null || FilingMasterDraftComments.CommentsID != CommentsID)
            {
                return BadRequest(ModelState);
            }
            _db.Update(FilingMasterDraftComments);
            _db.SaveChanges();
            return Ok(FilingMasterDraftComments);
        }
        [HttpPut("FilingMasterWorkflowCommentsCommentsUpdate{CommentsID:Int}")]
        public IActionResult FilingMasterWorkflowCommentsCommentsUpdate(int CommentsID, [FromBody] FilingMasterWorkflowComments FilingMasterWorkflowComments)
        {
            if (FilingMasterWorkflowComments == null || FilingMasterWorkflowComments.CommentsID != CommentsID)
            {
                return BadRequest(ModelState);
            }
            _db.Update(FilingMasterWorkflowComments);
            _db.SaveChanges();
            return Ok(FilingMasterWorkflowComments);
        }

        [HttpGet("FilingMasterComments{FilingID:Int}")]
        public IActionResult FilingMasterComments(int FilingID)
        {
            return Ok((from o in _db.FilingMasterComments
                       where o.FilingID == FilingID
                       select new
                       {
                           CommentsID = o.CommentsID,
                           FilingID=o.FilingID,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser

                       }));

        }
        [HttpGet("FilingMasterWorkflowComments{WorkflowID:Int}")]
        public IActionResult FilingMasterWorkflowComments(int WorkflowID)
        {
            return Ok((from o in _db.FilingMasterWorkflowComments
                       where o.WorkflowID == WorkflowID
                       select new
                       {
                           CommentsID = o.CommentsID,
                           WorkflowID = o.WorkflowID,
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
        public IActionResult FilingMasterDraftComments(int DraftID)
        {
            return Ok((from o in _db.FilingMasterDraftComments
                       where o.DraftID == DraftID
                       select new
                       {
                           CommentsID = o.CommentsID,
                           DraftID = o.DraftID,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }

        [HttpGet("FilingMasterCommentsbyID{CommentsID:Int}")]
        public IActionResult FilingMasterCommentsbyID(int CommentsID)
        {
            return Ok((from o in _db.FilingMasterComments
                       where o.CommentsID == CommentsID
                       select new
                       {
                           CommentsID = o.CommentsID,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser

                       }));

        }
        [HttpGet("FilingMasterWorkflowCommentsbyID{CommentsID:Int}")]
        public IActionResult FilingMasterWorkflowCommentsbyID(int CommentsID)
        {
            return Ok((from o in _db.FilingMasterWorkflowComments
                       where o.CommentsID == CommentsID
                       select new
                       {
                           CommentsID = o.CommentsID,
                           WorkflowID = o.WorkflowID,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }
        [HttpGet("FilingMasterDraftCommentsbyID{CommentsID:Int}")]
        public IActionResult FilingMasterDraftCommentsbyID(int CommentsID)
        {
            return Ok((from o in _db.FilingMasterDraftComments
                       where o.CommentsID == CommentsID
                       select new
                       {
                           CommentsID = o.CommentsID,
                           DraftID = o.DraftID,
                           CommentsText = o.CommentsText,
                           InformationRead = o.InformationRead,
                           InformationDeleted = o.InformationDeleted,
                           CreateDate = o.CreateDate,
                           UpdateDate = o.UpdateDate,
                           CreateUser = o.CreateUser,
                           UpdateUser = o.UpdateUser
                       }));

        }

        [HttpDelete("FilingMasterCommentssDelete{CommentsID:Int}")]
        public void FilingMasterCommentssDelete(int CommentsID)
        {
            FilingMasterComments FilingMasterComments;

            FilingMasterComments = _db.FilingMasterComments.Where(d => d.CommentsID == CommentsID).First();
            _db.FilingMasterComments.Remove(FilingMasterComments);
            _db.SaveChanges();

        }
        [HttpDelete("CustomerFilingCommentsDelete{CommentId:Int}")]
        public void CustomerFilingCommentsDelete(int CommentsID)
        {
            FilingMasterDraftComments FilingMasterDraftComments;

            FilingMasterDraftComments = _db.FilingMasterDraftComments.Where(d => d.CommentsID == CommentsID).First();
            _db.FilingMasterDraftComments.Remove(FilingMasterDraftComments);
            _db.SaveChanges();

        }
        [HttpDelete("FilingMasterWorkflowCommentsDelete{CommentId:Int}")]
        public void FilingMasterWorkflowCommentsDelete(int CommentsID)
        {
            FilingMasterWorkflowComments FilingMasterWorkflowComments;

            FilingMasterWorkflowComments = _db.FilingMasterWorkflowComments.Where(d => d.CommentsID == CommentsID).First();
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

    }
}
