using EDUHUNT_BE.Data;
using EDUHUNT_BE.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static SharedClassLibrary.DTOs.ServiceResponses;

namespace EDUHUNT_BE.Repositories
{
    public class AccountRepository : IUserAccount
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;
        private readonly AppDbContext _context; // Inject the AppDbContext

        public AccountRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config,
            AppDbContext context) // Inject the AppDbContext through the constructor
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.config = config;
            _context = context; // Assign the injected AppDbContext to the private field
        }




        public async Task<GeneralResponse> CreateAccount(UserDTO userDTO)
        {
            if (userDTO is null) return new GeneralResponse(false, "Model is empty");
            var newUser = new ApplicationUser()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                PasswordHash = userDTO.Password,
                UserName = userDTO.Email
            };
            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user is not null) return new GeneralResponse(false, "User registered already");

            var createUser = await userManager.CreateAsync(newUser, userDTO.Password);

            if (!createUser.Succeeded)
            {
                // Lấy danh sách lỗi từ quá trình tạo người dùng
                var errors = createUser.Errors.Select(e => e.Description);

                // Gia nhập danh sách lỗi thành một chuỗi
                var errorMessage = string.Join(", dau ne", errors);

                // Trả về thông báo lỗi
                return new GeneralResponse(false, errorMessage);
            }

            var profile = new Profile
            {
                UserId = Guid.Parse(newUser.Id),
                // Assign other properties as needed
            };

            _context.Profile.Add(profile);
            await _context.SaveChangesAsync();
            //Assign Default Role : Admin to first registrar; rest is user
            var checkAdmin = await roleManager.FindByNameAsync("Admin");
            if (checkAdmin is null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await userManager.AddToRoleAsync(newUser, "Admin");
                return new GeneralResponse(true, "Account Created");
            }
            else
            {
                var checkUser = await roleManager.FindByNameAsync("User");
                if (checkUser is null)
                    await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                await userManager.AddToRoleAsync(newUser, "User");
                return new GeneralResponse(true, "Account Created");
            }


          




        }

        public async Task<LoginResponse> LoginAccount(LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return new LoginResponse(false, null!, null!, "Login container is empty");

            var getUser = await userManager.FindByEmailAsync(loginDTO.Email);
            if (getUser is null)
                return new LoginResponse(false, null!, null!, "User not found");

            bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, loginDTO.Password);
            if (!checkUserPasswords)
                return new LoginResponse(false, null!, null!, "Invalid email/password");

            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());
            string token = GenerateToken(userSession);
            return new LoginResponse(true, token!, getUser.Id, "Login completed");
        }

        public Task<LoginResponse> LogoutAccount()
        {
            throw new NotImplementedException();
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<List<ListUserDTO>> ListUser()
        {
            var users = await userManager.Users.ToListAsync();
            var userDTOs = await Task.WhenAll(users.Select(async u => new ListUserDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = (List<string>)await userManager.GetRolesAsync(u)
            }));
            return userDTOs.ToList();
        }


        public async Task<DeleteUserResponse> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null) return new DeleteUserResponse(false, "User not found");
            var deleteUser = await userManager.DeleteAsync(user);
            if (!deleteUser.Succeeded)
            {
                var errors = deleteUser.Errors.Select(e => e.Description);
                var errorMessage = string.Join(", ", errors);
                return new DeleteUserResponse(false, errorMessage);
            }
            return new DeleteUserResponse(true, "User deleted");
        }
     

    }
}
