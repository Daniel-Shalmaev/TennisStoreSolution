using TennisStoreSharedLibrary.DTOs;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Interfaces
{
    public interface IUserAccountService
    {
        Task<LoginResponse> Login(LoginDTO model);
        Task<ServiceResponse> Register(UserDTO model);
    }
}
