using TennisStoreSharedLibrary.DTOs;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Services
{
    public interface IUserAccountService
    {
        Task<LoginResponse> Login(LoginDTO model);
        Task<ServiceResponse> Register(UserDTO model);
    }
}
