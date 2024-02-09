﻿using EDUHUNT_BE.Data;
using EDUHUNT_BE.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System.Data;
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




    public class AccountRepository(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config
        ) : IUserAccount
    {

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

            //Assign Default Role : 1: User, 2: Scholarship Provider, 3: Mentor 
            switch (userDTO.roleId)
            {
                case 1:
                    await AssignRole(newUser, "User");
                    break;
                case 2:
                    await AssignRole(newUser, "Scholarship Provider");
                    break;
                case 3:
                    await AssignRole(newUser, "Mentor");
                    break;
                default:
                    await AssignRole(newUser, "Admin");
                    break;
            }
            return new GeneralResponse(true, "Account Created");
        }

        private async Task AssignRole(ApplicationUser user, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role is null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = roleName });
                await userManager.AddToRoleAsync(user, roleName);
            }
            else
            {
                await userManager.AddToRoleAsync(user, roleName);
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
            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.FirstOrDefault());
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
    }
}
