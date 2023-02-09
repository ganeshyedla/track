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
        public IActionResult CreateFilingMaster([FromBody] FilingMaster FilingMaster)
        {

            _db.Add(FilingMaster);
            _db.SaveChanges();

            return Ok(FilingMaster);
        }
        [HttpGet("FilingMasterList")]
        public IActionResult FilingMasterList()
        {
            List<FilingMaster> FilingMaster = new List<FilingMaster>();
            FilingMaster = _db.FilingMaster.ToList();

            return Ok(FilingMaster);

        }
        [HttpGet("FilingMasterList{FilingId:Int}")]
        public IActionResult FilingMasterList(int FilingId)
        {
            FilingMaster FilingMaster = _db.FilingMaster.FirstOrDefault(x => x.FilingId == FilingId);

            return Ok(FilingMaster);

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
        //[HttpDelete("FilingMasterDelete{FilingId:Int}")]
        //public void Delete(int FilingId)
        //{
        //    _db.FilingMaster.Remove(FilingId);
            
        //}
    }
}
