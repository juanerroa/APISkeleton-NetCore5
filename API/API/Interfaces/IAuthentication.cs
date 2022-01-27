using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models.Auth;

namespace API.Interfaces
{
    public interface IAuthentication
    {
        Task<string> EncryptPassword(string password);
        Task<bool> VerifyPassword(User dbUser, string password);
        string GenerateToken(User dbUser);
        Task CheckDevUserExist();
    }
}