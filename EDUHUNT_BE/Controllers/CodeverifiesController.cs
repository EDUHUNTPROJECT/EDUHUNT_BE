using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EDUHUNT_BE.Data;
using EDUHUNT_BE.Model;
using Microsoft.AspNetCore.Identity;

namespace EDUHUNT_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeVerifiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CodeVerifiesController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: api/CodeVerifies/Save
        [HttpPost("Save")]
        public async Task<ActionResult<CodeVerify>> CreateCodeVerify(CodeVerify codeVerify)
        {
            var user = await _userManager.FindByEmailAsync(codeVerify.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var Verify = new CodeVerify
            {
                Email = codeVerify.Email,
                UserId = user.Id,
                Code = codeVerify.Code,
                Id = Guid.NewGuid(),
                ExpirationTime = DateTime.UtcNow.AddSeconds(30),
            };

            _context.CodeVerifies.Add(Verify);
            await _context.SaveChangesAsync();

            return Ok(true);
        }

        // GET: api/CodeVerifies/Verify
        [HttpPost("Verify")]
        public async Task<ActionResult<bool>> VerifyCode(CodeVerify codeVerify)
        {
            var user = await _userManager.FindByEmailAsync(codeVerify.Email);
            var Verify = await _context.CodeVerifies.FirstOrDefaultAsync(c => c.UserId == user.Id && c.Code == codeVerify.Code);

            if (Verify == null)
            {
                return NotFound();
            }

            if (DateTime.UtcNow > Verify.ExpirationTime)
            {
                _context.CodeVerifies.Remove(Verify);
                await _context.SaveChangesAsync();
                return NotFound();
            }

            return Ok(true);
        }
    }
}
