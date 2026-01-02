using MediatR;
using Microsoft.EntityFrameworkCore;
using BookReviews.Application.Common.Interfaces;
using BookReviews.Application.Common.Exceptions;
using BookReviews.Domain.Entities;

namespace BookReviews.Application.Features.Reviews.Commands.VoteReview
{
    public class VoteReviewCommandHandler : IRequestHandler<VoteReviewCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public VoteReviewCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task Handle(VoteReviewCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId == null)
             throw new UnauthorizedException("User not authenticated");

            var reviewExists = await _context.Reviews
                .AnyAsync(r => r.Id == request.ReviewId, cancellationToken);

            if (!reviewExists)
            {
                throw new NotFoundException(nameof(Review), request.ReviewId);
            }

            var existingVote = await _context.ReviewVotes
                .FirstOrDefaultAsync(v => v.ReviewId == request.ReviewId && v.UserId == userId, cancellationToken);

            if (existingVote != null)
            {
                existingVote.IsUpVote = request.IsUpVote;
                if (existingVote.IsDeleted) 
                    existingVote.IsDeleted = false;
            }
            else
            {
                var newVote = new ReviewVote
                {
                    ReviewId = request.ReviewId,
                    UserId = userId,
                    IsUpVote = request.IsUpVote
                };
                await _context.ReviewVotes.AddAsync(newVote, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}