using EDUHUNT_BE.Data;
using EDUHUNT_BE.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System.Text.Encodings.Web;

namespace EDUHUNT_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserAccount userAccount;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(IUserAccount userAccount, UserManager<ApplicationUser> userManager)
        {
            this.userAccount = userAccount;
            this.userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            var response = await userAccount.CreateAccount(userDTO);

            if (response.Flag)
            {
                var user = await userManager.FindByEmailAsync(userDTO.Email);
                if (user != null)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

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
                        <h2>Confirm your email address</h2>
                        <p>Welcome to EduHunt. Your confirmation code is below - enter it in your open browser window, and we will help you get signed in.</p>
                        <div>
                            <a href='{HtmlEncoder.Default.Encode(confirmationLink)}' class='button'>Verify Email</a>
                        </div>
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
                    SendMail.SendEmail(userDTO.Email, "Confirm your email", message,"");
                }
            }

            return Ok(response);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest();

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return Redirect("http://localhost:3000/login");
            else
                return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await userAccount.LoginAccount(loginDTO);
            return Ok(response);
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword( PasswordDTO passwordDTO)
        {
            if (passwordDTO == null)
                return BadRequest("Password data is null");

            var response = await userAccount.ChangePassword(passwordDTO.Id, passwordDTO.Password, passwordDTO.NewPassword);
            return Ok(response);
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword( PasswordDTO passwordDTO)
        {
            if (passwordDTO == null)
                return BadRequest("Password data is null");

            var response = await userAccount.ForgotPassword(passwordDTO.Email, passwordDTO.NewPassword);
            return Ok(response);
        }
        [HttpPost("loginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle(UserDTO UserDTO)
        {
            var response = await userAccount.LoginWithGoogle(UserDTO);
            return Ok(response);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await userAccount.LogoutAccount();
            return Ok(response);
        }

        [HttpGet("listuser")]
        public async Task<IActionResult> ListUser()
        {
            var response = await userAccount.ListUser();
            return Ok(response);
        }


        [HttpDelete("deleteuser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var response = await userAccount.DeleteUser(id);
            return Ok(response);
        }

    }
}
