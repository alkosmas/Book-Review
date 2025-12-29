using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BookReviews.Domain.Settings;
using BookReviews.Application.Common.Exceptions;
using BookReviews.Application.Common.Interfaces;




namespace BookReviews.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }


        public async Task<Guid> RegisterAsync(string email, string username, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            
            if (existingUser != null)
            {
                throw new BadRequestException($"User with email {email} already exists.");
            }

            var user = new ApplicationUser
            {
                Email = email,
                UserName = username
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errorList = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(string.Join(", ", errorList)); 
            }

            return Guid.Parse(user.Id);
        }
        public async Task<string> LoginAsync(string email, string password)
        {
           var existingUser = await _userManager.FindByEmailAsync(email);
            
            if (existingUser == null)
            {
                throw new BadRequestException("Invalid credentials");
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(existingUser,password);

            if (!isValidPassword)
                throw new BadRequestException("Invalid credentials");

            return GenerateToken(existingUser);

        }

        public string GenerateToken( ApplicationUser user)
        {
            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!)
            };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}