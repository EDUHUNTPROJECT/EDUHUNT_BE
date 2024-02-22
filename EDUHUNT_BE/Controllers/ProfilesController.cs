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
    public class ProfilesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfilesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profile>>> GetProfile()
        {
            return await _context.Profile.ToListAsync();
        }

        // GET: api/Profiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profile>> GetProfile(Guid id)
        {
            var profile = await _context.Profile.FirstOrDefaultAsync(p => p.UserId == id);

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }


        // PUT: api/Profiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(Guid id, Profile profile)
        {
            
            if (id != profile.Id)
            {
                return BadRequest();
            }
            var currentProfile = await _context.Profile.FirstOrDefaultAsync(p => p.Id == id);

            if (currentProfile.ContentURL != profile.ContentURL)
            {
                currentProfile.ContentURL = profile.ContentURL;
            }
            if (currentProfile.FirstName != profile.FirstName)
            {
                currentProfile.FirstName = profile.FirstName;
            }
            if (currentProfile.LastName != profile.LastName)
            {
                currentProfile.LastName = profile.LastName;
            }
            if (currentProfile.UserName != profile.UserName)
            {
                currentProfile.UserName = profile.UserName;
            }
            if (currentProfile.ContactNumber != profile.ContactNumber)
            {
                currentProfile.ContactNumber = profile.ContactNumber;
            }
            if (currentProfile.Address != profile.Address)
            {
                currentProfile.Address = profile.Address;
            }
            if (currentProfile.Description != profile.Description)
            {
                currentProfile.Description = profile.Description;
            }
            if (currentProfile.UrlAvatar != profile.UrlAvatar)
            {
                currentProfile.UrlAvatar = profile.UrlAvatar;
            }

            try
            {

            _context.Entry(currentProfile).State = EntityState.Modified;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
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

        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Profile>> PostProfile(Profile profile)
        {
            _context.Profile.Add(profile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfile", new { id = profile.Id }, profile);
        }

        // POST: api/UploadCV
        [HttpPost("UploadCV")]
        public async Task<ActionResult<CV>> UploadCV(CV cV)
        {
            var cv = await _context.CVs.FirstOrDefaultAsync(c => c.UserId == cV.UserId);

            if (string.IsNullOrWhiteSpace(cV.UrlCV))
            {
                if (cv == null)
                {
                    return NotFound("No CV found for the user");
                }
                else
                {
                    return Ok(cv);
                }
            }
            else
            {
                if (cv == null)
                {
                    var newCv = new CV
                    {
                        UserId = cV.UserId,
                        UrlCV = cV.UrlCV
                    };

                    _context.CVs.Add(newCv);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetCV", new { id = newCv.Id }, newCv);
                }
                else
                {
                    cv.UrlCV = cV.UrlCV;
                    _context.Entry(cv).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CVExists(cv.Id))
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
            }
        }

        private bool CVExists(Guid id)
        {
            return _context.CVs.Any(e => e.Id == id);
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(Guid id)
        {
            var profile = await _context.Profile.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.Profile.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfileExists(Guid id)
        {
            return _context.Profile.Any(e => e.Id == id);
        }
    }
}
