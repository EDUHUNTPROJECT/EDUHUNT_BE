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
    public class QAsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QAsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/QAs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QA>>> GetQAs()
        {
            return await _context.QAs.ToListAsync();
        }

        // GET: api/QAs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QA>> GetQA(Guid id)
        {
            var qA = await _context.QAs.FindAsync(id);

            if (qA == null)
            {
                return NotFound();
            }

            return qA;
        }

        // PUT: api/QAs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQA(Guid id, QA qA)
        {
            if (id != qA.Id)
            {
                return BadRequest();
            }

            _context.Entry(qA).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QAExists(id))
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

        // POST: api/QAs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QA>> PostQA(QA qA)
        {
            _context.QAs.Add(qA);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQA", new { id = qA.Id }, qA);
        }

        // DELETE: api/QAs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQA(Guid id)
        {
            var qA = await _context.QAs.FindAsync(id);
            if (qA == null)
            {
                return NotFound();
            }

            _context.QAs.Remove(qA);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QAExists(Guid id)
        {
            return _context.QAs.Any(e => e.Id == id);
        }
    }
}
