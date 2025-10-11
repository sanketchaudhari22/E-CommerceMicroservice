using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interface;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using E_CommerceSharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class UserRepository : IUserInterface
    {
        private readonly AuthenticationDbContext _context;
        private readonly IConfiguration _config;

        public UserRepository(AuthenticationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Case-insensitive email search
        private async Task<AppUser?> GetUserByEmail(string email)
            => await _context.Users
                .FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());

        public async Task<GetUserDto> GetUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            return new GetUserDto(
                user.Id,
                user.Name ?? "",
                user.TelephoneNumber ?? "",
                user.Address ?? "",
                user.Email ?? "",
                user.Password ?? "",
                user.Role ?? "User"
            );
        }

        public async Task<Response> Register(AppUserDto dto)
        {
            var exists = await GetUserByEmail(dto.Email);
            if (exists != null)
                return new Response(false, "Email already registered");

            var user = new AppUser
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                TelephoneNumber = dto.TelephoneNumber,
                Address = dto.Address,
                Role = dto.Role ?? "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new Response(true, "User registered successfully");
        }

        public async Task<Response> Login(LoginDto dto)
        {
            try
            {
                var email = dto.Email?.Trim();
                var user = await GetUserByEmail(email);
                if (user == null)
                {
                    Console.WriteLine("User not found: " + email);
                    return new Response(false, "User not found");
                }

                bool verifyPassword = BCrypt.Net.BCrypt.Verify(dto.Password.Trim(), user.Password.Trim());
                if (!verifyPassword)
                {
                    Console.WriteLine("Invalid password for: " + email);
                    return new Response(false, "Invalid password");
                }

                string token = GenerateToken(user);
                return new Response(true, "Login successful", token);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login exception: " + ex.Message);
                return new Response(false, "Something went wrong: " + ex.Message);
            }
        }


        private string GenerateToken(AppUser user)
        {
            var keyString = _config["Authentication:Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new Exception("JWT Key missing in config");

            var key = Encoding.UTF8.GetBytes(keyString);

            var issuer = _config["Authentication:Issuer"] ?? throw new Exception("JWT Issuer missing");
            var audience = _config["Authentication:Audience"] ?? throw new Exception("JWT Audience missing");

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email ?? "noemail@example.com"),
                new(ClaimTypes.Role, user.Role ?? "User")
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
