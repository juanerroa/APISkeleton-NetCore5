using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using API.Models.Auth;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class Authentication : IAuthentication
    {
        private const int SaltByteSize = 32;
        private const int HashByteSize = 32;
        private const int Iterations = 10000;
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        public Authentication(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> EncryptPassword(string password)
        {
            return await Task.Run(() => GetHashedString(password, null));
        }

        public async Task<bool> VerifyPassword(User dbUser, string password)
        {
            string salt = dbUser.Password.Substring(dbUser.Password.IndexOf('=') + 1);
            byte[] saltAsByteArray = Convert.FromBase64String(salt);
            string calculatedHashAndSalt = await Task.Run(() => GetHashedString(password, saltAsByteArray));
            return dbUser.Password == calculatedHashAndSalt;
        }

        public string GenerateToken(User dbUser)
        {
            var claims = new List<Claim>(){
                new Claim(JwtRegisteredClaimNames.NameId, dbUser.UserName),
                //new Claim(ClaimTypes.Role, dbUser.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }

        private string GetHashedString(string input, byte[] saltAsByteArray)
        {
            //Generate salt using a CSPRNG
            if (saltAsByteArray == null || saltAsByteArray.Length < 0)
            {
                using (RandomNumberGenerator random = new RNGCryptoServiceProvider())
                {
                    saltAsByteArray = new byte[SaltByteSize];
                    random.GetNonZeroBytes(saltAsByteArray);
                }
            }

            // Derive password hash using HMACSHA1
            string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input,
                salt: saltAsByteArray,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: Iterations,
                numBytesRequested: HashByteSize));

            string salt = Convert.ToBase64String(saltAsByteArray);
            string hashAndSalt = passwordHash + salt;
            return hashAndSalt;
        }

        public async Task CheckDevUserExist()
        {
            if (!_context.Users.Any(u => u.UserName.ToLower().Equals("developer")))
            {
                try
                {
                    User user = new User
                    {
                        UserName = "Developer",
                        Password = "123456",
                        Email = "developer@mail.com"
                    };
                    user.Password = await EncryptPassword(user.Password);
                    await _context.AddAsync(user);
                    var result = await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("damn");
                }
            }
        }
    }
}