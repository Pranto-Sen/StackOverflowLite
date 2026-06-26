namespace StackOverflowLite.API.Middleware;

public class ErrorResponse
{
    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public List<ValidationError> Errors { get; set; } = new();
}

public class ValidationError
{
    public string Property { get; set; } = string.Empty;

    public string Error { get; set; } = string.Empty;
}