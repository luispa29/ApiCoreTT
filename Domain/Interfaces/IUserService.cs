using Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateUser(UserRequest user);
        Task<string> Login(LoginRequest user);

    }
}
