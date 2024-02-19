using SharedClassLibrary.DTOs;
using static SharedClassLibrary.DTOs.ServiceResponses;

namespace SharedClassLibrary.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAccount(UserDTO userDTO);
        Task<GeneralResponse> ChangePassword(string userId, string currentPassword, string newPassword);
        Task<LoginResponse> LoginAccount(LoginDTO loginDTO);
        Task<LoginResponse> LogoutAccount();
    }
}
