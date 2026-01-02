using MediatR;
using BookReviews.Application.Features.Books.Dtos;

namespace BookReviews.Application.Features.Books.Queries.GetBookById
{
    public record GetBookByIdQuery(int Id) : IRequest<BookDto>
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public int? PublishedYear { get; set; }
        public int? MinRating { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}