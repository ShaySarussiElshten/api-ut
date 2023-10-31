using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingGarageManagement.Models
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }

        public ApiResponse(T data, string message = null)
        {
            Success = true;
            Data = data;
            Message = message ?? "Operation completed successfully.";
        }

        public ApiResponse(string message)
        {
            Success = false;
            Message = message;
    }
   }

}