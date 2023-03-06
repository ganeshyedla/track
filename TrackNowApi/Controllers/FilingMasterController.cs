using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("FilingMasterList")]
        public IActionResult FilingMasterList()
        {
            return Ok((from o in _db.FilingMaster
                       select new
                       {
                           FilingID = o.FilingId,
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
                       where s.ApproverID== UserID
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

    }
}
