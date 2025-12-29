using MediatR;
using BookReviews.Application.Common.Interfaces;
using System.Threading.Tasks;

namespace BookReviews.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand,Guid>
    {
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RegisterAsync(
                request.Email,
                request.Username,
                request.Password
            );
        }
        
    }
}