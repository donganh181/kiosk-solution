using System;
using System.Collections.Generic;
using System.Text;

namespace kiosk_solution.Data.Responses
{
    public class ErrorResponse : Exception
    {
        public ErrorDetailResponse Error { get; private set; }
        public ErrorResponse(int errorCode, string message)
        {
            Error = new ErrorDetailResponse
            {
                Message = message,
                Code = errorCode
            };
        }
    }
    public class ErrorDetailResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
