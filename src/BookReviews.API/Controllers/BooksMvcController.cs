using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using BookReviews.Application.Features.Books.Queries.GetBooks;
using BookReviews.Application.Features.Books.Commands.CreateBook;
using BookReviews.Application.Features.Books.Commands.UpdateBook;
using BookReviews.Application.Features.Reviews.Commands.CreateReview;
using BookReviews.Application.Features.Books.Queries.GetBookById;
using BookReviews.Application.Features.Reviews.Queries.GetReviewsByBookId;
using BookReviews.Application.Features.Reviews.Commands.VoteReview;

namespace BookReviews.API.Controllers
{
    public class BooksMvcController : Controller 
    {
        private readonly IMediator _mediator;

        public BooksMvcController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("books")] 
        public async Task<IActionResult> Index([FromQuery] GetBooksQuery query)
        {
            var result = await _mediator.Send(query);
            return View(result); 
        }

        [HttpGet("books/create")]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("books/create")]
        [Authorize]
        public async Task<IActionResult> Create(CreateBookCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }

            try
            {
                var bookId = await _mediator.Send(command);
                return RedirectToAction(nameof(Details), new { id = bookId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(command);
            }
        }

        [HttpGet("books/edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var bookDto = await _mediator.Send(new GetBookByIdQuery(id));
            
            var command = new UpdateBookCommand 
            { 
                Id = bookDto.Id, 
                Title = bookDto.Title, 
                Author = bookDto.Author, 
                Genre = bookDto.Genre, 
                PublishedYear = bookDto.PublishedYear 
            };
            
            return View(command);
        }

        [HttpPost("books/edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, UpdateBookCommand command)
        {
            if (id != command.Id) return BadRequest();

            if (!ModelState.IsValid) return View(command);

            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("books/details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));
            
            var reviews = await _mediator.Send(new GetReviewsByBookIdQuery(id));
            
            var viewModel = new BookDetailsViewModel
            {
                Book = book,
                Reviews = reviews
            };
            
            return View(viewModel);
        }

        [HttpPost("books/details/{id}/review")]
        [Authorize] 
        public async Task<IActionResult> AddReview(int id, CreateReviewCommand command)
        {
            if (id != command.BookId) return BadRequest();

            try {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Details), new { id });
            }
            catch {
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        [HttpPost("books/review/{reviewId}/vote")]
        [Authorize]
        public async Task<IActionResult> Vote(int reviewId, int bookId, bool isUpVote)
        {
            await _mediator.Send(new VoteReviewCommand { ReviewId = reviewId, IsUpVote = isUpVote });
            return RedirectToAction(nameof(Details), new { id = bookId });
        }
    }
}