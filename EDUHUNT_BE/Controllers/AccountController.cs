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


        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var response = await userAccount.DeleteUser(id);
            return Ok(response);
        }
    }
}
