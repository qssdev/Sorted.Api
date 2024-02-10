namespace Sorted.Api.Models
{
    public class RainfallModelMessage
    {
        public int StatusCode { get; set; }
        public RainfallReadingResponse? Data { get; set; }
        public ErrorResponse? Error { get; set; }

        public RainfallModelMessage(int statusCode, RainfallReadingResponse? data, ErrorResponse? error)
        {
            StatusCode = statusCode;
            Data = data;
            Error = error;
        }
    }
}
