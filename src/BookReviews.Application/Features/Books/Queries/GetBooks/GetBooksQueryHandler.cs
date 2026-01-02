using MediatR;
using Microsoft.EntityFrameworkCore;
using Mapster;
using BookReviews.Application.Common.Interfaces;
using BookReviews.Application.Common.Models;
using BookReviews.Domain.Entities;
using BookReviews.Application.Features.Books.Dtos;
using BookReviews.Application.Features.Books.Queries.GetBooks;

namespace BookReviews.Application.Features.Books.Queries.GetBooks
{
    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, PagedResponse<BookDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetBooksQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(b => b.Title.Contains(request.Title));
            }

            if (!string.IsNullOrWhiteSpace(request.Author))
            {
                query = query.Where(b => b.Author.Contains(request.Author));
            }

            if (!string.IsNullOrWhiteSpace(request.Genre))
            {
                query = query.Where(b => b.Genre == request.Genre);
            }

           /* if (request.PublishedYear.HasValue)
            {
                query = query.Where(b => b.PublishedYear == request.PublishedYear.Value);
            }*/

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectToType<BookDto>()
                .ToListAsync(cancellationToken);

            return new PagedResponse<BookDto>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}