namespace MiniLibrary.API.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult(this Result result)
    {
        return result.Match(
            success => Results.Ok(success),
            error => error.Type switch
            {
                ErrorType.Validation => Results.BadRequest(new { error = error.Description, code = error.Code }),
                ErrorType.NotFound => Results.NotFound(new { error = error.Description, code = error.Code }),
                ErrorType.Conflict => Results.Conflict(new { error = error.Description, code = error.Code }),
                ErrorType.Unauthorized => Results.Unauthorized(),
                ErrorType.Forbidden => Results.Forbid(),
                _ => Results.Problem(
                    title: "Server Error",
                    detail: error.Description,
                    statusCode: StatusCodes.Status500InternalServerError)
            });
    }

    public static IResult ToHttpResult(this Result result)
    {
        return result.Match(
            () => Results.NoContent(),
            error => error.Type switch
            {
                ErrorType.Validation => Results.BadRequest(new { error = error.Description, code = error.Code }),
                ErrorType.NotFound => Results.NotFound(new { error = error.Description, code = error.Code }),
                ErrorType.Conflict => Results.Conflict(new { error = error.Description, code = error.Code }),
                ErrorType.Unauthorized => Results.Unauthorized(),
                ErrorType.Forbidden => Results.Forbid(),
                _ => Results.Problem(
                    title: "Server Error",
                    detail: error.Description,
                    statusCode: StatusCodes.Status500InternalServerError)
            });
    }
}