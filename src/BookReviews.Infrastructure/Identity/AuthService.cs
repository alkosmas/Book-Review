using BookReviews.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using BookReviews.Application.Common.Exceptions;

namespace BookReviews.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
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
           throw new NotImplementedException();
        }
    }
}