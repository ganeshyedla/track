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

    }
}
