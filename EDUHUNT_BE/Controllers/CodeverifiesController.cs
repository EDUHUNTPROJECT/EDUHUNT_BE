using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EDUHUNT_BE.Data;
using EDUHUNT_BE.Model;
using Microsoft.AspNetCore.Identity;
using EDUHUNT_BE.Helper;

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

        [HttpPost("Save")]
        public async Task CreateCodeVerify(CodeVerify codeVerify)
        {
            var user = await _userManager.FindByEmailAsync(codeVerify.Email);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Fetch any existing verification codes for the user
            var existingVerifications = _context.CodeVerifies.Where(v => v.UserId == user.Id);

            // If any exist, remove them
            if (existingVerifications.Any())
            {
                _context.CodeVerifies.RemoveRange(existingVerifications);
                await _context.SaveChangesAsync();
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
        }

        // POST: api/CodeVerifies/resetpassword
        [HttpPost("resetpassword")]
        public async Task<ActionResult<bool>> ResetPassword([FromBody] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Generate a 6-digit random number
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();

            var verify = new CodeVerify
            {
                Email = email,
                Code = code,
            };

            await CreateCodeVerify(verify);

            var message = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                        }}
                        h2 {{
                            color: #333;
                        }}
                        p {{
                            color: #666;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            color: #ffffff;
                            background-color: #007BFF;
                            border: none;
                            border-radius: 5px;
                            text-decoration: none;
                        }}
                    </style>
                </head>
                <body>
                    <h2>Reset your password</h2>
                    <p>We received a request to reset your password. Your confirmation code is: {code}. Enter this code in your open browser window to reset your password.</p>
                    <p>If you did not request this email, there is nothing to worry about; you can safely ignore it.</p>

                    <img src='https://img.freepik.com/free-vector/bird-colorful-gradient-design-vector_343694-2506.jpg' width='120' height='70' alt='Sky' />

                    <div>
                        <a href='https://skyhq.com'>Our blog</a> |
                        <a href='https://sky.com/legal'>Policies</a> |
                        <a href='https://sky.com/help'>Help center</a> |
                        <a href='https://sky.com/community'>EduHunt Community</a>
                    </div>

                    <p>
                        ©2024 Sky Technologies, LLC, a Salesforce company. <br />
                        5990 Howard Street, San Francisco, CA 941076, USA <br />
                        <br />
                        All rights reserved.
                    </p>
                </body>
                </html>";

            // Please replace SendMail.SendEmail with your own method to send emails
            SendMail.SendEmail(email, "Reset your password", message, "");

            return Ok(true);
        }


        // GET: api/CodeVerifies/Verify
        [HttpPost("Verify")]
        public async Task<ActionResult<bool>> VerifyCode(CodeVerify codeVerify)
        {
            codeVerify.Id = new Guid();
            var Verify = await _context.CodeVerifies.FirstOrDefaultAsync(c => c.Email == codeVerify.Email && c.Code == codeVerify.Code);

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
