using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Api.Filters;

public static class ExceptionFilterHelper
{
    public static void Handle(ExceptionContext context, int statusCode)
    {
        context.HttpContext.Response.StatusCode = statusCode;
        context.ExceptionHandled = true;
        context.Result = new ContentResult
        {
            Content = context.Exception.Message
        };
    }
}
