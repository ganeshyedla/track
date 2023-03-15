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

    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ApplicationController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet("UserRoleList")]
        public IActionResult UserRoleList()
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
        [HttpGet("UserRoleList{UserId:Int}")]
        public IActionResult UserRoleList(int UserId)
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
        public IActionResult CustomerUpdate(int BusinessCategoryId, [FromBody] BusinessCategoryMaster BusinessCategoryMaster)
        {

            if (BusinessCategoryMaster == null || BusinessCategoryMaster.BusinessCategoryId != BusinessCategoryId)
            {
                return BadRequest(ModelState);
            }

            _db.Update(BusinessCategoryMaster);
            
            _db.SaveChanges();

            return Ok(BusinessCategoryMaster);

        }
    }
}
