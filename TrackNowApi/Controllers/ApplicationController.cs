using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Net;
using TrackNowApi.Data;
using TrackNowApi.Model;

namespace TrackNowApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ApplicationController(ApplicationDbContext db)
        {
            _db = db;
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
        public void DeleteBusinessCategoryMaster(int BusinessCategoryId)
        {
            BusinessCategoryMaster BusinessCategoryMaster = _db.BusinessCategoryMaster.Where(d => d.BusinessCategoryId == BusinessCategoryId).First();
            _db.BusinessCategoryMaster.Remove(BusinessCategoryMaster);
            _db.SaveChanges();
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
        public IActionResult CreateApprovers(Approvers []Approver)
        {
            try
            {
                foreach (Approvers Bc in Approver)
                {
                    _db.Add(Bc);
                }
                _db.SaveChanges();
                return Ok(Approver);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ViewApprovers/{ApproverId:int}")]
        public IActionResult ViewApproverId(int ApproverId)
        {
            try
            {
                var Approvers = _db.Approvers
                                               .FirstOrDefault(n => n.ApproverId == ApproverId);

                if (Approvers != null)
                {
                    return Ok(Approvers);
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

        [HttpGet("ListApprovers")]
        public IActionResult ListApprovers()
        {
            try
            {
                var Approvers = _db.Approvers.ToList();
                return Ok(Approvers);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("DeleteApprovers/{ApproverId:int}")]
        public IActionResult DeleteApprovers(int ApproverId)
        {
            try
            {

                var Approvers = _db.Approvers
                                            .FirstOrDefault(n => n.ApproverId == ApproverId);

                if (Approvers != null)
                {
                    _db.Approvers.Remove(Approvers);
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
        [HttpPut("UpdateApproverConfiguration/{ApproverId:int}")]
        public IActionResult UpdateApprovers(int ApproverId, [FromBody] Approvers Approvers)
        {
            try
            {

                var existingNotification = _db.Approvers.
                                      FirstOrDefault(n => n.ApproverId == ApproverId);

                if (existingNotification != null)

                {
                    existingNotification.ApproverId = Approvers.ApproverId;
                    existingNotification.CustomerId = Approvers.CustomerId;
                    existingNotification.State = Approvers.State;
                    existingNotification.ApproverGroupId = Approvers.ApproverGroupId;
                    existingNotification.Isdefault = Approvers.Isdefault;
                    existingNotification.ApproverName = Approvers.ApproverName;
                    existingNotification.Juristiction = Approvers.Juristiction;
                    existingNotification.CreateDate = Approvers.CreateDate;
                    existingNotification.CreateUser = Approvers.CreateUser;
                    existingNotification.UpdateDate = Approvers.UpdateDate;
                    existingNotification.UpdateUser = Approvers.UpdateUser;



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


        //public static IWebHostEnvironment _webHostEnvironment;

        //public ApplicationController(IWebHostEnvironment webHostEnvironment)
        //{
        //    _webHostEnvironment = webHostEnvironment;
        //}

      


        [HttpPost("upload")]
        public ActionResult<string> Upload(IFormFile file, string folderPath)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var targetPath = Path.Combine(folderPath, fileName);
                    using (var fileStream = new FileStream(targetPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    return Ok(targetPath);
                }
                else
                {
                    return BadRequest("File is null or empty.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


       

        [HttpGet("{fileName}")]
        public IActionResult Download(string fileName, string folderPath)
        {
            // string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            string filePath = string.Empty;



            string[] filePaths = Directory.GetFiles(folderPath, fileName);
            if (filePaths != null)
            {
                //var targetPath = Path.Combine(folderPath, fileName);
                filePath = Path.Combine(folderPath, fileName);
            }

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }



            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/octet-stream", fileName);
        }








    }
    }




