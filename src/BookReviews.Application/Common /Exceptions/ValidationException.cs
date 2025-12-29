using FluentValidation.Results; 

namespace BookReviews.Application.Common.Exceptions
{
  public class ValidationException : Exception
{
    public List<string> Errors { get; }

    public ValidationException(ValidationResult validationResult)
    {
        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
    }
    
    public ValidationException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }
}
}