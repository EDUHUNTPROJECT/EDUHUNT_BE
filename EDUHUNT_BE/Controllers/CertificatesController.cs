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
    public class CertificatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CertificatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/certificates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Certificate>>> GetCertificate()
        {
            return await _context.Certificates.ToListAsync();
        }

        // GET: api/certificates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Certificate>> GetCertificateById(Guid id)
        {
            var certificate = await _context.Certificates.FindAsync(id);

            if (certificate == null)
            {
                return NotFound();
            }

            return certificate;
        }

        // POST: api/certificates
        [HttpPost]
        public async Task<ActionResult<Certificate>> PostCertificate(Certificate certificate)
        {
            certificate.Id = Guid.NewGuid();
            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getcertificate", new { id = certificate.Id }, certificate);
        }

        // DELETE: api/certificates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCertificate(Guid id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
            {
                return NotFound();
            }

            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}