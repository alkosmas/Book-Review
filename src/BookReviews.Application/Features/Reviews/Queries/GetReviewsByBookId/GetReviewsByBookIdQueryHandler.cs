using MediatR;
using Microsoft.EntityFrameworkCore;
using BookReviews.Application.Common.Interfaces;
using BookReviews.Application.Features.Reviews.Dtos;

namespace BookReviews.Application.Features.Reviews.Queries.GetReviewsByBookId
{
    public class GetReviewsByBookIdQueryHandler : IRequestHandler<GetReviewsByBookIdQuery, List<ReviewDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetReviewsByBookIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReviewDto>> Handle(GetReviewsByBookIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Reviews
                .Where(r => r.BookId == request.BookId)
                .Include(r => r.ReviewVotes) 
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Content = r.Content,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt,
                    UserName = "User_" + r.UserId.Substring(0, Math.Min(8, r.UserId.Length)),
                    Likes = r.ReviewVotes.Count(v => v.IsUpVote),
                    Dislikes = r.ReviewVotes.Count(v => !v.IsUpVote)
                })
                .ToListAsync(cancellationToken);
        }
    }
}