using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using API.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IAuthentication _auth;
        public UsersController(DatabaseContext context, IAuthentication auth)
        {
            _context = context;
            _auth = auth;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            var response = new
            {
                success = false,
                message = String.Empty
            };

            try
            {
                user.Password = await _auth.EncryptPassword(user.Password);
                await _context.AddAsync(user);
                var result = await _context.SaveChangesAsync();
                
                if(result > 0)
                {
                    response = new
                    {
                        success = true,
                        message = String.Empty
                    };
                    
                    return CreatedAtAction(nameof(Create), response);
                }
                else
                    return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                //ex message should be logged...
                return StatusCode(500, response);
            }
        }
    }
}