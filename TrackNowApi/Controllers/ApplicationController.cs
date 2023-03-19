using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
        public IActionResult AppConfigurationCreate([FromBody] AppConfiguration item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            _db.AppConfiguration.Add(item);
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
    }
    

}
