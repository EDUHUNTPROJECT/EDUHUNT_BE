using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EDUHUNT_BE.Data;
using EDUHUNT_BE.Model;
using Microsoft.AspNetCore.Identity;

namespace EDUHUNT_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QAsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public QAsController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/QAs
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<QA>>> GetQAs()
        {
            return await _context.QAs.ToListAsync();
        }

        // GET: api/QAs/GetAllUserOrMentor/{id}
        [HttpGet("GetAllUserOrMentor/{id}")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsersOrMentors(Guid id)
        {
            try
            {
                // Retrieve the user by ID
                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user == null)
                {
                    return NotFound();
                }

                // Retrieve roles directly without using IsInRoleAsync
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.Contains("User"))
                {
                    // Retrieve mentors if the user has the "User" role
                    var mentors = await _userManager.GetUsersInRoleAsync("Mentor");
                    return Ok(mentors);
                }
                else if (userRoles.Contains("Mentor"))
                {
                    // Retrieve users if the user has the "Mentor" role
                    var users = await _userManager.GetUsersInRoleAsync("User");
                    return Ok(users);
                }
                else
                {
                    return BadRequest("Invalid role for the specified user");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/QAs/Conversations/{id}
        [HttpGet("Conversations/{id}")]
        public async Task<ActionResult<IEnumerable<QA>>> GetConversations(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
            {
                return BadRequest("Invalid user ID format");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains("User"))
            {
                var mentorQAs = await _context.QAs
                    .Where(q => q.AskerId == userId)
                    .ToListAsync();

                return Ok(mentorQAs);
            }
            else if (userRoles.Contains("Mentor"))
            {
                var userQAs = await _context.QAs
                    .Where(q => q.AnswerId == userId)
                    .ToListAsync();

                return Ok(userQAs);
            }
            else
            {
                return BadRequest("Invalid role for the specified user");
            }
        }


        // GET: api/QAs/ByUserId/
        [HttpGet("ByUserId")]
        public async Task<ActionResult<IEnumerable<QA>>> GetQAsByUserId(Guid answerId, Guid askedId)
        {
            var qAsByUser = await _context.QAs.Where(q => q.AskerId == askedId && q.AnswerId == answerId).ToListAsync();
            if (qAsByUser == null || qAsByUser.Count == 0) { return NotFound(); }
            return qAsByUser;
        }

        // GET: api/QAs/Detail/{id}
        [HttpGet("Detail/{id}")]
        public async Task<ActionResult<QA>> GetQADetail(Guid id)
        {
            var qA = await _context.QAs.FindAsync(id);

            if (qA == null)
            {
                return NotFound();
            }

            return qA;
        }

        // PUT: api/QAs/Edit/{id}
        [HttpPut("Edit/{id}")]
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

        // POST: api/QAs/Create
        [HttpPost("Create")]
        public async Task<ActionResult<QA>> PostQA(QA qA)
        {
            qA.Id = Guid.NewGuid(); // Generate a new GUID for the Id
            _context.QAs.Add(qA);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQA", new { id = qA.Id }, qA);
        }

        // DELETE: api/QAs/Delete/{id}
        [HttpDelete("Delete/{id}")]
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

        // DELETE: api/QAs/Delete/{id}
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAllQA()
        {
            var qA = await _context.QAs.ToListAsync();
            if (qA == null)
            {
                return NotFound();
            }
            foreach(var  item in qA)
            {
                _context.QAs.Remove(item);
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QAExists(Guid id)
        {
            return _context.QAs.Any(e => e.Id == id);
        }
    }
}
