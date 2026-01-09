using Core.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GatewayServiceApi.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class NotificationServiceFailedExceptionFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is NotificationServiceFailedException)
        {
            ExceptionFilterHelper.Handle(context, 400);
        }
    }
}
