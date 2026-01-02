namespace BookReviews.Application.Features.Books.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Genre { get; set; }
        public int PublishedYear { get; set; }
        public int? MinRating { get; set; }
        public string Author { get; set; } = string.Empty;
    }
}