using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class UserService(IUserRepository _userRepo, ITokenService _token) : IUserService
    {
        public async Task<bool> CreateUser(UserRequest user)
        {
            try
            {
                if(await _userRepo.ValidEmailUser(user))
                {
                    return false;
                }

                return await _userRepo.CreateUser(user);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while creating the user.", ex);
            }
        }

        public async Task<string> Login(LoginRequest user)
        {
            try
            {
               bool result = await _userRepo.Login(user);
                if (result)
                {
                    return  _token.GenereToken(user.Email);
                }

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
