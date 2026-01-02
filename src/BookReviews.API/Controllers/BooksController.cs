using Microsoft.AspNetCore.Mvc;
using MediatR;
using BookReviews.Application.Features.Books.Commands.CreateBook;
using BookReviews.Application.Features.Books.Commands.UpdateBook;
using BookReviews.Application.Features.Books.Queries.GetBooks;
using BookReviews.Application.Features.Books.Queries.GetBookById;
using Microsoft.AspNetCore.Authorization;
using BookReviews.Application.Features.Books.Commands.DeleteBook;



[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }
    
  
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetBooksQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetBookByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBookCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(); 
        }
            await _mediator.Send(command);
            return NoContent(); 
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteBookCommand { Id = id });
        return NoContent();
    }
}