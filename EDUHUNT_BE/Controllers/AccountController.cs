using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;

namespace EDUHUNT_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IUserAccount userAccount) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            var response = await userAccount.CreateAccount(userDTO);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await userAccount.LoginAccount(loginDTO);
            return Ok(response);
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(string userId, string currentPassword, string newPassword)
        {
            var response = await userAccount.ChangePassword(userId, currentPassword, newPassword);
            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await userAccount.LogoutAccount();
            return Ok(response);
        }
    }
}
