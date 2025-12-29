using MediatR;

namespace BookReviews.Application.Features.Books.Commands.CreateBook
{
    public class CreateBookCommand : IRequest<int> 
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public string Genre { get; set; } = string.Empty;
    }
}