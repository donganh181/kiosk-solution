using System;
using System.Collections.Generic;
using System.Text;

namespace kiosk_solution.Data.Responses
{
    public class SuccessResponse<TEntity> where TEntity : class
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public TEntity Data { get; set; }

        public SuccessResponse(int code, string message, TEntity data)
        {
            Message = message;
            Data = data;
            Code = code;
        }
    }

}
