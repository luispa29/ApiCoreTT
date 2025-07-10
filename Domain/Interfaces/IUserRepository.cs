using Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(UserRequest user);
        Task<bool> ValidEmailUser(UserRequest user);
        Task<bool> Login(LoginRequest user);
    }
}
