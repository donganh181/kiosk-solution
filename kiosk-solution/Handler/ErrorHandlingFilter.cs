using kiosk_solution.Data.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiosk_solution.Handler
{
    public class ErrorHandlingFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is System.Linq.Dynamic.Core.Exceptions.ParseException || context.Exception is ErrorResponse)
            {
                string message = context.Exception.ToString();
                if (context.Exception.GetType() == typeof(ErrorResponse)) message = ((ErrorResponse)context.Exception).Message;
                context.Result = new ObjectResult(new ErrorResponse(((ErrorResponse)context.Exception).Code, message))
                {
                    StatusCode = ((ErrorResponse)context.Exception).Code
                };
                context.ExceptionHandled = true;
                return;
            }
        }
    }
}
