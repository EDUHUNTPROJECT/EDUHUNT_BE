using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EDUHUNT_BE.Data;
using EDUHUNT_BE.Model;

namespace EDUHUNT_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoadMapsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoadMapsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/RoadMaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoadMap>>> GetRoadMap()
        {
            return await _context.RoadMaps.ToListAsync();
        }

        // GET: api/RoadMaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<RoadMap>>> GetRoadMapById(string id)
        {
            var roadMaps = await _context.RoadMaps.Where(r => r.UserId == id).ToListAsync();

            if (roadMaps == null)
            {
                return NotFound();
            }

            return roadMaps;
        }

        // PUT: api/roadmaps/5/approve
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveRoadMap(Guid id, [FromBody] bool isApproved)
        {
            var roadMap = await _context.RoadMaps.FindAsync(id);
            if (roadMap == null)
            {
                return NotFound();
            }

            if (isApproved)
            {
                roadMap.IsApproved = true;
                _context.Entry(roadMap).State = EntityState.Modified;
            }
            else
            {
                _context.RoadMaps.Remove(roadMap);
            }
            await _context.SaveChangesAsync();

            return Ok(true);
        }

        // POST: api/RoadMaps
        [HttpPost]
        public async Task<ActionResult<List<RoadMap>>> PostRoadMaps(List<RoadMap> roadMaps)
        {
            foreach (var roadMap in roadMaps)
            {
                roadMap.Id = Guid.NewGuid();
                _context.RoadMaps.Add(roadMap);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoadMap", roadMaps);
        }

        // DELETE: api/RoadMaps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoadMap(Guid id)
        {
            var roadMap = await _context.RoadMaps.FindAsync(id);
            if (roadMap == null)
            {
                return NotFound();
            }

            _context.RoadMaps.Remove(roadMap);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}