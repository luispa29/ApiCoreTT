using Domain.Interfaces;
using Model.Request;

namespace Application
{
    public class UserService(IUserRepository _userRepo, ITokenService _token) : IUserService
    {
        public async Task<bool> CreateUser(UserRequest user)
        {
            try
            {
                if (await _userRepo.ValidEmailUser(user)) return false;

                return await _userRepo.CreateUser(user);
            }
            catch (Exception) { throw; }
        }

        public async Task<string> Login(LoginRequest user)
        {
            try
            {
                bool result = await _userRepo.Login(user);

                if (result) return _token.GenereToken(user.Email);


                return string.Empty;
            }
            catch (Exception) { throw; }
        }


    }
}
