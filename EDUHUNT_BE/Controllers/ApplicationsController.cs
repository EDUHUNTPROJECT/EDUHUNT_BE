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
    public class ApplicationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApplicationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Applications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
            return await _context.Applications.ToListAsync();
        }

        // GET: api/Applications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Application>> GetApplication(Guid id)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            return application;
        }

        // PUT: api/Applications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(Guid id, Application application)
        {
            if (id != application.Id)
            {
                return BadRequest();
            }

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
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

        // POST: api/Applications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Application>> PostApplication(Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplication", new { id = application.Id }, application);
        }

        // DELETE: api/Applications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(Guid id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // GET: api/Applications/ScholarshipProvider/{scholarshipProviderId}
        [HttpGet("ScholarshipProvider/{scholarshipProviderId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetApplicationsByScholarshipProvider(string scholarshipProviderId)
        {
            var applicationsWithScholarshipInfo = await _context.Applications
                .Join(_context.ScholarshipInfos, // The table we are joining with
                      application => application.ScholarshipID, // Foreign key in Applications
                      scholarshipInfo => scholarshipInfo.Id, // Primary key in ScholarshipInfo
                      (application, scholarshipInfo) => new { Application = application, ScholarshipInfo = scholarshipInfo }) // Result selector
                .Where(result => result.ScholarshipInfo.AuthorId == scholarshipProviderId) // Filter by ScholarshipProviderId
                .Select(result => new
                {
                    // Application details
                    ApplicationId = result.Application.Id,
                    StudentID = result.Application.StudentID,
                    ScholarshipID = result.Application.ScholarshipID,
                    StudentCV = result.Application.StudentCV,
                    Status = result.Application.Status,
                    MeetingURL = result.Application.MeetingURL,
                    StudentAvailableStartDate = result.Application.StudentAvailableStartDate,
                    StudentAvailableEndDate = result.Application.StudentAvailableEndDate,
                    ScholarshipProviderAvailableStartDate = result.Application.ScholarshipProviderAvailableStartDate,
                    ScholarshipProviderAvailableEndDate = result.Application.ScholarshipProviderAvailableEndDate,
                    ApplicationReason = result.Application.ApplicationReason,

                    // ScholarshipInfo details
                    ScholarshipTitle = result.ScholarshipInfo.Title,
                    ScholarshipBudget = result.ScholarshipInfo.Budget,
                    ScholarshipLocation = result.ScholarshipInfo.Location,
                    SchoolName = result.ScholarshipInfo.SchoolName,
                    Description = result.ScholarshipInfo.Description,
                    CategoryId = result.ScholarshipInfo.CategoryId,
                    IsInSite = result.ScholarshipInfo.IsInSite,
                    Url = result.ScholarshipInfo.Url,
                    CreatedAt = result.ScholarshipInfo.CreatedAt,
                    IsApproved = result.ScholarshipInfo.IsApproved,
                    ImageUrl = result.ScholarshipInfo.ImageUrl
                })
                .ToListAsync();

            if (!applicationsWithScholarshipInfo.Any())
            {
                return NotFound();
            }

            return applicationsWithScholarshipInfo;
        }



        private bool ApplicationExists(Guid id)
        {
            return _context.Applications.Any(e => e.Id == id);
        }
    }
}
