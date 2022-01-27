using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using API.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IAuthentication _auth;

        public LoginController(DatabaseContext context, IAuthentication auth)
        {
            _context = context;
            _auth = auth;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] User user)
        {
            var dbUser = _context.Users.Where(u=>u.UserName.ToLower().Equals(user.UserName.ToLower())).FirstOrDefault();
            if(dbUser != null){
                var verified = await _auth.VerifyPassword(dbUser, user.Password);
                if(verified)
                {
                    string token = _auth.GenerateToken(dbUser);
                    var response = new 
                    {
                        userName = dbUser.UserName,
                        token
                    };

                    return Ok(response);
                }
                else
                    return StatusCode(401);
            }
            
            return StatusCode(404);
        }
    }
}