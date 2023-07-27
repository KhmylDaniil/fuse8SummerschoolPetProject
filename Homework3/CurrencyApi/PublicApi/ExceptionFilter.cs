using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi
{
    /// <summary>
    /// Фильтр обработки исключений
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ApiRequestLimitException:
                    _logger.Error(context.Exception.Message, context.Exception);
                    context.Result = new ObjectResult(context.Exception.Message) { StatusCode = StatusCodes.Status429TooManyRequests};
                    break;
                case CurrencyNotFoundException:
                    context.Result = new ObjectResult(context.Exception.Message) { StatusCode = StatusCodes.Status404NotFound };
                    break;
                default:
                    _logger.Error(context.Exception.Message, context.Exception);
                    context.Result = new ObjectResult(context.Exception.Message) { StatusCode = StatusCodes.Status500InternalServerError };
                    break;
            }
        }

        //public void OnException(ExceptionContext context)
        //{
        //    var exceptionCodeString = context.Exception.GetType().GetProperty("StatusCode").GetValue(context.Exception).ToString();

        //    switch (exceptionCodeString)
        //    {
        //        case "TooManyRequests":
        //            _logger.Error(context.Exception.Message, context.Exception);
        //            break;
        //        case "UnprocessableEntity":
        //            context.Result = new ObjectResult(context.Exception.Message) { StatusCode = StatusCodes.Status404NotFound };
        //            break;
        //        default:
        //            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        //            _logger.Error(context.Exception.Message, context.Exception);
        //            break;
        //    }
        //}
    }
}
