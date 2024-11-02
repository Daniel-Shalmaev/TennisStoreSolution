using Microsoft.EntityFrameworkCore;
using TennisStoreServer.Data;
using TennisStoreSharedLibrary.DTOs;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreServer.Repositories
{
    public class UserAccountRepository(AppDbContext appDbContext) : IUserAccount
    {
        public Task<LoginResponse> GetRefreshToken(PostRefreshTokenDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<UserSession> GetUserByToken(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponse> Login(LoginDTO model)
        {
            if (model is null)
                return new LoginResponse(false, "Model is empty");

            var findUser = await appDbContext.UserAccounts
                .FirstOrDefaultAsync(_ => _.Email!.Equals(model!.Email!));

            if (findUser is null)
                return new LoginResponse(false, "User not found");

            if (!BCrypt.Net.BCrypt.Verify(model!.Password, findUser.Password))
                return new LoginResponse(false, "Invalid UserName/Password");

        }

        public async Task<ServiceResponse> Register(UserDTO model)
        {
            if (model is null)
                return new ServiceResponse(false, "Model is empty");

            var findUser = await appDbContext.UserAccounts.
                FirstOrDefaultAsync(_ => _.Email!.ToLower().Equals(model.Email!.ToLower()));

            if (findUser is not null)
                return new ServiceResponse(false, "User Registered already");

            var user = appDbContext.UserAccounts.Add(new UserAccount()
            {
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Name = model.Name,
                Email = model.Email,
            }).Entity;

            await Commit();

            var checkIfAdminIsCreated = await appDbContext.SystemRoles
                .FirstOrDefaultAsync(_ => _.Name!.ToLower().Equals("admin"));

            if (checkIfAdminIsCreated is null)
            {
                var result = appDbContext.SystemRoles.Add(new SystemRole() { Name = "Admin" }).Entity;
                await Commit();

                appDbContext.UserRoles.Add(new UserRole() { RoleId = result.Id, UserId = user.Id });
                await Commit();
            }
            else
            {
                var checkIfUserCreated = await appDbContext.SystemRoles
                    .FirstOrDefaultAsync(_ => _.Name!.ToLower().Equals("user"));

                int RoleId = 0;

                if (checkIfUserCreated is null)
                {
                    var userResult = appDbContext.SystemRoles.Add(new SystemRole() { Name = "User" }).Entity;
                    await Commit();
                    RoleId = userResult.Id;
                }

                appDbContext.UserRoles.Add(new UserRole()
                {
                    RoleId = RoleId == 0 ? checkIfUserCreated!.Id : RoleId,
                    UserId = user.Id
                });
                await Commit();
            }
            return new ServiceResponse(true, "Account created");
        }

        private async Task Commit() => await appDbContext.SaveChangesAsync();
    }
}
