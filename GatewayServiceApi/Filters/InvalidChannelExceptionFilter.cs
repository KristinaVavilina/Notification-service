using Core.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GatewayServiceApi.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class InvalidChannelExceptionFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is InvalidChannelException)
        {
            ExceptionFilterHelper.Handle(context, 400);
        }
    }
}
