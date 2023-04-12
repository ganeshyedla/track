using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Text.Json;
using TrackNowApi.Data;
using TrackNowApi.Model;
using Azure.Storage.Blobs;

namespace TrackNowApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly string Blob_connectionString;

        public ApplicationController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            Blob_connectionString = configuration.GetConnectionString("Blob_connectionString");
        }

        [HttpGet("UserRole")]
        public IActionResult UserRole()
        {
            return Ok((from r in _db.Roles
                       join ur in _db.UsersRoles on r.RoleId equals ur.RoleId
                       join u in _db.Users on ur.UserId equals u.UserId
                       select new
                       {
                           UserId = u.UserId,
                           UserName = u.UserName,
                           LoginId = u.LoginId,
                           RoleId = r.RoleId,
                           RoleName = r.RoleName
                       }
                    )
             );
        }
        [HttpGet("UserRole{UserId:Int}")]
        public IActionResult UserRole(int UserId)
        {
            return Ok((from r in _db.Roles
                       join ur in _db.UsersRoles on r.RoleId equals ur.RoleId
                       join u in _db.Users on ur.UserId equals u.UserId
                       where u.UserId == UserId
                       select new
                       {
                           UserId = u.UserId,
                           UserName = u.UserName,
                           LoginId = u.LoginId,
                           RoleId = r.RoleId,
                           RoleName = r.RoleName
                       }
                    )
             );

        }
        [HttpPost("BusinessCategoryMaster")]
        public IActionResult CreateBusinessCategoryMaster([FromBody] BusinessCategoryMaster[] BusinessCategoryMaster)
        {
            foreach (BusinessCategoryMaster Bc in BusinessCategoryMaster)
            {
                _db.Add(Bc);
            }

            _db.SaveChanges();
            return Ok(BusinessCategoryMaster);
        }
        [HttpGet("BusinessCategoryMasterList")]
        public IActionResult BusinessCategoryMasterList()
        {
            return Ok(from o in _db.BusinessCategoryMaster
                      select new
                      {
                          BusinessCategoryId = o.BusinessCategoryId,
                          BusinessCategoryName = o.BusinessCategoryName,
                          BusinessCategoryDescription = o.BusinessCategoryDescription,
                          CreatedDate = o.CreatedDate,
                          CreatedUser = o.CreatedUser,
                          UpdateDate = o.UpdateDate,
                          UpdateUser = o.UpdateUser
                      }
                        );
        }
        [HttpGet("BusinessCategoryMasterList{BusinessCategoryId:Int}")]
        public IActionResult BusinessCategoryMasterList(int BusinessCategoryId)
        {
            return Ok(from o in _db.BusinessCategoryMaster
                      where o.BusinessCategoryId == BusinessCategoryId
                      select new
                      {
                          BusinessCategoryId = o.BusinessCategoryId,
                          BusinessCategoryName = o.BusinessCategoryName,
                          BusinessCategoryDescription = o.BusinessCategoryDescription,
                          CreatedDate = o.CreatedDate,
                          CreatedUser = o.CreatedUser,
                          UpdateDate = o.UpdateDate,
                          UpdateUser = o.UpdateUser
                      });

        }
        [HttpDelete("BusinessCategoryMaster{BusinessCategoryId:Int}")]
        public APIStatusJSON DeleteBusinessCategoryMaster(int BusinessCategoryId)
        {
            try { 
                BusinessCategoryMaster BusinessCategoryMaster = _db.BusinessCategoryMaster.Where(d => d.BusinessCategoryId == BusinessCategoryId).FirstOrDefault();

                if (BusinessCategoryMaster != null)
                {
                    var CustomerBusinessCategory = _db.CustomerBusinessCategory.Where(d => d.BusinessCategoryId == BusinessCategoryId).FirstOrDefault();
                    var FilingBusinessCategory = _db.FilingBusinessCategory.Where(d => d.BusinessCategoryId == BusinessCategoryId).FirstOrDefault();
                    var FilingDraftBusinessCategory = (
                        from d in _db.FilingMasterDraft
                        join c in _db.FilingDraftBusinessCategory on d.DraftId equals c.DraftId
                        join f in _db.FilingMaster on d.FilingId equals f.FilingId
                        where f.ChangesInprogress == true &&  c.BusinessCategoryId == BusinessCategoryId
                        select d).FirstOrDefault(); 
                    ;
                    var CustomerDraftBusinessCategory = (
                        from d in _db.CustomerFilingMasterDraft
                        join c in _db.CustomerDraftBusinessCategory on d.DraftId equals c.DraftId
                        where d.Status == "Pending" && c.BusinessCategoryId == BusinessCategoryId
                        select d).FirstOrDefault();

                    if (CustomerBusinessCategory==null && FilingBusinessCategory==null && FilingDraftBusinessCategory==null && CustomerDraftBusinessCategory==null)
                    { 
                        _db.BusinessCategoryMaster.Remove(BusinessCategoryMaster);
                        _db.SaveChanges();
                        return new APIStatusJSON { Status = "Success" };
                    }
                    else
                    {
                        return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Business Category Master in used in some table" };
                    }
                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Business Category Master Not found" };
                }
            }
            catch(Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        [HttpPut("BusinessCategoryMasterUpdate{BusinessCategoryId:Int}")]
        public IActionResult BusinessCategoryMasterUpdate(int BusinessCategoryId, [FromBody] BusinessCategoryMaster BusinessCategoryMaster)
        {

            if (BusinessCategoryMaster == null || BusinessCategoryMaster.BusinessCategoryId != BusinessCategoryId)
            {
                return BadRequest(ModelState);
            }

            _db.Update(BusinessCategoryMaster);

            _db.SaveChanges();

            return Ok(BusinessCategoryMaster);

        }

        [HttpGet("AppConfigurationList")]
        public IActionResult AppConfigurationList()
        {
            var AppConfiguration = _db.AppConfiguration.ToList();
            return Ok(AppConfiguration);
        }

        [HttpPost("AppConfigurationCreate")]
        public IActionResult AppConfigurationCreate([FromBody] AppConfiguration []item)
        {
            foreach (AppConfiguration Bc in item)
            {
                _db.Add(Bc);
            }
            _db.SaveChanges();
            return Ok(item);
        }

        [HttpGet("AppConfigurationbyid/{ConfigId:int}")]
        public ActionResult<AppConfiguration> AppConfigurationGetById(int ConfigId)
        {
            var res = _db.AppConfiguration.FirstOrDefault(p => p.ConfigId == ConfigId);
            if (res != null)
            {
                return Ok(res);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("AppConfigurationupdate/{ConfigId}")]
        public IActionResult AppConfigurationUpdate(int ConfigId, AppConfiguration updatedFilingMaster)
        {
            var FilingMaster = _db.AppConfiguration.FirstOrDefault(p => p.ConfigId == ConfigId); if (FilingMaster == null)
            {
                return NotFound();
            }
            FilingMaster.UpdateUser = updatedFilingMaster.UpdateUser;
            _db.SaveChanges(); return Ok();
        }

        [HttpDelete("AppConfigurationdelete/{ConfigId:int}")]
        public IActionResult AppConfiguration(int ConfigId)
        {
            var res = _db.AppConfiguration.FirstOrDefault(t => t.ConfigId == ConfigId);
            if (res == null)
            {
                return NotFound();
            }
            _db.AppConfiguration.Remove(res);
            _db.SaveChanges();
            return new NoContentResult();
        }



        [HttpPost("CreateApproverConfiguration")]
        public IActionResult CreateApproverConfiguration(ApproverConfiguration Approver)
        {
            try
            {
                _db.ApproverConfiguration.Add(Approver);
                _db.SaveChanges();
                return Ok(Approver);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ViewApproverConfiguration/{ApproverConfigId:int}")]
        public IActionResult ViewApproverConfigurations(int ApproverConfigId)
        {
            try
            {
                var ApproverConfiguration = _db.ApproverConfiguration
                                               .FirstOrDefault(n => n.ApproverConfigId == ApproverConfigId);

                if (ApproverConfiguration != null)
                {
                    return Ok(ApproverConfiguration);
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

        [HttpGet("ListApproverConfiguration")]
        public IActionResult ListApproverConfiguration()
        {
            try
            {
                var ApproverConfiguration = _db.ApproverConfiguration.ToList();
                return Ok(ApproverConfiguration);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("DeleteApproverConfiguration/{ApproverConfigId:int}")]
        public IActionResult DeleteApproverConfiguration(int ApproverConfigId)
        {
            try
            {

                var ApproverConfiguration = _db.ApproverConfiguration
                                            .FirstOrDefault(n => n.ApproverConfigId == ApproverConfigId);

                if (ApproverConfiguration != null)
                {
                    _db.ApproverConfiguration.Remove(ApproverConfiguration);
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
        [HttpPut("UpdateApproverConfiguration/{ApproverConfigId:int}")]
        public IActionResult UpdateApproverConfigurations(int ApproverConfigId, [FromBody] ApproverConfiguration ApproverConfiguration)
        {
            try
            {

                var existingNotification = _db.ApproverConfiguration.
                                      FirstOrDefault(n => n.ApproverConfigId == ApproverConfigId);

                if (existingNotification != null)

                {
                    existingNotification.ApproverLevel = ApproverConfiguration.ApproverLevel;
                    existingNotification.ApproverGroupId = ApproverConfiguration.ApproverGroupId;
                    existingNotification.FilingType = ApproverConfiguration.FilingType;
                    existingNotification.CreateDate = ApproverConfiguration.CreateDate;
                    existingNotification.CreateUser = ApproverConfiguration.CreateUser;
                    existingNotification.UpdateDate = ApproverConfiguration.UpdateDate;
                    existingNotification.UpdateUser = ApproverConfiguration.UpdateUser;

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

        //=============================================================================================================

        [HttpPost("CreateApprovers")]
        public APIStatusJSON CreateApprovers(Approvers []Approver)
        {
            
            try
            {
                long MaxId = _db.Approvers.Max(u => u.Id);
                foreach (Approvers Bc in Approver)
                {
                    _db.Add(Bc);
                }
                _db.SaveChanges();
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(Approver, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("UK_Approvers"))
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Appovers already available" };
                else
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpGet("ViewApprovers/{Id:int}")]
        public APIStatusJSON ViewApproverId(int Id)
        {
            try
            {
                var Approvers = _db.Approvers.FirstOrDefault(n => n.Id == Id);

                if (Approvers != null)
                {
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(Approvers, new JsonSerializerOptions
                        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                    };
                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Approvers Not found" };
                }

            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpGet("ListApprovers")]
        public APIStatusJSON ListApprovers()
        {   try
            {
                var Approvers = ( from a in _db.Approvers 
                                  join u in _db.Users on a.ApproverId equals u.UserId
                        join c in _db.Customer on a.CustomerId equals c.CustomerId into Cus
                           from m in Cus.DefaultIfEmpty()
                           select new
                        {
                            Id = a.Id,
                            ApproverId = a.ApproverId,
                            CustomerId = a.CustomerId,
                            State       = a.State,
                            ApproverGroupId = a.ApproverGroupId,
                            Isdefault   = a.Isdefault,
                            ApproverName   = u.UserName,
                            Juristiction   = a.Juristiction,
                            CreateDate  = a.CreateDate,
                            CreateUser = a.CreateUser,
                            UpdateDate = a.UpdateDate,
                            UpdateUser = a.UpdateUser,
                            CustomerName = m.CustomerName,
                            FilingType = a.FilingType
                        } );

                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(Approvers, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        [HttpDelete("DeleteApprovers/{Id:int}")]
        public APIStatusJSON DeleteApprovers(int Id)
        {
            try
            {
                var Approvers = _db.Approvers.FirstOrDefault(n => n.Id == Id);

                if (Approvers != null)
                {
                    _db.Approvers.Remove(Approvers);
                    _db.SaveChanges();
                    return new APIStatusJSON
                    { Status = "Success" };
                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Approver not found" };
                }
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }
        [HttpPut("UpdateApprovers")]
        public APIStatusJSON UpdateApprovers([FromBody] Approvers Approvers)
        {
            try
            {

                var existingNotification = _db.Approvers.
                                      FirstOrDefault(n => n.Id == Approvers.Id);

                if (existingNotification != null)

                {
                    existingNotification.ApproverId = Approvers.ApproverId;
                    existingNotification.CustomerId = Approvers.CustomerId;
                    existingNotification.State = Approvers.State;
                    existingNotification.ApproverGroupId = Approvers.ApproverGroupId;
                    existingNotification.Isdefault = Approvers.Isdefault;
                    existingNotification.Juristiction = Approvers.Juristiction;
                    existingNotification.CreateDate = Approvers.CreateDate;
                    existingNotification.CreateUser = Approvers.CreateUser;
                    existingNotification.UpdateDate = Approvers.UpdateDate;
                    existingNotification.UpdateUser = Approvers.UpdateUser;

                    _db.SaveChanges();
                    return new APIStatusJSON
                    {
                        Status = "Success",
                        Data = JsonDocument.Parse(JsonSerializer.Serialize(Approvers, new JsonSerializerOptions
                        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                    };

                }
                else
                {
                    return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Approver not found" };
                }
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }

        }

        [HttpPost("[action]")]
        public APIStatusJSON CustomerFilingUpload([FromForm] CustomerFilingUpload request)
        {
            try
            {
                BlobContainerClient blobContainerClient = new BlobContainerClient(Blob_connectionString, "jsitracknow");

                List<azzureupload> uploadedFiles = new List<azzureupload>();

                foreach (IFormFile file in request.Files)
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;

                        var blobName = file.FileName;
                        var folderPath = $"Customer/{request.CustomerId}/";

                        if (!string.IsNullOrEmpty(request.CommentId))
                        {
                            folderPath += $"Comments/CommentID:{request.CommentId}/";
                        }
                        else if (!string.IsNullOrEmpty(request.FilingId))
                        {
                            folderPath += $"Filing/{request.FilingId}/";
                        }
                        else if (!string.IsNullOrEmpty(request.WorkflowId))
                        {
                            folderPath += $"WorkFlow/{request.WorkflowId}/";
                        }
                        else if (!string.IsNullOrEmpty(request.WorkflowId))
                        {
                            folderPath += $"WorkFlow/{request.WorkflowId}/";
                        }
                        else if (!string.IsNullOrEmpty(request.FileTrackingId))
                        {
                            folderPath += $"FileTracking/{request.FileTrackingId}/";
                        }
                        else if (!string.IsNullOrEmpty(request.DraftId))
                        {
                            folderPath += $"Draft/{request.DraftId}/";
                        }
                        
                        if (!string.IsNullOrEmpty(request.AttachmentId))
                        {
                            folderPath += $"AttachmentID:{request.AttachmentId}/";
                        }
                        var blobClient = blobContainerClient.GetBlobClient($"{folderPath}{blobName}");
                        blobClient.Upload(stream);

                        var blobUrl = blobClient.Uri.AbsoluteUri;
                        var blobUrlFormatted = $"{blobUrl.Substring(0, blobUrl.LastIndexOf("/"))}/{blobName}";

                        uploadedFiles.Add(new azzureupload { FileName = blobName, Message = $"File '{blobName}' uploaded successfully." });
                        Response.Headers.Add("X-File-URL", blobUrlFormatted);
                    }
                }
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(uploadedFiles, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (RequestFailedException ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Upload request failed" };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpPost("[action]")]
        public APIStatusJSON FilingmasterUpload([FromForm] FilingUploadFilesRequest request)
        {
            try
            {
                BlobContainerClient blobContainerClient = new BlobContainerClient(Blob_connectionString, "jsitracknow");

                List<azzureupload> uploadedFiles = new List<azzureupload>();

                foreach (IFormFile file in request.Files)
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;

                        var blobName = file.FileName;
                        var folderPath = $"FilingMaster/{request.FilingId}/";

                        if (!string.IsNullOrEmpty(request.CommentId))
                        {
                            folderPath += $"Comments/CommentID:{request.CommentId}/";
                        }
                        else if (!string.IsNullOrEmpty(request.WorkflowId))
                        {
                            folderPath += $"WorkFlow/{request.WorkflowId}/";
                        }
                        else if (!string.IsNullOrEmpty(request.DraftId))
                        {
                            folderPath += $"Draft/{request.DraftId}/";
                        }
                        if (!string.IsNullOrEmpty(request.AttachmentId))
                        {
                            folderPath += $"AttachmentID:{request.AttachmentId}/";
                        }
                        var blobClient = blobContainerClient.GetBlobClient($"{folderPath}{blobName}");
                        blobClient.Upload(stream);

                        var blobUrl = blobClient.Uri.AbsoluteUri;
                        var blobUrlFormatted = $"{blobUrl.Substring(0, blobUrl.LastIndexOf("/"))}/{blobName}";

                        uploadedFiles.Add(new azzureupload { FileName = blobName, Message = $"File '{blobName}' uploaded successfully." });
                        Response.Headers.Add("X-File-URL", blobUrlFormatted);
                    }
                }
                return new APIStatusJSON
                {
                    Status = "Success",
                    Data = JsonDocument.Parse(JsonSerializer.Serialize(uploadedFiles, new JsonSerializerOptions
                    { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                };
            }
            catch (RequestFailedException ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = "Upload request failed" };
            }
            catch (Exception ex)
            {
                return new APIStatusJSON { Status = "Failure", ErrorCode = 1, ErrorMessage = ex.Message };
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DownloadFile([FromQuery] DownloadFileRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.FileUrl))
                {
                    // Download the file using the URL
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(request.FileUrl);
                        var content = await response.Content.ReadAsStreamAsync();

                        return new FileStreamResult(content, "application/octet-stream");
                    }
                }
                else
                {
                    return BadRequest("File URL cannot be null or empty.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



    }
}




