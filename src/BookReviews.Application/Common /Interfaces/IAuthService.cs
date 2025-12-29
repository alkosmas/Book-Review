
namespace BookReviews.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<Guid> RegisterAsync(string email, string username, string password);
        Task<string> LoginAsync(string email, string password);
    }
}