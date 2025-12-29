using MediatR;
using BookReviews.Application.Common.Interfaces;
using System.Threading.Tasks;

namespace BookReviews.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand , string>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LoginAsync(
                request.Email, 
                request.Password
            );
        }
    }
}