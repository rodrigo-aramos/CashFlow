using System.Net;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace CashFlow.Domain.DTO.Response
{
    public class DefaultDtoResponse<T>
    {
        public bool Success { get; set; } = true;
        public T Result { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public int StatusCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<string> Errors { get; set; } = new List<string>();

        public DefaultDtoResponse(HttpStatusCode statusCode, T result = default)
        {            
            StatusCode = (int)statusCode;
            Success = StatusCode >= 200 && StatusCode <= 299;
            Result = result;
        }

        public DefaultDtoResponse<T> AddErrorMessage(string message)
        {
            message = (!string.IsNullOrWhiteSpace(message)) ? message : "Falha catastrófica";
            Errors.Add(message);
            return this;
        }
    }
}
