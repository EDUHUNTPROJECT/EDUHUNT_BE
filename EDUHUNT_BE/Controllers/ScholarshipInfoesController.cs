using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ScholarshipInfoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ScholarshipInfoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ScholarshipInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScholarshipInfo>>> GetScholarshipInfos()
        {
            return await _context.ScholarshipInfos.ToListAsync();
        }

        // GET: api/ScholarshipInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScholarshipInfo>> GetScholarshipInfo(Guid id)
        {
            var scholarshipInfo = await _context.ScholarshipInfos.FindAsync(id);

            if (scholarshipInfo == null)
            {
                return NotFound();
            }

            return scholarshipInfo;
        }

        // PUT: api/ScholarshipInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScholarshipInfo(Guid id, ScholarshipInfo scholarshipInfo)
        {
            if (id != scholarshipInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(scholarshipInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScholarshipInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ScholarshipInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ScholarshipInfo>> PostScholarshipInfo(ScholarshipInfo scholarshipInfo)
        {
            scholarshipInfo.Id = Guid.NewGuid();
            scholarshipInfo.IsInSite = true;
            _context.ScholarshipInfos.Add(scholarshipInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScholarshipInfo", new { id = scholarshipInfo.Id }, scholarshipInfo);
        }

        // DELETE: api/ScholarshipInfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScholarshipInfo(Guid id)
        {
            var scholarshipInfo = await _context.ScholarshipInfos.FindAsync(id);
            if (scholarshipInfo == null)
            {
                return NotFound();
            }

            _context.ScholarshipInfos.Remove(scholarshipInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScholarshipInfoExists(Guid id)
        {
            return _context.ScholarshipInfos.Any(e => e.Id == id);
        }
    }
}
